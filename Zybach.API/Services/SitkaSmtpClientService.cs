using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace Zybach.API.Services
{
    public class SitkaSmtpClientService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly ZybachConfiguration _zybachConfiguration;

        private static ILogger _logger { get; set; }

        public SitkaSmtpClientService(ISendGridClient sendGridClient, IOptions<ZybachConfiguration> zybachConfiguration, ILogger logger)
        {
            _sendGridClient = sendGridClient;
            _zybachConfiguration = zybachConfiguration.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends an email including mock mode and address redirection  <see cref="ZybachConfiguration.SITKA_EMAIL_REDIRECT"/>, then calls onward to <see cref="SendDirectly"/>
        /// </summary>
        /// <param name="message"></param>
        public async Task Send(MailMessage message)
        {
            var messageWithAnyAlterations = AlterMessageIfInRedirectMode(message);
            var messageAfterAlterationsAndCreatingAlternateViews = CreateAlternateViewsIfNeeded(messageWithAnyAlterations);
            await SendDirectly(messageAfterAlterationsAndCreatingAlternateViews);
        }

        private static MailMessage CreateAlternateViewsIfNeeded(MailMessage message)
        {
            if (!message.IsBodyHtml)
            {
                return message;
            }
            // Define the plain text alternate view and add to message
            const string plainTextBody = "You must use an email client that supports HTML messages";

            var plainTextView = AlternateView.CreateAlternateViewFromString(plainTextBody, null, MediaTypeNames.Text.Plain);

            message.AlternateViews.Add(plainTextView);

            // Define the html alternate view with embedded image and
            // add to message. To reference images attached as linked
            // resources from your HTML message body, use "cid:contentID"
            // in the <img> tag...
            var htmlBody = message.Body;

            var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            message.AlternateViews.Add(htmlView);


            return message;
        }


        /// <summary>
        /// Sends an email message at a lower level than <see cref="Send"/>, skipping mock mode and address redirection  <see cref="ZybachConfiguration.SITKA_EMAIL_REDIRECT"/>
        /// </summary>
        /// <param name="mailMessage"></param>
        public async Task SendDirectly(MailMessage mailMessage)
        {
            var defaultEmailFrom = GetDefaultEmailFrom();
            var sendGridMessage = new SendGridMessage()
            {
                From = new EmailAddress(defaultEmailFrom.Address, defaultEmailFrom.DisplayName),
                Subject = mailMessage.Subject,
                PlainTextContent = mailMessage.Body,
                HtmlContent = mailMessage.IsBodyHtml ? mailMessage.Body : null
            };

            sendGridMessage = AddUniqueSendGridRecipientsFromMailMessage(sendGridMessage, mailMessage);

            var response = await _sendGridClient.SendEmailAsync(sendGridMessage);

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.Error($"Encountered {response.StatusCode} status code sending email. Email sent response response body of \"{responseBody}\"");
            }
            
        }
        /// <summary>
        /// SendGrid was giving errors with mail messages that contained the same addresses in the to, cc, or bcc fields.
        /// This method makes sure email addresses are unique in prioritized order by to -> cc -> bcc
        /// </summary>
        /// <param name="sendGridMessage"></param>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        private SendGridMessage AddUniqueSendGridRecipientsFromMailMessage(SendGridMessage sendGridMessage, MailMessage mailMessage)
        {
            var addedEmailAddresses = new List<string>();

            var distinctToAddresses = mailMessage.To.DistinctBy(x => x.Address).Select(x => new EmailAddress(x.Address, x.DisplayName)).ToList();
            if (distinctToAddresses.Any())
            {
                sendGridMessage.AddTos(distinctToAddresses);
                addedEmailAddresses.AddRange(distinctToAddresses.Select(x => x.Email));
            }

            if (mailMessage.CC.Any())
            {
                var distinctCcAddresses = mailMessage.CC.DistinctBy(x => x.Address)
                    .Where(x => !addedEmailAddresses.Contains(x.Address))
                    .Select(x => new EmailAddress(x.Address, x.DisplayName)).ToList();
                if (distinctCcAddresses.Any())
                {
                    sendGridMessage.AddCcs(distinctCcAddresses);
                    addedEmailAddresses.AddRange(distinctCcAddresses.Select(x => x.Email));
                }
            }

            if (mailMessage.Bcc.Any())
            {
                var distinctBccAddresses = mailMessage.Bcc.DistinctBy(x => x.Address)
                    .Where(x => !addedEmailAddresses.Contains(x.Address))
                    .Select(x => new EmailAddress(x.Address, x.DisplayName)).ToList();
                if (distinctBccAddresses.Any())
                {
                    sendGridMessage.AddBccs(distinctBccAddresses);
                    addedEmailAddresses.AddRange(distinctBccAddresses.Select(x => x.Email));
                }

            }

            return sendGridMessage;
        }


        /// <summary>
        /// Alter message TO, CC, BCC if the setting <see cref="ZybachConfiguration.SITKA_EMAIL_REDIRECT"/> is set
        /// Appends the real to the body
        /// </summary>
        /// <param name="realMailMessage"></param>
        /// <returns></returns>
        private MailMessage AlterMessageIfInRedirectMode(MailMessage realMailMessage)
        {
            var redirectEmail = _zybachConfiguration.SITKA_EMAIL_REDIRECT;
            var isInRedirectMode = !String.IsNullOrWhiteSpace(redirectEmail);

            if (!isInRedirectMode)
            {
                return realMailMessage;
            }

            ClearOriginalAddressesAndAppendToBody(realMailMessage, "To", realMailMessage.To);
            ClearOriginalAddressesAndAppendToBody(realMailMessage, "CC", realMailMessage.CC);
            ClearOriginalAddressesAndAppendToBody(realMailMessage, "BCC", realMailMessage.Bcc);

            realMailMessage.To.Add(redirectEmail);

            return realMailMessage;
        }

        private static void ClearOriginalAddressesAndAppendToBody(MailMessage realMailMessage, string addressType, ICollection<MailAddress> addresses)
        {
            var newline = realMailMessage.IsBodyHtml ? "<br />" : Environment.NewLine;
            var separator = newline + "\t";

            var toExpected = addresses.Aggregate(String.Empty, (s, mailAddress) => s + Environment.NewLine + "\t" + mailAddress.ToString());
            if (!String.IsNullOrWhiteSpace(toExpected))
            {
                var toAppend =
                    $"{newline}{separator}Actual {addressType}:{(realMailMessage.IsBodyHtml ? toExpected.HtmlEncodeWithBreaks() : toExpected)}";
                realMailMessage.Body += toAppend;

                for (var i = 0; i < realMailMessage.AlternateViews.Count; i++)
                {
                    var stream = realMailMessage.AlternateViews[i].ContentStream;
                    using (var reader = new StreamReader(stream))
                    {
                        var alternateBody = reader.ReadToEnd();
                        alternateBody += toAppend;
                        var newAlternateView = AlternateView.CreateAlternateViewFromString(alternateBody, null, realMailMessage.AlternateViews[i].ContentType.MediaType);
                        realMailMessage.AlternateViews[i].LinkedResources.ToList().ForEach(x => newAlternateView.LinkedResources.Add(x));
                        realMailMessage.AlternateViews[i] = newAlternateView;
                    }
                }


            }
            addresses.Clear();
        }

        private static string FlattenMailAddresses(IEnumerable<MailAddress> addresses)
        {
            return String.Join("; ", addresses.Select(x => x.ToString()));
        }

        public string GetDefaultEmailSignature()
        {
            string defaultEmailSignature = $@"<br /><br />
Respectfully, the {_zybachConfiguration.PlatformLongName} team
<br /><br />
***
<br /><br />
You have received this email because you are a registered user of the {_zybachConfiguration.PlatformLongName}. 
<br /><br />
<a href=""mailto:{_zybachConfiguration.SupportEmail}"">{_zybachConfiguration.SupportEmail}</a>";
            return defaultEmailSignature;
        }

        public string GetSupportNotificationEmailSignature()
        {
            string supportNotificationEmailSignature = $@"<br /><br />
Respectfully, the {_zybachConfiguration.PlatformLongName} team
<br /><br />
***
<br /><br />
You have received this email because you are assigned to receive support notifications within the {_zybachConfiguration.PlatformLongName}. 
<br /><br />
<a href=""mailto:{_zybachConfiguration.SupportEmail}"">{_zybachConfiguration.SupportEmail}</a>";
            return supportNotificationEmailSignature;
        }

        public MailAddress GetDefaultEmailFrom()
        {
            return new MailAddress("donotreply@sitkatech.net", $"{_zybachConfiguration.PlatformLongName}");
        }

        public static void AddBccRecipientsToEmail(MailMessage mailMessage, IEnumerable<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                mailMessage.Bcc.Add(recipient);
            }
        }

        public static void AddCcRecipientsToEmail(MailMessage mailMessage, IEnumerable<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                mailMessage.CC.Add(recipient);
            }
        }
    }
}