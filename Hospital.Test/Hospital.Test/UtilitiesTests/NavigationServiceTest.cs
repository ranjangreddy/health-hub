using Hospital.Commands;
using Hospital.Commands.ManagePatients;
using Hospital.Commands.ManagePatients.ManagePatient;
using Hospital.Commands.ManageWards;
using Hospital.Commands.Navigation;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.UtilitiesTests
{
    public class NavigationServiceTest
    {
        private readonly NavigationService navigationService = new();

        private readonly Mock<Lazy<CreateWardCommand>> mockCreateWardCommand = new();
        private readonly Mock<Lazy<DisplayWardCommand>> mockDisplayWardCommand = new();
        private readonly Mock<Lazy<ChangeWardOwners>> mockChangeWardOwners = new();
        private readonly Mock<Lazy<DeleteWardCommand>> mockDeleteWardCommand = new();
        private readonly Mock<Lazy<BackCommand>> mockBackCommand = new();
        private readonly Mock<INavigationService> mockNavigationService = new();
        private readonly Mock<IMenuHandler> mockMenuHandler = new();

        private readonly Mock<Lazy<CreatePatientCommand>> mockCreatePatientCommand = new();
        private readonly Mock<Lazy<DisplayPatientsCommand>> mockDisplayPatientsCommand = new();
        private readonly Mock<Lazy<AssignToDoctorCommand>> mockAssignToDoctorCommand = new();
        private readonly Mock<Lazy<ChangeHealthStatusCommand>> mockChangeHealthStatusCommand = new();
        private readonly Mock<Lazy<DeletePatientCommand>> mockDeletePatientCommand = new();

        private ManageWardsCommand CreateMockManageWardsCommand()
        {
            return new ManageWardsCommand(
                mockCreateWardCommand.Object,
                mockDisplayWardCommand.Object,
                mockChangeWardOwners.Object,
                mockDeleteWardCommand.Object,
                mockBackCommand.Object,
                mockNavigationService.Object,
                mockMenuHandler.Object);
        }

        private ManagePatientsCommand CreateMockManagePatientsCommand()
        {
            var lazyCreatePatientCommand = new Lazy<CreatePatientCommand>(() => mockCreatePatientCommand.Object.Value);
            var lazyDisplayPatientsCommand = new Lazy<DisplayPatientsCommand>(() => mockDisplayPatientsCommand.Object.Value);
            var lazyAssignToDoctorCommand = new Lazy<AssignToDoctorCommand>(() => mockAssignToDoctorCommand.Object.Value);
            var lazyChangeHealthStatusCommand = new Lazy<ChangeHealthStatusCommand>(() => mockChangeHealthStatusCommand.Object.Value);
            var lazyDeletePatientCommand = new Lazy<DeletePatientCommand>(() => mockDeletePatientCommand.Object.Value);

            return new ManagePatientsCommand(
                lazyCreatePatientCommand,
                lazyDisplayPatientsCommand,
                lazyAssignToDoctorCommand,
                lazyChangeHealthStatusCommand,
                lazyDeletePatientCommand,
                mockBackCommand.Object,
                mockNavigationService.Object,
                mockMenuHandler.Object);
        }

        [Fact]
        public void Queue_WhenValidCommand_ShouldAddToStack()
        {
            var command = CreateMockManageWardsCommand();

            navigationService.Queue(command);

            Assert.Equal(command, navigationService.GetCurrentCommand());
        }

        [Fact]
        public void Queue_WhenInvalidCommand_ShouldNotAddToStack()
        {
            var invalidCommand = new TestCompositeCommand();

            navigationService.Queue(invalidCommand);

            Assert.Throws<InvalidOperationException>(navigationService.GetCurrentCommand);
        }

        [Fact]
        public void GetCurrentCommand_WhenCalled_ShouldReturnTopCommand()
        {
            var command = CreateMockManageWardsCommand();

            navigationService.Queue(command);

            var result = navigationService.GetCurrentCommand();

            Assert.Equal(command, result);
        }

        [Fact]
        public void GetPreviousCommand_WhenMultipleCommands_ShouldReturnPreviousCommand()
        {
            var firstCommand = CreateMockManageWardsCommand();
            var secondCommand = CreateMockManagePatientsCommand();

            navigationService.Queue(firstCommand);
            navigationService.Queue(secondCommand);

            var previousCommand = navigationService.GetPreviousCommand();

            Assert.Equal(firstCommand, previousCommand);
        }

        [Fact]
        public void GetPreviousCommand_WhenSingleCommand_ShouldThrowException()
        {
            var command = CreateMockManageWardsCommand();

            navigationService.Queue(command);

            Assert.Throws<InvalidOperationException>(navigationService.GetPreviousCommand);
        }
    }

    public class TestCompositeCommand : Command
    {
        public TestCompositeCommand() : base("Test") { }

        public override void Execute() { }
    }
}