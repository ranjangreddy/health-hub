using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Utilities.UserInterface
{
    public class ConsoleService : IConsoleService
    {
        public void Clear() => Console.Clear();

        public void WriteLine(string message) => Console.WriteLine(message);

        public string ReadLine() => Console.ReadLine();
    }
}