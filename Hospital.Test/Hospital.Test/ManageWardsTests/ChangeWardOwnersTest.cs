using Hospital.Commands.ManageWards;
using Hospital.Enums;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManageWardsTests
{
    public class ChangeWardOwnersTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListsStorage;

        private ChangeWardOwners changeWardOwners;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListsStorage = new Mock<IListsStorage>();

            changeWardOwners = new ChangeWardOwners(
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenOperationIsAddAndVerificationFailed_ShouldReturnEarly()
        {
            //Arrange
            SetUpMocks();

            var usersList = new List<User>();
            var wardsList = new List<Ward>();
            var mockUser = Mock.Of<User>();
            mockUser.AssignedWards = wardsList;

            mockListsStorage.Setup(x => x.Users)
                            .Returns(usersList);
            mockListsStorage.Setup(x => x.Wards)
                            .Returns(wardsList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<User>>(), It.IsAny<string>()))
                           .Returns(mockUser);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Operation>())
                           .Returns(Operation.Add);

            //Act
            changeWardOwners.Execute();

            //Assert
            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.ChangeWardOwners.NoWardToAssign), Times.Once());
        }

        [Fact]
        public void Execute_WhenOperationIsRemoveAndVerificationFailed_ShouldReturnEarly()
        {
            //Arrange
            SetUpMocks();

            var usersList = new List<User>();
            var wardsList = new List<Ward>();
            var mockUser = Mock.Of<User>();
            mockUser.AssignedWards = wardsList;

            mockListsStorage.Setup(x => x.Users)
                            .Returns(usersList);
            mockListsStorage.Setup(x => x.Wards)
                            .Returns(wardsList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<User>>(), It.IsAny<string>()))
                           .Returns(mockUser);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Operation>())
                           .Returns(Operation.Remove);

            //Act
            changeWardOwners.Execute();

            //Assert
            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.ChangeWardOwners.NoWardPrompt), Times.Once());
        }

        [Fact]
        public void Execute_WhenOperationIsAddAndVerificationPassed_ShouldAddUserToWard()
        {
            //Arrange
            SetUpMocks();

            var usersList = new List<User>();
            var mockUser = Mock.Of<User>();
            mockUser.AssignedWards = new List<Ward>();

            var mockWard = Mock.Of<Ward>();
            mockWard.AssignedUsers = new List<User>();
            var wardsList = new List<Ward>() { mockWard };

            mockListsStorage.Setup(x => x.Users)
                            .Returns(usersList);
            mockListsStorage.Setup(x => x.Wards)
                            .Returns(wardsList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<User>>(), It.IsAny<string>()))
                           .Returns(mockUser);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Operation>())
                           .Returns(Operation.Add);
            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Ward>>(), It.IsAny<string>()))
                           .Returns(mockWard);

            //Act
            changeWardOwners.Execute();

            //Assert
            Assert.Contains(mockWard, mockUser.AssignedWards);
            Assert.Contains(mockUser, mockWard.AssignedUsers);
        }

        [Fact]
        public void Execute_WhenOperationIsRemoveAndVerificationPassed_ShouldRemoveUserFromWard()
        {
            //Arrange
            SetUpMocks();

            var mockWard = Mock.Of<Ward>();
            mockWard.AssignedUsers = new List<User>();
            var wardsList = new List<Ward>() { mockWard };

            var usersList = new List<User>();
            var mockUser = Mock.Of<User>();
            mockUser.AssignedWards = new List<Ward>() { mockWard };


            mockListsStorage.Setup(x => x.Users)
                            .Returns(usersList);
            mockListsStorage.Setup(x => x.Wards)
                            .Returns(wardsList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<User>>(), It.IsAny<string>()))
                           .Returns(mockUser);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Operation>())
                           .Returns(Operation.Remove);
            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Ward>>(), It.IsAny<string>()))
                           .Returns(mockWard);

            //Act
            changeWardOwners.Execute();

            //Assert
            Assert.DoesNotContain(mockWard, mockUser.AssignedWards);
            Assert.DoesNotContain(mockUser, mockWard.AssignedUsers);
        }
    }
}