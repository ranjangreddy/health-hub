using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.UtilitiesTests
{
    public class InputHandlerTest
    {
        private Mock<IConsoleService> mockConsoleService;

        private InputHandler inputHandler;

        private void SetUpMocks()
        {
            mockConsoleService = new Mock<IConsoleService>();

            inputHandler = new InputHandler(mockConsoleService.Object);
        }

        [Fact]
        public void GetInput_WhenStringProvided_ShouldReturnStringValue()
        {
            SetUpMocks();

            var expectedValue = "Test Input";

            mockConsoleService.Setup(x => x.ReadLine())
                              .Returns(expectedValue);

            var result = inputHandler.GetInput("");

            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void GetInput_WhenStopMessageProvided_ShouldThrowException()
        {
            SetUpMocks();

            mockConsoleService.Setup(x => x.ReadLine())
                              .Returns(UiMessages.InputHandler.StopMessage);

            Assert.Throws<Exception>(() => inputHandler.GetInput(""));
        }

        [Fact]
        public void GetIntInput_WhenStringProvided_ShouldReturnIntValue()
        {
            SetUpMocks();

            var expectedValue = 12;

            mockConsoleService.Setup(x => x.ReadLine())
                              .Returns("12");

            var result = inputHandler.GetIntInput("");

            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void GetIntInput_WhenStopMessageProvided_ShouldThrowException()
        {
            SetUpMocks();

            mockConsoleService.Setup(x => x.ReadLine())
                              .Returns(UiMessages.InputHandler.StopMessage);

            Assert.Throws<Exception>(() => inputHandler.GetIntInput(""));
            var exception = Assert.Throws<Exception>(() => inputHandler.GetIntInput(""));
            Assert.Equal(UiMessages.ExceptionMessages.OperationTerminated, exception.Message);
        }

        [Fact]
        public void GetIntInput_WhenFirstDataInvalidAndSecondValidProvided_ShouldRepeatsPromptTwice()
        {
            SetUpMocks();

            mockConsoleService.SetupSequence(x => x.ReadLine())
                              .Returns("invalid")
                              .Returns("5");

            var result = inputHandler.GetIntInput("");

            mockConsoleService.Verify(x => x.WriteLine(""), Times.Exactly(2));
            mockConsoleService.Verify(x => x.ReadLine(), Times.Exactly(2));
            Assert.Equal(5, result);
        }

        [Fact]
        public void GetDateTimeInput_WhenValidStringProvided_ShouldReturnDateTimeValue()
        {
            SetUpMocks();

            var expectedValue = new DateTime(1990, 1, 1);

            mockConsoleService.SetupSequence(x => x.ReadLine())
                              .Returns("1990-1-1");

            var result = inputHandler.GetDateTimeInput("");

            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void GetDateTimeInput_WhenStopMessageProvided_ShouldThrowException()
        {
            SetUpMocks();

            mockConsoleService.Setup(x => x.ReadLine())
                              .Returns(UiMessages.InputHandler.StopMessage);

            var exception = Assert.Throws<Exception>(() => inputHandler.GetDateTimeInput(""));
            Assert.Equal(UiMessages.ExceptionMessages.OperationTerminated, exception.Message);
        }

        [Fact]
        public void GetDateTimeInput_WhenFirstDataInvalidAndSecondValidProvided_ShouldRepeatsPromptTwice()
        {
            SetUpMocks();

            var expectedValue = new DateTime(1990, 1, 1);

            mockConsoleService.SetupSequence(x => x.ReadLine())
                              .Returns("invalid")
                              .Returns("1990-1-1");

            var result = inputHandler.GetDateTimeInput("");

            Assert.Equal(expectedValue, result);
            mockConsoleService.Verify(x => x.WriteLine(""), Times.Exactly(2));
            mockConsoleService.Verify(x => x.ReadLine(), Times.Exactly(2));
        }
    }
}