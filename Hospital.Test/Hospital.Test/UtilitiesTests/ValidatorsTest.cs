using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities;
using Hospital.Utilities.ListManagement.Interfaces;
using Moq;

namespace Hospital.Test.UtilitiesTests
{
    public class ValidatorsTests
    {
        private Mock<IListsStorage> mockListsStorage;

        private Validators validators;

        private void SetupMocks()
        {
            mockListsStorage = new Mock<IListsStorage>();

            validators = new Validators(mockListsStorage.Object);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("Valid String")]
        public void ValidateString_WhenValidInput_ShouldReturnTrue(string input)
        {
            SetupMocks();

            var result = validators.ValidateString(input);

            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidateString_WhenInvalidInput_ShouldReturnFalse(string input)
        {
            SetupMocks();

            var result = validators.ValidateString(input);

            Assert.False(result);
        }

        [Fact]
        public void ValidatePersonalIdNumber_WhenValidInput_ShouldReturnTrue()
        {
            SetupMocks();

            var validPersonalIdNumber = "12345678901";

            mockListsStorage.Setup(x => x.PersonalIdNumbers)
                            .Returns(["12345678900"]);

            var result = validators.ValidatePersonalIdNumber(validPersonalIdNumber);

            Assert.True(result);
        }

        [Theory]
        [InlineData("123456789")]
        [InlineData("12345678900")]
        [InlineData("")]
        public void ValidatePersonalIdNumber_WhenInvalidInput_ShouldReturnFalse(string input)
        {
            SetupMocks();

            mockListsStorage.Setup(x => x.PersonalIdNumbers)
                            .Returns(["12345678900"]);

            var result = validators.ValidatePersonalIdNumber(input);

            Assert.False(result);
        }

        [Fact]
        public void ValidateBirthday_WhenValidInput_ShouldReturnTrue()
        {
            SetupMocks();

            var validBirthday = DateTime.Today.AddYears(-30);

            var result = validators.ValidateBirthday(validBirthday);

            Assert.True(result);
        }

        [Fact]
        public void ValidateBirthday_WhenTooOldDate_ShouldReturnFalse()
        {
            SetupMocks();

            var invalidBirthday = DateTime.Today.AddYears(-160);

            var result = validators.ValidateBirthday(invalidBirthday);

            Assert.False(result);
        }

        [Fact]
        public void ValidateBirthday_WhenTooYoungDate_ShouldReturnFalse()
        {
            SetupMocks();

            var invalidBirthday = DateTime.Today;

            var result = validators.ValidateBirthday(invalidBirthday);

            Assert.False(result);
        }

        [Theory]
        [InlineData(Gender.Male)]
        [InlineData(Gender.Female)]
        public void ValidateGender_WhenValidInput_ShouldReturnTrue(Gender gender)
        {
            SetupMocks();

            var result = validators.ValidateGender(gender);

            Assert.True(result);
        }

        [Fact]
        public void ValidateGender_WhenInvalidInput_ShouldReturnFalse()
        {
            SetupMocks();

            var invalidGender = (Gender)999;

            var result = validators.ValidateGender(invalidGender);

            Assert.False(result);
        }

        [Fact]
        public void ValidateCapacity_WhenValidInput_ShouldReturnTrue()
        {
            SetupMocks();

            var validInput = 10;

            var result = validators.ValidateCapacity(validInput);

            Assert.True(result);
        }

        [Fact]
        public void ValidateCapacity_WhenInvalidInput_ShouldReturnFalse()
        {
            SetupMocks();

            var invalidInput = -2;

            var result = validators.ValidateCapacity(invalidInput);

            Assert.False(result);
        }

        [Fact]
        public void ValidateLogin_WhenValidInput_ShouldReturnTrue()
        {
            SetupMocks();

            var validLogin = "valid";

            mockListsStorage.Setup(x => x.Logins)
                            .Returns(["test"]);

            var result = validators.ValidateLogin(validLogin);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("test")]
        public void ValidateLogin_WhenInvalidInput_ShouldReturnFalse(string input)
        {
            SetupMocks();

            mockListsStorage.Setup(x => x.Logins)
                            .Returns(["test"]);

            var result = validators.ValidateLogin(input);

            Assert.False(result);
        }

        [Fact]
        public void ValidatePassword_WhenValidInput_ShouldReturnTrue()
        {
            SetupMocks();

            var validPassword = "testpassword";

            var result = validators.ValidatePassword(validPassword);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("test")]
        public void ValidatePassword_WhenInvalidInput_ShouldReturnFalse(string input)
        {
            SetupMocks();

            var result = validators.ValidatePassword(input);

            Assert.False(result);
        }

        [Fact]
        public void ValidateWardName_WhenValidInput_ShouldReturnTrue()
        {
            SetupMocks();

            var validWardName = "test";

            mockListsStorage.Setup(x => x.WardsNames)
                            .Returns(["wardname"]);

            var result = validators.ValidateWardName(validWardName);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("test")]
        public void ValidateWardName_WhenInvalidInput_ShouldReturnFalse(string input)
        {
            SetupMocks();

            mockListsStorage.Setup(x => x.WardsNames)
                            .Returns(["test"]);

            var result = validators.ValidateWardName(input);

            Assert.False(result);
        }

        [Fact]
        public void ValidatePossibiltyAssignToWard_WhenThereIsPossiblity_ShouldReturnsTrue()
        {
            SetupMocks();

            var mockWard = new Mock<Ward>();

            mockWard.Setup(x => x.Capacity)
                    .Returns(10);
            mockWard.Setup(x => x.AssignedPatients)
                    .Returns([]);

            var result = validators.ValidatePossibilityAssignToWard(mockWard.Object);

            Assert.True(result);
        }

        [Fact]
        public void ValidatePossibiltyAssignToWard_WhenThereIsNoPossibility_ShouldReturnsFalse()
        {
            SetupMocks();

            var mockPatient = new Mock<Patient>();
            var mockWard = new Mock<Ward>();

            mockWard.Setup(x => x.Capacity)
                    .Returns(1);
            mockWard.Setup(x => x.AssignedPatients)
                    .Returns([mockPatient.Object]);

            var result = validators.ValidatePossibilityAssignToWard(mockWard.Object);

            Assert.False(result);
        }

        [Theory]
        [InlineData(Position.Cleaner)]
        [InlineData(Position.Doctor)]
        [InlineData(Position.Nurse)]
        public void ValidatePosition_WhenValidInput_ShouldReturnTrue(Position position)
        {
            SetupMocks();

            var result = validators.ValidatePosition(position);

            Assert.True(result);
        }

        [Fact]
        public void ValidatePosition_WhenInvalidInput_ShouldReturnFalse()
        {
            SetupMocks();

            var invalidPosition = (Position)999;

            var result = validators.ValidatePosition(invalidPosition);

            Assert.False(result);
        }
    }
}