using ApiServer_Example.Data;
using ApiServer_Example.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSever_Example_Test.Services
{
    [TestClass]
    public class UserRepositoryTests
    {
        private ApplicationDbContext _inMemoryApplicationDbContext;
        private Guid _userGuid;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _inMemoryApplicationDbContext = new ApplicationDbContext(options);
            _userGuid = Guid.NewGuid();

            var user = new User
            {
                Id = _userGuid,
                Email = "test@test.pl",
                PasswordHash = "AQAAAAEAACcQAAAAECGIYx+DgbVREs4dVy7Oy0sL6xp4KVl9WrGjcmfxteov7nXpspqWNWCa3e+FUJ/sLQ==",
                RsaKeys = null
            };

            _inMemoryApplicationDbContext.Users.Add(user);
            _inMemoryApplicationDbContext.SaveChanges();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _inMemoryApplicationDbContext.Dispose();
        }

        private UserRepository CreateUserRepository()
        {
            return new UserRepository(_inMemoryApplicationDbContext);
        }

        [TestMethod]
        public async Task IsEmailUniqAsync_NotExistingEmail_Success()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string email = "nonemail@email.pl";

            // Act
            var result = await userRepository.IsEmailUniqAsync(email);

            // Assert
            Assert.AreEqual(true,result);
        }

        [TestMethod]
        public async Task IsEmailUniqAsync_ExistingEmail_Success()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string email = "test@test.pl";

            // Act
            var result = await userRepository.IsEmailUniqAsync(email);

            // Assert
            Assert.AreEqual(false,result);
        }

        [TestMethod]
        public async Task GetUserByEmailAsync_ExistingEmail_Success()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string email = "test@test.pl";

            // Act
            var result = await userRepository.GetUserByEmailAsync(email);

            // Assert
            Assert.AreEqual(_userGuid,result.Id);
        }

        [TestMethod]
        public async Task CreateUserAsync_CorrectValues_Success()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            User user = new User
            {
                Email = "testing@test.pl",
                PasswordHash = "dadda",
                RsaKeys = null
            };

            // Act
            var result = await userRepository.CreateUserAsync(user);

            // Assert
            Assert.IsNotNull(result.Id);
        }

        [TestMethod]
        public async Task GetAllUsers_CorrectValues_Success()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();

            // Act
            var result = await userRepository.GetAllUsers();

            // Assert
            Assert.AreEqual(1,result.Count());
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ExistingID_Success()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();

            // Act
            var result = await userRepository.GetUserByIdAsync(_userGuid);

            // Assert
            Assert.AreEqual(_userGuid,result.Id);
        }
    }
}
