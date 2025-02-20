using Hospital.Commands.ManagePatients;
using Hospital.PeopleCategories.PatientClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManagePatientsTests
{
    public class DisplayPatientsCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListsStorage> mockListsStorage;

        private DisplayPatientsCommand displayPatientsCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListsStorage = new Mock<IListsStorage>();

            displayPatientsCommand = new DisplayPatientsCommand(
                mockMenuHandler.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Exectue_WhenPatientsListEmpty_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Patients)
                            .Returns([]);

            displayPatientsCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt), Times.Once());
            mockMenuHandler.Verify(x => x.DisplayList(It.IsAny<List<Patient>>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenPatientsListNotEmpty_ShouldDisplayPatientsList()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Patients)
                            .Returns([It.IsAny<Patient>()]);

            displayPatientsCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt), Times.Never());
            mockMenuHandler.Verify(x => x.DisplayList(It.IsAny<List<Patient>>()), Times.Once());
        }
    }
}