using Hospital.Commands.ManageWards;
using Hospital.Database.Interfaces;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;
using NHibernate;

namespace Hospital.Test.ManageWardsTests
{
    public class DeleteWardCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListsStorage;
        private Mock<IDatabaseOperations> mockDatabaseOperations;

        private DeleteWardCommand deleteWardCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListsStorage = new Mock<IListsStorage>();
            mockDatabaseOperations = new Mock<IDatabaseOperations>();

            deleteWardCommand = new DeleteWardCommand(
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenWardsListEmpty_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([]);

            deleteWardCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DeleteWardMessages.NoWardPrompt), Times.Once());
            mockMenuHandler.Verify(x => x.SelectObject(It.IsAny<List<Ward>>(), UiMessages.DeleteWardMessages.SelectWardPrompt), Times.Never());
        }

        [Fact]
        public void Execute_WhenAssignedPatientOrEmployeeToWard_ShouldReturnEarly()
        {
            SetUpMocks();

            var assignedPatientsList = new List<Patient>() { new Mock<Patient>().Object };

            var mockWard = new Mock<Ward>();
            mockWard.SetupAllProperties();
            mockWard.Object.IsDeleted = false;
            mockWard.Setup(x => x.AssignedPatients)
                    .Returns(assignedPatientsList);

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([mockWard.Object]);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Ward>>(), It.IsAny<string>()))
                           .Returns(mockWard.Object);

            deleteWardCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DeleteWardMessages.WardNonEmptyPrompt), Times.Once());
            mockListManage.Verify(x => x.SoftDelete(mockWard.Object, It.IsAny<List<Ward>>()), Times.Never());
            Assert.Contains(mockWard.Object, mockListsStorage.Object.Wards);
            Assert.False(mockWard.Object.IsDeleted);
        }

        [Fact]
        public void Execute_WhenNotAssignedPatientOrEmployeeToWard_ShouldRemoveWard()
        {
            SetUpMocks();

            var mockWard = new Mock<Ward>();
            mockWard.SetupAllProperties();
            mockWard.Object.IsDeleted = false;
            mockWard.Setup(x => x.AssignedPatients)
                    .Returns([]);
            mockWard.Setup(x => x.AssignedEmployees)
                    .Returns([]);

            var wardsList = new List<Ward>() { mockWard.Object };

            mockListsStorage.Setup(x => x.Wards)
                            .Returns(wardsList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Ward>>(), It.IsAny<string>()))
                            .Returns(mockWard.Object);

            mockDatabaseOperations.Setup(x => x.Update(It.IsAny<Ward>(), It.IsAny<ISession>()))
                                  .Returns(true);

            mockListManage.Setup(x => x.SoftDelete(It.IsAny<Ward>(), It.IsAny<List<Ward>>()))
                          .Callback((Ward ward, List<Ward> list) =>
                          {
                              ward.IsDeleted = true;
                              list.Remove(ward);
                          });

            deleteWardCommand.Execute();

            Assert.DoesNotContain(mockWard.Object, wardsList);
            Assert.True(mockWard.Object.IsDeleted);
        }
    }
}