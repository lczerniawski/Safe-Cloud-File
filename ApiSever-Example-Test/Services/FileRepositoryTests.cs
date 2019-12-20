using ApiServer_Example.Data;
using ApiServer_Example.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.Models;
using Microsoft.EntityFrameworkCore;
using ApplicationDbContext = ApiServer_Example.Data.ApplicationDbContext;

namespace ApiSever_Example_Test.Services
{
    [TestClass]
    public class FileRepositoryTests
    {
        private ApplicationDbContext _inMemoryApplicationDbContext;
        private Guid _fileGuid;
        private Guid _userGuid;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _inMemoryApplicationDbContext = new ApplicationDbContext(options);

            var file = new FileModel
            {
                FileName = "Test",
                FileType = ".jpg",
                Id = Guid.NewGuid(),
                IsShared = false,
                UserId = Guid.NewGuid()
            };

            _inMemoryApplicationDbContext.Files.Add(file);
            _inMemoryApplicationDbContext.SaveChanges();
            _fileGuid = file.Id;
            _userGuid = file.UserId;

            var trackerEntries = _inMemoryApplicationDbContext.ChangeTracker.Entries().ToList();
            foreach (var trackerEntry in trackerEntries)
            {
                trackerEntry.State = EntityState.Detached;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _inMemoryApplicationDbContext.Dispose();
        }

        private FileRepository CreateFileRepository()
        {
            return new FileRepository(_inMemoryApplicationDbContext);
        }

        [TestMethod]
        public async Task CreateFileAsync_CorrectValues_Success()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            FileModel fileModel = new FileModel
            {
                IsShared = false,
                FileName = "TestFile",
                FileType = ".json",
                UserId = Guid.NewGuid()
            };

            // Act
            var result = await fileRepository.CreateFileAsync(fileModel);

            // Assert
            Assert.IsNotNull(result.Id);
        }

        [TestMethod]
        public async Task UpdateFile_CorrectValues_Success()
        {
            // Arrange
            var fileRepository = CreateFileRepository();
            FileModel fileModel = new FileModel
            {
                FileName = "ChangedFileName",
                FileType = ".jpg",
                Id = _fileGuid,
                IsShared = false,
                UserId = Guid.NewGuid()
            };

            // Act
            var result = await fileRepository.UpdateFile(fileModel);

            // Assert
            Assert.AreEqual("ChangedFileName",result.FileName);
        }

        [TestMethod]
        public async Task GetAllUserFiles_ExistingUser_Success()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();

            // Act
            var result = await fileRepository.GetAllUserFiles(_userGuid);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task GetFileByIdAsync_ExistingFile_Success()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();

            // Act
            var result = await fileRepository.GetFileByIdAsync(_fileGuid);

            // Assert
            Assert.AreEqual(_fileGuid, result.Id);
        }

        [TestMethod]
        public async Task GetFileByNameAsync_CorrectValues_Success()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            var fullFileName = "Test.jpg";

            // Act
            var result = await fileRepository.GetFileByNameAsync(fullFileName);

            // Assert
            Assert.AreEqual(_fileGuid, result.Id);
        }

        [TestMethod]
        public async Task CheckIfFileExist_CorrectValues_Success()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            var fullFileName = "Test.jpg";

            // Act
            var result = await fileRepository.CheckIfFileExist(fullFileName);

            // Assert
            Assert.AreEqual(true,result);
        }

        [TestMethod]
        public async Task DeleteFileAsync_CorrectValues_Success()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();

            // Act
            var result = await fileRepository.DeleteFileAsync(_fileGuid);

            // Assert
            Assert.AreEqual(true,result);
        }
    }
}
