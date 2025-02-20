using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities;
using Hospital.Utilities.ListManagement.Interfaces;
using Moq;

namespace Hospital.Test.UtilitiesTests
{
    public class AuthenticationServiceTest
    {
        private Mock<IListsStorage> mockListsStorage;

        private AuthenticationService authenticationService;

        private void SetupMocks()
        {
            mockListsStorage = new Mock<IListsStorage>();

            authenticationService = new AuthenticationService(mockListsStorage.Object);
        }

        [Fact]
        public void GetUserByLogin_WhenThereIsUserWithProvidedLogin_ShouldReturnUser()
        {
            SetupMocks();

            var login = "test";
            var mockUser = new Mock<User>();

            mockUser.Setup(x => x.Login)
                    .Returns(login);

            mockListsStorage.Setup(x => x.Users)
                            .Returns([mockUser.Object]);

            var userObject = authenticationService.GetUserByLogin(login);

            Assert.NotNull(userObject);
            Assert.Equal(login, userObject.Login);
        }

        [Fact]
        public void GetUserByLogin_WhenThereIsNoUserWithProvidedLogin_ShouldReturnNull()
        {
            SetupMocks();

            var login = "testuser";

            mockListsStorage.Setup(x => x.Users)
                            .Returns([new Mock<User>().Object]);

            var result = authenticationService.GetUserByLogin(login);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData(" ", " ")]
        public void Authenticate_WhenSuccessfullyAuthenticateUser_ShouldReturnsTrue(string userPassword, string inputPassword)
        {
            SetupMocks();

            var result = authenticationService.Authenticate(userPassword, inputPassword);

            Assert.True(result);
        }

        [Theory]
        [InlineData("test", "testpassword")]
        [InlineData(" ", "")]
        public void Authenticate_WhenUnsuccessfullyAuthenticateUser_ShouldReturnsFalse(string userPassword, string inputPassword)
        {
            SetupMocks();

            var result = authenticationService.Authenticate(userPassword, inputPassword);

            Assert.False(result);
        }
    }
}