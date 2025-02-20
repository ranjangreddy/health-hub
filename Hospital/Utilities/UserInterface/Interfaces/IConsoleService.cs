namespace Hospital.Utilities.UserInterface.Interfaces
{
    public interface IConsoleService
    {
        void Clear();
        void WriteLine(string message);
        string ReadLine();
    }
}