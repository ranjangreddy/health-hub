using Hospital.Enums;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.UtilitiesTests
{
    public class DTOFactoryTest
    {
        private Mock<IInputHandler> mockInputHandler;
        private Mock<IMenuHandler> mockMenuHandler;

        private DTOFactory dtoFactory;

        private void SetUpMocks()
        {
            mockInputHandler = new Mock<IInputHandler>();
            mockMenuHandler = new Mock<IMenuHandler>();

            dtoFactory = new DTOFactory(
                mockMenuHandler.Object,
                mockInputHandler.Object);
        }

        [Fact]
        public void GatherPersonData_WhenCorrectData_ShouldReturnPersonDTO()
        {
            SetUpMocks();

            var expectedName = "John";
            var expectedSurname = "Doe";
            var expectedGender = Gender.Male;
            var expectedBirthday = new DateTime(1990, 1, 1);

            mockInputHandler.Setup(x => x.GetInput(UiMessages.FactoryMessages.ProvideNamePrompt))
                            .Returns(expectedName);
            mockInputHandler.Setup(x => x.GetInput(UiMessages.FactoryMessages.ProvideSurnamePrompt))
                            .Returns(expectedSurname);
            mockInputHandler.Setup(x => x.GetDateTimeInput(It.IsAny<string>()))
                            .Returns(expectedBirthday);

            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Gender>())
                           .Returns(expectedGender);

            var personDTO = dtoFactory.GatherPersonData();

            Assert.Equal(expectedName, personDTO.Name);
            Assert.Equal(expectedSurname, personDTO.Surname);
            Assert.Equal(expectedGender, personDTO.Gender);
            Assert.Equal(expectedBirthday, personDTO.Birthday);
        }

        [Fact]
        public void GatherEmployeeData_WhenCorrectData_ShouldReturnDoctorDTO()
        {
            SetUpMocks();

            var mockWard = new Mock<Ward>();
            var wardsList = new List<Ward>() { mockWard.Object };
            var expectedPosition = Position.Doctor;

            mockMenuHandler.Setup(x => x.ShowInteractiveMenu(wardsList))
                           .Returns(mockWard.Object);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Position>())
                           .Returns(expectedPosition);

            var employeeDTO = dtoFactory.GatherEmployeeData(wardsList);

            Assert.Equal(mockWard.Object, employeeDTO.AssignedWard);
            Assert.Equal(expectedPosition, employeeDTO.Position);
            Assert.NotNull(employeeDTO.AssignedPatients);
        }

        [Fact]
        public void GatherPatientData_WhenCorrectData_ShouldReturnPatientDTO()
        {
            SetUpMocks();

            var expectedPersonalIdNumber = "12345678901";
            var mockWard = new Mock<Ward>();
            var wardsList = new List<Ward>() { mockWard.Object };

            mockInputHandler.Setup(x => x.GetInput(It.IsAny<string>()))
                            .Returns(expectedPersonalIdNumber);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu(wardsList))
                           .Returns(mockWard.Object);

            var patientDTO = dtoFactory.GatherPatientData(wardsList);

            Assert.Equal(expectedPersonalIdNumber, patientDTO.PersonalIdNumber);
            Assert.Equal(mockWard.Object, patientDTO.AssignedWard);
        }

        [Fact]
        public void GatherUserData_WhenCorrectData_ShouldReturnUserDTO()
        {
            SetUpMocks();

            var expectedLogin = "login";
            var expectedPassword = "passwordpassword";
            var expectedRank = Rank.Admin;

            mockInputHandler.Setup(x => x.GetInput(UiMessages.FactoryMessages.ProvideLoginPrompt))
                            .Returns(expectedLogin);
            mockInputHandler.Setup(x => x.GetInput(UiMessages.FactoryMessages.ProvidePasswordPrompt))
                            .Returns(expectedPassword);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Rank>())
                           .Returns(expectedRank);

            var userDTO = dtoFactory.GatherUserData();

            Assert.Equal(expectedLogin, userDTO.Login);
            Assert.Equal(expectedPassword, userDTO.Password);
            Assert.Equal(expectedRank, userDTO.Rank);
        }

        [Fact]
        public void GatherWardData_WhenCorrectData_ShouldReturnWardDTO()
        {
            SetUpMocks();

            var excpectedName = "testward";
            var excpectedCapacity = 10;

            mockInputHandler.Setup(x => x.GetInput(UiMessages.FactoryMessages.ProvideNamePrompt))
                            .Returns(excpectedName);
            mockInputHandler.Setup(x => x.GetIntInput(UiMessages.FactoryMessages.ProvideCapacityPrompt))
                            .Returns(excpectedCapacity);

            var wardDTO = dtoFactory.GatherWardData();

            Assert.Equal(excpectedName, wardDTO.Name);
            Assert.Equal(excpectedCapacity, wardDTO.Capacity);
            Assert.NotNull(wardDTO.AssignedPatients);
            Assert.NotNull(wardDTO.AssignedEmployees);
        }
    }
}