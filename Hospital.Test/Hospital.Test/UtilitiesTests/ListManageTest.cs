using Hospital.Database;
using Hospital.Database.Interfaces;
using Hospital.PeopleCategories.PatientClass;
using Hospital.Utilities.ListManagement;
using Moq;
using NHibernate;

namespace Hospital.Test.UtilitiesTests
{
    public class ListManageTest
    {
        private Mock<IDatabaseOperations> mockDatabaseOperations;
        private Mock<IDatabaseService> mockDatabaseService;
        private CreateSession createSession;

        private ListManage listManage;

        private void SetUpMocks()
        {
            mockDatabaseOperations = new Mock<IDatabaseOperations>();
            mockDatabaseService = new Mock<IDatabaseService>();
            createSession = new CreateSession(mockDatabaseService.Object);

            listManage = new ListManage(
                mockDatabaseOperations.Object,
                createSession);
        }

        [Fact]
        public void Add_WhenObjectIsNull_ShouldThrowArgumentNullExceptionndNotAddToList()
        {
            //Arrange
            SetUpMocks();

            var list = new List<Patient>();
            Patient patient = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => listManage.Add(patient, list));
            Assert.DoesNotContain(patient, list);
        }

        [Fact]
        public void Add_WhenDatabaseOperationFails_ShouldThrowExceptionAndNotAddToList()
        {
            //Arrange
            SetUpMocks();

            var list = new List<Patient>();
            var patient = Mock.Of<Patient>();

            mockDatabaseOperations.Setup(x => x.Add(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(false);

            //Assert
            Assert.Throws<Exception>(() => listManage.Add(patient, list));
            Assert.DoesNotContain(patient, list);
        }

        [Fact]
        public void Add_WhenAddingObjectToList_ShouldAddObjectToList()
        {
            //Arrange
            SetUpMocks();

            var list = new List<Patient>();
            var patient = Mock.Of<Patient>();

            mockDatabaseOperations.Setup(x => x.Add(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(true);

            //Act
            listManage.Add(patient, list);

            //Assert
            mockDatabaseOperations.Verify(x => x.Add(It.IsAny<Patient>(), It.IsAny<ISession>()), Times.Once);
            Assert.Contains(patient, list);
        }

        [Fact]
        public void Delete_WhenObjectIsNull_ShouldThrowArgumentNullExceptionAndNotDeleteFromList()
        {
            //Arrange
            SetUpMocks();

            Patient patient = null;
            var list = new List<Patient>();

            //Assert
            Assert.Throws<ArgumentNullException>(() => listManage.Delete(patient, list));
        }

        [Fact]
        public void Delete_WhenDatabaseOperationFails_ShouldThrowExceptionAndNotAddToList()
        {
            //Arrange
            SetUpMocks();

            var list = new List<Patient>();
            var patient = Mock.Of<Patient>();

            mockDatabaseOperations.Setup(x => x.Add(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(false);

            //Assert
            Assert.Throws<Exception>(() => listManage.Delete(patient, list));
            Assert.DoesNotContain(patient, list);
        }

        [Fact]
        public void Delete_WhenDeletingObjectToList_ShouldRemoveObjectFromList()
        {
            //Arrange
            SetUpMocks();

            var patient = Mock.Of<Patient>();
            var list = new List<Patient>() { patient };

            mockDatabaseOperations.Setup(x => x.Delete(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(true);

            //Act
            listManage.Delete(patient, list);

            //Assert
            mockDatabaseOperations.Verify(x => x.Delete(It.IsAny<Patient>(), It.IsAny<ISession>()), Times.Once);
            Assert.DoesNotContain(patient, list);
        }

        [Fact]
        public void Update_WhenObjectIsNull_ShouldThrowArgumentNullExceptionAndNotUpdateObject()
        {
            //Arrange
            SetUpMocks();

            Patient patient = null;
            var list = new List<Patient>();

            //Assert
            Assert.Throws<ArgumentNullException>(() => listManage.Update(patient, list));
            Assert.Equal(patient, patient);
        }

        [Fact]
        public void Update_WhenCannotFindObjectInList_ShouldThrowExceptionAndNotUpdateObject()
        {
            //Arrange
            SetUpMocks();

            var list = new List<Patient>();
            var patient = Mock.Of<Patient>();

            //Assert
            Assert.Throws<Exception>(() => listManage.Update(patient, list));
            Assert.Equal(patient, patient);
        }

        [Fact]
        public void Update_WhenDatabaseOperationFails_ShouldThrowExceptionAndNotUpdateObject()
        {
            //Arrange
            SetUpMocks();

            var patient = Mock.Of<Patient>();
            var list = new List<Patient>() { patient };

            mockDatabaseOperations.Setup(x => x.Update(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(false);

            //Assert
            Assert.Throws<Exception>(() => listManage.Update(patient, list));
            Assert.Equal(patient, patient);
        }

        [Fact]
        public void Update_WhenUpdatingObjectInList_ShouldUpdateObjectInList()
        {
            //Arrange
            SetUpMocks();

            var patient = Mock.Of<Patient>();
            patient.Id = 1;

            var patientChanged = Mock.Of<Patient>();
            patientChanged.Id = 1;
            patientChanged.Name = "changed";

            var list = new List<Patient> { patient };

            mockDatabaseOperations.Setup(x => x.Update(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(true);

            //Act
            listManage.Update(patientChanged, list);

            //Assert
            mockDatabaseOperations.Verify(x => x.Update(It.IsAny<Patient>(), It.IsAny<ISession>()), Times.Once);
            var updatedPatient = list.Single();
            Assert.Equal(patientChanged.Id, updatedPatient.Id);
            Assert.Equal(patientChanged.Name, updatedPatient.Name);
        }

        [Fact]
        public void SoftDelete_WhenObjectIsNull_ShouldThrowArgumentNullExceptionAndNotUpdateObject()
        {
            //Arrange
            SetUpMocks();

            Patient patient = null;
            var list = new List<Patient>();

            //Assert
            Assert.Throws<ArgumentNullException>(() => listManage.SoftDelete(patient, list));
        }

        [Fact]
        public void SoftDelete_WhenSoftDeletingObjectFromList_ShouldSoftDeleteObjectFromList()
        {
            //Arrange
            SetUpMocks();

            var patient = Mock.Of<Patient>();
            var list = new List<Patient>() { patient };

            mockDatabaseOperations.Setup(x => x.Update(It.IsAny<Patient>(), It.IsAny<ISession>()))
                                  .Returns(true);

            //Act
            listManage.SoftDelete(patient, list);

            //Assert
            Assert.True(patient.IsDeleted);
            Assert.DoesNotContain(patient, list);
        }
    }
}