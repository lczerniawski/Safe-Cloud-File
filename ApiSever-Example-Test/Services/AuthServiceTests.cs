using ApiServer_Example.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Text;
using System.Threading.Tasks;
using ApiServer_Example.Domains.Models;
using Castle.Core.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ApiSever_Example_Test.Services
{
    [TestClass]
    public class AuthServiceTests
    {
        private MockRepository _mockRepository;

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IUserRepository> _mockUserRepository;
        private User _user;

        [TestInitialize]
        public void TestInitialize()
        {
            this._mockRepository = new MockRepository(MockBehavior.Strict);

            this._mockConfiguration = this._mockRepository.Create<IConfiguration>();
            this._mockUserRepository = this._mockRepository.Create<IUserRepository>();

            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("bRhYJRlZvBj2vW4MrV5HVdPgIE6VMtCFB0kTtJ1m");

            var configurationSectionInt = new Mock<IConfigurationSection>();
            configurationSectionInt.Setup(a => a.Value).Returns("600");

            _mockConfiguration.Setup(a => a.GetSection("JWTSecretKey")).Returns(configurationSection.Object);
            _mockConfiguration.Setup(a => a.GetSection("JWTLifespan")).Returns(configurationSectionInt.Object);


            var userGuid = Guid.NewGuid();
            _user = new User
            {
                Id = userGuid,
                Email = "test@test.pl",
                PasswordHash = "AQAAAAEAACcQAAAAECGIYx+DgbVREs4dVy7Oy0sL6xp4KVl9WrGjcmfxteov7nXpspqWNWCa3e+FUJ/sLQ==",
                RsaKeys = null
            };
            _mockUserRepository.Setup(f => f.GetUserByIdAsync(userGuid)).ReturnsAsync(_user);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._mockRepository.Verify();
        }

        private AuthService CreateService()
        {
            return new AuthService(this._mockConfiguration.Object,this._mockUserRepository.Object);
        }

        [TestMethod]
        public async Task GetAuthData_WithCorrectGuid_Success()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.GetAuthData(_user.Id);

            // Assert
            Assert.AreEqual(result.Email,_user.Email);
        }

        [TestMethod]
        public void HashPassword_string_Success()
        {
            // Arrange
            var service = this.CreateService();
            string password = "string";

            // Act
            var result = service.HashPassword(password);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void VerifyPassword_string_Success()
        {
            // Arrange
            var service = this.CreateService();
            string actualPassword = "string";
            string hashedPassword = "AQAAAAEAACcQAAAAECGIYx+DgbVREs4dVy7Oy0sL6xp4KVl9WrGjcmfxteov7nXpspqWNWCa3e+FUJ/sLQ==";

            // Act
            var result = service.VerifyPassword(actualPassword,hashedPassword);

            // Assert
            Assert.AreEqual(true,result);
        }
    }
}
