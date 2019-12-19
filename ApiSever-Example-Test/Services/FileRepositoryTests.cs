using ApiServer_Example.Data;
using ApiServer_Example.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using ApiServer_Example.Domains.Models;

namespace ApiSever_Example_Test.Services
{
    [TestClass]
    public class FileRepositoryTests
    {
        private MockRepository mockRepository;
        private Mock<ApplicationDbContext> mockApplicationDbContext;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockApplicationDbContext = this.mockRepository.Create<ApplicationDbContext>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private FileRepository CreateFileRepository()
        {
            return new FileRepository(
                this.mockApplicationDbContext.Object);
        }

        [TestMethod]
        public async Task CreateFileAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            FileModel fileModel = null;

            // Act
            var result = await fileRepository.CreateFileAsync(fileModel);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public async Task DeleteFileAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            Guid id = default(global::System.Guid);

            // Act
            var result = await fileRepository.DeleteFileAsync(id);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public async Task UpdateFile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            FileModel fileModel = null;

            // Act
            var result = await fileRepository.UpdateFile(fileModel);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetAllUserFiles_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            Guid userId = default(global::System.Guid);

            // Act
            var result = await fileRepository.GetAllUserFiles(
                userId);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetFileByIdAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            Guid id = default(global::System.Guid);

            // Act
            var result = await fileRepository.GetFileByIdAsync(id);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetFileByNameAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            string fileName = null;

            // Act
            var result = await fileRepository.GetFileByNameAsync(fileName);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public async Task CheckIfFileExist_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fileRepository = this.CreateFileRepository();
            string fileName = null;

            // Act
            var result = await fileRepository.CheckIfFileExist(fileName);

            // Assert
            Assert.Fail();
        }
    }
}
