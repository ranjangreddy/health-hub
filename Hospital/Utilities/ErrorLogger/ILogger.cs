namespace Hospital.Utilities.ErrorLogger
{
    public interface ILogger
    {
        void WriteLog(Exception ex);
    }
}