namespace Hospital.Utilities.ErrorLogger
{
    public class Logger : ILogger
    {
        private readonly StreamWriter _streamWriter;

        public Logger(
            StreamWriter streamWriter)
        {
            _streamWriter = streamWriter;
        }

        public void WriteLog(Exception ex)
        {
            _streamWriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | " + ex.ToString());
            _streamWriter.Flush();
        }
    }
}