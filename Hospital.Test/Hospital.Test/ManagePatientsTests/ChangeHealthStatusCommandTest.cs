using Hospital.Commands.ManagePatients.ManagePatient;
using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManagePatientsTests
{
    public class ChangeHealthStatusCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListsStorage;

        private ChangeHealthStatusCommand changeHealthStatusCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListsStorage = new Mock<IListsStorage>();

            changeHealthStatusCommand = new ChangeHealthStatusCommand(
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenNoPatients_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Patients)
                            .Returns([]);

            changeHealthStatusCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt), Times.Once());
            mockMenuHandler.Verify(x => x.SelectObject(mockListsStorage.Object.Patients, UiMessages.ChangeHealthStatusMessages.SelectPatientPrompt), Times.Never());
        }

        [Fact]
        public void Execute_WhenThereIsPatient_ShouldChangeHealthStatusOnPatient()
        {
            SetUpMocks();

            var mockPatient = new Mock<Patient>().SetupAllProperties();

            mockListsStorage.Setup(x => x.Patients)
                            .Returns([mockPatient.Object]);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Patient>>(), It.IsAny<string>()))
                           .Returns(mockPatient.Object);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Health>())
                           .Returns(Health.Good);

            changeHealthStatusCommand.Execute();

            mockListManage.Verify(x => x.Update(mockPatient.Object, mockListsStorage.Object.Patients), Times.Once());
            mockMenuHandler.Verify(x => x.ShowMessage(string.Format(UiMessages.ChangeHealthStatusMessages.OperationSuccessPrompt, mockPatient.Object.Name, mockPatient.Object.Surname)));
            Assert.True(mockPatient.Object.HealthStatus == Health.Good);
        }
    }
}