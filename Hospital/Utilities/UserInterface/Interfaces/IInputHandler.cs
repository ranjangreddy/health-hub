namespace Hospital.Utilities.UserInterface.Interfaces
{
    public interface IInputHandler
    {
        string GetInput(string prompt);
        int GetIntInput(string prompt);
        DateTime GetDateTimeInput(string prompt);
    }
}