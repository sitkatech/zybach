using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Zybach.Tests.IntegrationTests.EntityFramework.UserTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.FileResourceTests
{
    public class FileResourceTestClass : UserTestClass
    {
        private static FileResourceDto _newFileResource;

        protected static FileResourceDto NewFileResource
        {
            get
            {
                if (_newFileResource == null)
                {
                    CreateNewFileResource();
                }

                return _newFileResource;
            }
        }

        protected static List<FileResourceDto> FileResourcesToCleanUp = new();

        private static void CreateNewFileResource()
        {
            var fileResource = new FileResource()
            {
                CreateDate = DateTime.Now,
                CreateUserID = NewUser.UserID,
                FileResourceGUID = Guid.NewGuid(),
                OriginalBaseFilename = "test.txt",
                OriginalFileExtension = "txt",
                //FileResourceCanonicalName = "test.txt"
            };

            AssemblySteps.DbContext.FileResources.Add(fileResource);
            AssemblySteps.DbContext.SaveChanges();
            AssemblySteps.DbContext.Entry(fileResource).Reload();

            var fileResourceWithIncludes = AssemblySteps.DbContext.FileResources
                .Include(x => x.CreateUser)
                .First(x => x.FileResourceID == fileResource.FileResourceID);

            _newFileResource = fileResourceWithIncludes.AsDto();
            FileResourcesToCleanUp.Add(_newFileResource);
        }

        [ClassCleanup]
        public new static void ClassCleanup()
        {
            UserTestClass.ClassCleanup();

            if (AssemblySteps.Idempotent)
            {
                foreach (var fileResourceDto in FileResourcesToCleanUp)
                {
                    var fileResource = AssemblySteps.DbContext.FileResources.FirstOrDefault(x => x.FileResourceID == fileResourceDto.FileResourceID);
                    if (fileResource != null)
                    {
                        AssemblySteps.DbContext.FileResources.Remove(fileResource);
                        AssemblySteps.DbContext.SaveChanges();
                    }
                }
            }
        }
    }
}
