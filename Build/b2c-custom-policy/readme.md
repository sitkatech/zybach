Setup for this was a bit annoying

1. All of the XML files needed to be updated to have the groundwateraccounting.onmicrosoft.com url in them
2. Needed to create signing key and encryption key from docs here: https://learn.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-user-flows?pivots=b2c-custom-policy&tabs=app-reg-preview
2. Needed to register the IdentityExperienceFramework and ProxyIdentityExperienceFramework applications following the  steps on the above documentation page
3. Update both of the the `client_id` values in the `TrustFrameworkExtensions.xml` file with the clientID for the ProxyIdentityExperienceFramework application registration
4. Update the `IdTokenAudience` and `resource_id` values in the `TrustFrameworkExtensions.xml` file with the clientID for the IdentityExperienceFramework application registration

Docs for next steps: https://learn.microsoft.com/en-us/azure/active-directory-b2c/custom-email-sendgrid?pivots=b2c-custom-policy
1. create a SendGrid api key and store as a policy key
2. create the Dynamic Template in Sendgrid with the html in the documentation https://mc.sendgrid.com/dynamic-templates
3. Record the ID of the template and update the `DisplayControl_TrustFrameworkExtensions.xml` file value for the `template_id` to the sendgrid template id
4. update the `from.email` to `donotreply@sitkatech.net`
5. add and update the `personalizations.0.dynamic_template_data.subject` value to a proper subject for the email

These xml templates were found here: https://github.com/azure-ad-b2c/samples/tree/master/policies/custom-email-verifcation-displaycontrol/policy/SendGrid
as well as the LocalAccounts examples here: https://github.com/Azure-Samples/active-directory-b2c-custom-policy-starterpack/tree/main/LocalAccounts

Then I had to upload these custom policies one at a time in the CORRECT ORDER (because they inherit from eachother)
1. `TrustFrameworkBase.xml`
2. `TrustFrameworkLocalization.xml`
3. `TrustFrameworkExtensions.xml`
4. `DisplayControl_TrustFrameworkExtensions.xml`
5. `DisplayControl_SignUpOrSignin.xml` -- had to adjust the email here to `signInNames.emailAddress`
6. `DisplayControl_PasswordReset.xml`


The email template: `EmailTemplate.html` is what is in Sendgrid currently (1/10/2023), but we can just update the template in Sendgrid. Maybe we can try to keep them synced.

