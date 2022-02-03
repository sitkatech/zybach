using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharpDocx;
using Zybach.API.ReportTemplates.Models;
using Zybach.EFModels.Entities;

namespace Zybach.API.ReportTemplates
{
    public class ReportTemplateGenerator
    {
        public const string TemplateTempDirectoryName = "ReportTemplates";
        public const string TemplateTempImageDirectoryName = "Images";
        public const int TemplateTempDirectoryFileLifespanInDays = 2;
        protected ReportTemplate ReportTemplate { get; set; }
        protected ReportTemplateModelEnum ReportTemplateModelEnum { get; set; }
        protected ReportTemplateModelTypeEnum ReportTemplateModelTypeEnum { get; set; }
        protected List<int> SelectedModelIDs { get; set; }
        protected string FullTemplateTempDirectory { get; set; }
        protected string FullTemplateTempImageDirectory { get; set; }

        /// <summary>
        /// ReportTemplateUniqueIdentifier is used for file names in the TEMP directory.
        /// </summary>
        protected Guid ReportTemplateUniqueIdentifier { get; set; }
        
        public ReportTemplateGenerator(ReportTemplate reportTemplate, List<int> selectedModelIDs)
        {
            ReportTemplate = reportTemplate;
            ReportTemplateModelEnum = (ReportTemplateModelEnum) reportTemplate.ReportTemplateModelID;
            ReportTemplateModelTypeEnum = (ReportTemplateModelTypeEnum) reportTemplate.ReportTemplateModelTypeID;
            SelectedModelIDs = selectedModelIDs;
            ReportTemplateUniqueIdentifier = Guid.NewGuid();
            InitializeTempFolders(new DirectoryInfo(Path.GetTempPath()));
        }

        private void InitializeTempFolders(DirectoryInfo directoryInfo)
        {
            var tempPath = directoryInfo;
            var baseTempDirectory = new DirectoryInfo($"{tempPath.FullName}{TemplateTempDirectoryName}");
            baseTempDirectory.Create();
            FullTemplateTempDirectory = baseTempDirectory.FullName;
            FullTemplateTempImageDirectory = baseTempDirectory.CreateSubdirectory(TemplateTempImageDirectoryName).FullName + '/';
        }

        public void Generate(ZybachDbContext dbContext, VegaRenderService.VegaRenderService vegaRenderService)
        {
            var templatePath = GetTemplatePath();
            DocxDocument document;
            SaveTemplateFileToTempDirectory();

            // todo: if someone generates a report with all wells, the resulting .docx can get up to 3gb+ depending on the tenant, how do we want to handle this situation?
            if (TemplateHasImages(templatePath))
            {
                SaveImageFilesToTempDirectory(dbContext, vegaRenderService);
            }

            // Word will insert hidden bookmarks apparently. Bookmarks seem to cause a good amount of issues with the generation
            // and the error is extremely confusing for the user. This might make it so user's cannot make templates with bookmarks
            // but this seems very necessary - 6/26/2020 SMG
            RemoveBookmarks(templatePath);

            switch (ReportTemplateModelEnum)
            {
                case ReportTemplateModelEnum.ChemigationPermit:
                    var chemigationPermitDetailedBaseViewModel = new ReportTemplateChemigationPermitDetailedBaseViewModel()
                    {
                        ReportModel = GetListOfChemigationPermitDetailedModels(dbContext)
                    };
                    document = DocumentFactory.Create<DocxDocument>(templatePath, chemigationPermitDetailedBaseViewModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var compilePath = GetCompilePath();
            document.ImageDirectory = FullTemplateTempImageDirectory;
            document.Generate(compilePath);

            CleanTempDirectoryOfOldFiles(FullTemplateTempDirectory);
        }

        private void RemoveBookmarks(string templatePath)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(templatePath, true))
            {
                var bs = wordDoc.MainDocumentPart.Document
                    .Descendants<BookmarkStart>()
                    .ToList();
                foreach (var s in bs)
                    s.Remove();

                var be = wordDoc.MainDocumentPart.Document
                    .Descendants<BookmarkEnd>()
                    .ToList();
                foreach (var e in be)
                    e.Remove();
            }
        }

        /// <summary>
        /// Simple regex to test to see if a word document template has any Image() methods in it.
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        private static bool TemplateHasImages(string templatePath)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(templatePath, true))
            {
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    var docText = sr.ReadToEnd();
                    var regexForImage = @"Image\(";
                    var match = Regex.Match(docText, regexForImage);
                    return match.Success;
                }
            }
        }

        /// <summary>
        /// Because Sharpdocx uses directories for images we need to save the images that can be used with the chosen model into a directory that can be accessed
        /// when the report generates. This allows us to create a helper on the ReportTemplateProjectImage model that can then call Image() and pass in the
        /// same file name (that uses the file resource unique GUID)
        /// </summary>
        private async void SaveImageFilesToTempDirectory(ZybachDbContext dbContext, VegaRenderService.VegaRenderService vegaRenderService)
        {
             switch (ReportTemplateModelEnum)
            {
                case ReportTemplateModelEnum.ChemigationPermit:
                    var chemigationPermitsList = dbContext.ChemigationPermits.Where(x => SelectedModelIDs.Contains(x.ChemigationPermitID)).ToList();
                    foreach (var chemigationPermit in chemigationPermitsList)
                    {
                        var imageByteArray = await vegaRenderService.PrintPNG(ChemigationPermits.TempHardCodedVegaSpec(chemigationPermit.ChemigationPermitID));
                        var imagePath = $"{FullTemplateTempImageDirectory}/{chemigationPermit.ChemigationPermitID}";
                        File.WriteAllBytes($"{imagePath}.png", imageByteArray);
                        //CorrectImageProblemsAndSaveToDisk(projectImage, imagePath);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// In testing the sharpdocx image functionality at least two issues with images uploaded to ProjectFirma came up
        /// 1. Encountered an image with a corrupt color profile
        /// 2. Encountered an image with no DPI set
        ///
        /// In case #1, this caused caused the generation to crash
        /// In case #2, this caused the the image in the OpenXML for the .docx to have invalid x and y extents, corrupting the .docx file
        ///
        /// It is likely that doing this will fix other potential issues with uploaded images to ProjectFirma
        ///
        /// We can also take the opportunity here to do some scaling of the images so that they don't need to generate massive images files that have been uploaded
        /// to ProjectFirma
        ///
        /// todo: let the owner of the SharpDocx repository know about these issues to be able to set defaults there instead
        /// </summary>
        //private static void CorrectImageProblemsAndSaveToDisk(ProjectImage projectImage, string imagePath)
        //{
        //    // in order to save time on subsequent reports, we should check to see if the file already exists at the path and return early
        //    var fileInfo = new FileInfo(imagePath);
        //    if (fileInfo.Exists)
        //    {
        //        return;
        //    }

        //    using (var ms = new MemoryStream(projectImage.FileResource.FileResourceData.Data))
        //    {
        //        var bitmap = new Bitmap(ms);
        //        using (Bitmap newBitmap = new Bitmap(bitmap))
        //        {
        //            newBitmap.Save(imagePath, ImageFormat.Png);
        //        }
        //    }
        //}

        private void CleanTempDirectoryOfOldFiles(string targetDirectory)
        {
            if (Directory.Exists(targetDirectory))
            {
                var fileEntries = Directory.GetFiles(targetDirectory);
                foreach (string fileName in fileEntries)
                    DeleteFileIfOlderThanLifespan(fileName);
                var directories = Directory.GetDirectories(targetDirectory);
                foreach (string directory in directories)
                    CleanTempDirectoryOfOldFiles(directory);
            }
        }

        private void DeleteFileIfOlderThanLifespan(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if(fileInfo.LastAccessTime < DateTime.Now.AddDays(-TemplateTempDirectoryFileLifespanInDays))
                fileInfo.Delete();
        }

        /// <summary>
        /// Stores the uploaded ReportTemplate .docx file in the temp directory.
        /// </summary>
        private void SaveTemplateFileToTempDirectory()
        {
            var filePath = GetTemplatePath();
            File.WriteAllBytes(filePath, ReportTemplate.FileResource.FileResourceData);
        }

        /// <summary>
        /// Get the intended template path for this ReportTemplate. 
        /// </summary>
        /// <returns></returns>
        private string GetTemplatePath()
        {
            var fileName = new FileInfo($"{FullTemplateTempDirectory}/{ReportTemplateUniqueIdentifier}-{ReportTemplate.FileResource.OriginalBaseFilename}");
            fileName.Directory.Create();
            return fileName.FullName;
        }
        
        /// <summary>
        /// Get the compile path for this ReportTemplate that the .docx template will compile to.
        /// </summary>
        /// <returns></returns>
        public string GetCompilePath()
        {
            var fileName = new FileInfo($"{FullTemplateTempDirectory}/{ReportTemplateUniqueIdentifier}-generated-{ReportTemplate.FileResource.OriginalBaseFilename}");
            fileName.Directory.Create();
            return fileName.FullName;
        }

        private List<ReportTemplateChemigationPermitDetailedModel> GetListOfChemigationPermitDetailedModels(ZybachDbContext dbContext)
        {
            var listOfModels = ChemigationPermits.ListByPermitIDsAsDetailedDto(dbContext, SelectedModelIDs)
                .OrderBy(x => SelectedModelIDs.IndexOf(x.ChemigationPermitID))
                .Select(x => new ReportTemplateChemigationPermitDetailedModel(x)).ToList();
            return listOfModels;
        }

        public static void ValidateReportTemplate(ReportTemplate reportTemplate, out bool reportIsValid, out string errorMessage, out string sourceCode, ZybachDbContext dbContext, ILogger logger, VegaRenderService.VegaRenderService vegaRenderService)
        {
            errorMessage = "";
            sourceCode = "";

            var reportTemplateModel = (ReportTemplateModelEnum)reportTemplate.ReportTemplateModelID;
            List<int> selectedModelIDs;
            switch (reportTemplateModel)
            {
                case ReportTemplateModelEnum.ChemigationPermit:
                    // select 10 random models to test the report with
                    // SMG 2/17/2020 this can cause problems with templates failing only some of the time, but it feels costly to validate against every single model in the system
                    selectedModelIDs = dbContext.ChemigationPermits.AsNoTracking().Where(x => x.ChemigationPermitAnnualRecords.Count > 0).Select(x => x.ChemigationPermitID).Take(10).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ValidateReportTemplateForSelectedModelIDs(reportTemplate, selectedModelIDs, out reportIsValid, out errorMessage, out sourceCode, logger, dbContext, vegaRenderService);
        }

        public static void ValidateReportTemplateForSelectedModelIDs(ReportTemplate reportTemplate, List<int> selectedModelIDs, out bool reportIsValid, out string errorMessage, out string sourceCode, ILogger logger, ZybachDbContext dbContext, VegaRenderService.VegaRenderService vegaRenderService)
        {
            errorMessage = "";
            sourceCode = "";
            var reportTemplateGenerator = new ReportTemplateGenerator(reportTemplate, selectedModelIDs);
            var tempDirectory = reportTemplateGenerator.GetCompilePath();
            try
            {
                reportTemplateGenerator.Generate(dbContext, vegaRenderService);
                reportIsValid = true;
            }
            catch (SharpDocxCompilationException exception)
            {
                errorMessage = exception.Errors;
                sourceCode = exception.SourceCode;
                reportIsValid = false;
                logger.LogError(
                    $"There was a SharpDocxCompilationException validating a report template. Temporary template file location:\"{tempDirectory}\" Error Message: \"{errorMessage}\". Source Code: \"{sourceCode}\"");
            }
            catch (Exception exception)
            {
                reportIsValid = false;

                // SMG 2/12/2020 submitted an issue on the SharpDocx repo https://github.com/egonl/SharpDocx/issues/13 for better exceptions to be able to refactor this out later.
                switch (exception.Message)
                {
                    case "No end tag found for code.":
                        errorMessage =
                            $"CodeBlockBuilder exception: \"{exception.Message}\". Could not find a matching closing tag \"%>\" for an opening tag.";
                        break;
                    case "TextBlock is not terminated with '<% } %>'.":
                        errorMessage = $"CodeBlockBuilder exception: \"{exception.Message}\".";
                        break;
                    default:
                        errorMessage = exception.Message;
                        break;
                }

                sourceCode = exception.StackTrace;
                logger.LogError(
                    $"There was a SharpDocxCompilationException validating a report template. Temporary template file location:\"{tempDirectory}\". Error Message: \"{errorMessage}\".",
                    exception);
            }
        
        }

    }
}