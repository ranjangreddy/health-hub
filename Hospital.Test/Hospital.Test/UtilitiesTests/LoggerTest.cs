using Hospital.Utilities.ErrorLogger;
using Moq;

namespace Hospital.Test.UtilitiesTests
{
    public class LoggerTest
    {
        private Mock<StreamWriter> mockStreamWriter;

        private Logger logger;

        private void SetUpMocks()
        {
            mockStreamWriter = new Mock<StreamWriter>(MockBehavior.Strict, new MemoryStream());
            mockStreamWriter.Setup(sw => sw.WriteLine(It.IsAny<string>()));
            mockStreamWriter.Setup(sw => sw.Flush());

            logger = new Logger(mockStreamWriter.Object);
        }

        [Fact]
        public void WriteLog_WhenExceptionThrown_ShouldWriteExcpetion()
        {
            SetUpMocks();

            var exceptionMessage = "Test exception";
            var exception = new Exception(exceptionMessage);

            logger.WriteLog(exception);

            mockStreamWriter.Verify(sw => sw.WriteLine(It.Is<string>(s => s.Contains(exceptionMessage))), Times.Once());
            mockStreamWriter.Verify(sw => sw.Flush(), Times.Once());
        }
    }
}