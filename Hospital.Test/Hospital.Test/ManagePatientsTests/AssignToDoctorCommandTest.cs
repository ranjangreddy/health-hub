using Hospital.Commands.ManagePatients.ManagePatient;
using Hospital.Entities.Employee;
using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManagePatientsTests
{
    public class AssignToDoctorCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListsStorage;

        private AssignToDoctorCommand assignToDoctorCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListsStorage = new Mock<IListsStorage>();

            assignToDoctorCommand = new AssignToDoctorCommand(
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenPatientsListEmpty_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Patients)
                            .Returns([]);

            assignToDoctorCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt), Times.Once());
            mockListsStorage.Verify(x => x.Employees, Times.Never());
            mockMenuHandler.Verify(x => x.SelectObject(It.IsAny<List<Patient>>(), UiMessages.AssignToDoctorMessages.SelectPatientPrompt), Times.Never());
        }

        [Fact]
        public void Execute_WhenDoctorsListEmpty_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Patients)
                            .Returns([It.IsAny<Patient>()]);
            mockListsStorage.Setup(x => x.Employees)
                            .Returns([]);

            assignToDoctorCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.AssignToDoctorMessages.NoDoctorsPrompt), Times.Once());
        }

        [Fact]
        public void Exectue_WhenDifferentWard_ShouldReturnEarly()
        {
            SetUpMocks();

            var mockWard = new Mock<Ward>();
            var mockWardSecond = new Mock<Ward>();
            var mockPatient = new Mock<Patient>();
            var patientsList = new List<Patient>() { mockPatient.Object };
            var mockDoctor = new Mock<Employee>();
            var employeesList = new List<Employee>() { mockDoctor.Object };

            mockPatient.Setup(x => x.AssignedWard)
                       .Returns(mockWard.Object);
            mockDoctor.Setup(x => x.AssignedWard)
                      .Returns(mockWardSecond.Object);
            mockDoctor.Setup(x => x.Position)
                      .Returns(Position.Doctor);

            mockListsStorage.Setup(x => x.Patients)
                            .Returns(patientsList);
            mockListsStorage.Setup(x => x.Employees)
                            .Returns(employeesList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Patient>>(), It.IsAny<string>()))
                           .Returns(mockPatient.Object);
            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Employee>>(), It.IsAny<string>()))
                           .Returns(mockDoctor.Object);

            assignToDoctorCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.AssignToDoctorMessages.WrongWardPrompt), Times.Once());
        }

        [Fact]
        public void Execute_WhenThereIsPatientAndDoctor_ShouldAssignPatientToDoctor()
        {
            SetUpMocks();

            var mockPatient = new Mock<Patient>().SetupAllProperties();
            var patientsList = new List<Patient>() { mockPatient.Object };
            var mockDoctor = new Mock<Employee>();
            var employeesList = new List<Employee>() { mockDoctor.Object };

            mockDoctor.Setup(x => x.Position)
                      .Returns(Position.Doctor);

            mockListsStorage.Setup(x => x.Patients)
                            .Returns(patientsList);
            mockListsStorage.Setup(x => x.Employees)
                            .Returns(employeesList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Patient>>(), It.IsAny<string>()))
                           .Returns(mockPatient.Object);
            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Employee>>(), It.IsAny<string>()))
                           .Returns(mockDoctor.Object);

            assignToDoctorCommand.Execute();

            mockListManage.Verify(x => x.Update(mockPatient.Object, mockListsStorage.Object.Patients), Times.Once());
            mockMenuHandler.Verify(x => x.ShowMessage(string.Format(UiMessages.AssignToDoctorMessages.OperationSuccessPrompt, UiMessages.DoctorObjectMessages.Position, mockDoctor.Object.Surname, mockPatient.Object.Name, mockPatient.Object.Surname)), Times.Once());
            Assert.True(mockPatient.Object.AssignedDoctor == mockDoctor.Object);
        }
    }
}