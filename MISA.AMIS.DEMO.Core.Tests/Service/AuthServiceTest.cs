using AutoMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core.UnitTests.Service
{
    [TestFixture]
    public class AuthServiceTest
    {
        #region Properties
        public IUserRepository UserRepository { get; set; }
        public IJWTProvider JwtProvider { get; set; }
        public IMapper Mapper { get; set; }
        public ITokenService TokenService { get; set; }
        public IAuthService AuthServiceSub { get; set; } 
        #endregion

        #region SetUp
        [SetUp]
        public void SetUp()
        {
            UserRepository = Substitute.For<IUserRepository>();
            JwtProvider = Substitute.For<IJWTProvider>();
            Mapper = Substitute.For<IMapper>();
            TokenService = Substitute.For<ITokenService>();
            AuthServiceSub = Substitute.For<AuthService>(UserRepository, JwtProvider, Mapper, TokenService);
        } 
        #endregion

        #region Methods
        #region GetAuthentication
        [Test]
        public async Task GetAuthentication_ValidUserInfo_Success()
        {
            // Arrange
            var userDto = new UserDto();
            var userInfo = new UserModel();
            AuthServiceSub.CheckValidUserInformation(userDto).Returns(userInfo);

            // Act
            var result = await AuthServiceSub.GetAuthentication(userDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<UserModel>());
            TokenService.Received(1).GetAccessToken(userInfo);
            await TokenService.Received(1).GetRefreshToken(Arg.Any<User>());
        }

        [Test]
        public async Task GetAuthentication_InValidUserInfo_Failure()
        {
            // Arrange
            var userDto = new UserDto();
            AuthServiceSub.CheckValidUserInformation(userDto).ReturnsNull();

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidUserLoginException>(async () => await AuthServiceSub.GetAuthentication(userDto));
            await AuthServiceSub.Received(1).CheckValidUserInformation(userDto);
        }
        #endregion

        #region ValidateTokenToLogout
        [Test]
        public async Task ValidateTokenToLogout_ValidToken_Success()
        {
            // Arrange
            string token = "token";
            UserRepository.GetUserModel(Arg.Any<string>()).Returns(new UserModel());

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await AuthServiceSub.ValidateTokenToLogout(token));
            await TokenService.Received(1).RevokeTokens(Arg.Any<User>());
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }

        [Test]
        public async Task ValidateTokenToLogout_InValidToken_Failure()
        {
            // Arrange
            string? token = null;

            // Act & Assert
            Assert.ThrowsAsync<AuthenticationTimeout>(async () => await AuthServiceSub.ValidateTokenToLogout(token));
            await TokenService.Received(0).RevokeTokens(Arg.Any<User>());
            await UserRepository.Received(0).GetUserModel(Arg.Any<string>());
        }

        [Test]
        public async Task ValidateTokenToLogout_InValidUserInfo_Failure()
        {
            // Arrange
            string token = "token";
            UserRepository.GetUserModel(Arg.Any<string>()).ReturnsNull();

            // Act & Assert
            Assert.ThrowsAsync<AuthenticationTimeout>(async () => await AuthServiceSub.ValidateTokenToLogout(token));
            await TokenService.Received(0).RevokeTokens(Arg.Any<User>());
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }
        #endregion

        #region RefreshAuthenticationForClient
        [Test]
        public async Task RefreshAuthenticationForClient_ValidToken_Success()
        {
            // Arrange
            string oldAccessToken = "access-token";
            string refreshToken = "refresh-token";
            UserRepository.GetUserModel(Arg.Any<string>()).Returns(new UserModel()
            {
                RefreshToken = "refresh-token",
                RefreshTokenExpiryTime = DateTime.MaxValue
            });

            // Act
            await AuthServiceSub.RefreshAuthenticationForClient(oldAccessToken, refreshToken);

            // Assert
            TokenService.Received(1).GetAccessToken(Arg.Any<UserModel>());
            await TokenService.Received(1).GetRefreshToken(Arg.Any<User>());
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }

        [Test]
        public async Task RefreshAuthenticationForClient_InValidToken_Success()
        {
            // Arrange
            string? oldAccessToken = null;
            string refreshToken = "refresh-token";

            // Act & Assert
            Assert.ThrowsAsync<AuthenticationTimeout>(async () => await AuthServiceSub.RefreshAuthenticationForClient(oldAccessToken, refreshToken));
            TokenService.Received(0).GetAccessToken(Arg.Any<UserModel>());
            await TokenService.Received(0).GetRefreshToken(Arg.Any<User>());
            await UserRepository.Received(0).GetUserModel(Arg.Any<string>());
        }

        [Test]
        public async Task RefreshAuthenticationForClient_ExpiredToken_Success()
        {
            // Arrange
            string oldAccessToken = "access-token";
            string refreshToken = "refresh-token";
            UserRepository.GetUserModel(Arg.Any<string>()).ReturnsNull();

            // Act & Assert
            Assert.ThrowsAsync<AuthenticationTimeout>(async () => await AuthServiceSub.RefreshAuthenticationForClient(oldAccessToken, refreshToken));
            TokenService.Received(0).GetAccessToken(Arg.Any<UserModel>());
            await TokenService.Received(0).GetRefreshToken(Arg.Any<User>());
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }
        #endregion

        #region GetCurrentUser
        [Test]
        public async Task GetCurrentUser_ValidToken_Success()
        {
            // Arrange
            string accessToken = "access-token";
            UserRepository.GetUserModel(Arg.Any<string>()).Returns(new UserModel());

            // Act
            var result = await AuthServiceSub.GetCurrentUser(accessToken);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<UserModel>());
        }

        [Test]
        public async Task GetCurrentUser_InValidToken_Failure()
        {
            // Arrange
            string? accessToken = null;

            // Act & Assert
            Assert.ThrowsAsync<AuthenticationTimeout>(async () => await AuthServiceSub.GetCurrentUser(accessToken));
            await UserRepository.Received(0).GetUserModel(Arg.Any<string>());
        }

        [Test]
        public async Task GetCurrentUser_InValidUser_Failure()
        {
            // Arrange
            string? accessToken = "access-token";
            UserRepository.GetUserModel(Arg.Any<string>()).ReturnsNull();

            // Act & Assert
            Assert.ThrowsAsync<AuthenticationTimeout>(async () => await AuthServiceSub.GetCurrentUser(accessToken));
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }
        #endregion

        #region CheckValidUserInformation
        [Test]
        public async Task CheckValidUserInformation_ValidUserInfo_Success()
        {
            // Arrange
            var userDto = new UserDto() { UserName = "huy", Password = "1" };
            UserRepository.GetUserModel(userDto.UserName).Returns(new UserModel() { UserName = "huy", Password = "1" });

            // Act
            var result = await AuthServiceSub.CheckValidUserInformation(userDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<UserModel>());
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }

        [Test]
        public async Task CheckValidUserInformation_InValidUsername_Failure()
        {
            // Arrange
            var userDto = new UserDto() { UserName = "huy" };
            UserRepository.GetUserModel(Arg.Any<string>()).ReturnsNull();

            // Act
            var result = await AuthServiceSub.CheckValidUserInformation(userDto);

            // Assert
            Assert.That(result, Is.Null);
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }

        [Test]
        public async Task CheckValidUserInformation_InValidPassword_Failure()
        {
            // Arrange
            var userDto = new UserDto() { UserName = "huy", Password = "1" };
            UserRepository.GetUserModel(Arg.Any<string>()).Returns(new UserModel() { UserName = "huy", Password = "2" });

            // Act
            var result = await AuthServiceSub.CheckValidUserInformation(userDto);

            // Assert
            Assert.That(result, Is.Null);
            await UserRepository.Received(1).GetUserModel(Arg.Any<string>());
        }
        #endregion 
        #endregion
    }
}
