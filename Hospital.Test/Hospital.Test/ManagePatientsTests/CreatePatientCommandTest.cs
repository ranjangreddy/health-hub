using Hospital.Commands.ManagePatients;
using Hospital.Database.Interfaces;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;
using NHibernate;

namespace Hospital.Test.ManagePatientsTests
{
    public class CreatePatientCommandTest
    {
        private Mock<IObjectsFactory> mockObjectsFactory;
        private Mock<IValidateObjects> mockValidateObjects;
        private Mock<IDTOFactory> mockDtoFactory;
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListsStorage;
        private Mock<IManageCapacity> mockManageCapacity;
        private Mock<IDatabaseOperations> mockDatabaseOperations;

        private CreatePatientCommand createPatientCommand;

        private void SetUpMocks()
        {
            mockObjectsFactory = new Mock<IObjectsFactory>();
            mockValidateObjects = new Mock<IValidateObjects>();
            mockDtoFactory = new Mock<IDTOFactory>();
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListsStorage = new Mock<IListsStorage>();
            mockManageCapacity = new Mock<IManageCapacity>();
            mockDatabaseOperations = new Mock<IDatabaseOperations>();

            createPatientCommand = new CreatePatientCommand(
                mockObjectsFactory.Object,
                mockValidateObjects.Object,
                mockDtoFactory.Object,
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object,
                mockManageCapacity.Object);
        }

        [Fact]
        public void Execute_WhenNoWards_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([]);

            createPatientCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.CreatePatientMessages.NoWardErrorPrompt), Times.Once());
            mockDtoFactory.Verify(x => x.GatherPatientData(It.IsAny<List<Ward>>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenPatientInvalid_ShouldReturnEarly()
        {
            SetUpMocks();

            var ward = new Mock<Ward>();

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([ward.Object]);

            mockDtoFactory.Setup(x => x.GatherPatientData(It.IsAny<List<Ward>>()))
                          .Returns(new PatientDTO(new PersonDTO()));

            mockValidateObjects.Setup(x => x.ValidatePatientObject(It.IsAny<PatientDTO>()))
                               .Returns(false);

            createPatientCommand.Execute();

            mockObjectsFactory.Verify(x => x.CreatePatient(It.IsAny<PatientDTO>()), Times.Never());
            mockListManage.Verify(x => x.Add(It.IsAny<Patient>(), It.IsAny<List<Patient>>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenWardsListIsNotNullAndValidationPassed_ShouldCreatePatient()
        {
            SetUpMocks();

            var mockPatient = new Mock<Patient>();
            var patientsList = new List<Patient>();
            var mockWard = new Mock<Ward>();
            var wardsList = new List<Ward>() { mockWard.Object };

            mockListsStorage.Setup(x => x.Wards)
                            .Returns(wardsList);
            mockListsStorage.Setup(x => x.Patients)
                            .Returns(patientsList);

            mockDtoFactory.Setup(x => x.GatherPatientData(wardsList))
                           .Returns(new PatientDTO(new PersonDTO()));

            mockValidateObjects.Setup(x => x.ValidatePatientObject(It.IsAny<PatientDTO>()))
                               .Returns(true);

            mockObjectsFactory.Setup(x => x.CreatePatient(It.IsAny<PatientDTO>()))
                              .Returns(mockPatient.Object);

            mockDatabaseOperations.Setup(x => x.Add(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(true);

            mockListManage.Setup(x => x.Add(It.IsAny<Patient>(), It.IsAny<List<Patient>>()))
                          .Callback((Patient item, List<Patient> list) =>
                          {
                              if (mockDatabaseOperations.Object.Add(item, new Mock<ISession>().Object))
                              {
                                  list.Add(item);
                              }
                          });

            createPatientCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(string.Format(UiMessages.CreatePatientMessages.OperationSuccessPrompt, mockPatient.Object.Name, mockPatient.Object.Surname)), Times.Once());
            Assert.Contains(mockPatient.Object, patientsList);
        }
    }
}