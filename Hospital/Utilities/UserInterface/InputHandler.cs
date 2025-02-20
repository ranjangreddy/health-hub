using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Utilities.UserInterface
{
    public class InputHandler : IInputHandler
    {
        private readonly IConsoleService _consoleService;

        public InputHandler(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public string GetInput(string prompt)
        {
            return GetInputWithValidation<string>(prompt, input => true);
        }

        public int GetIntInput(string prompt)
        {
            return GetInputWithValidation<int>(prompt, input => int.TryParse(input, out _));
        }

        public DateTime GetDateTimeInput(string prompt)
        {
            return GetInputWithValidation<DateTime>(prompt, input => DateTime.TryParse(input, out _));
        }

        private T GetInputWithValidation<T>(string prompt, Func<string, bool> validationFunc)
        {
            string? input;

            do
            {
                _consoleService.Clear();
                _consoleService.WriteLine(prompt);
                input = _consoleService.ReadLine();

                if (input == UiMessages.InputHandler.StopMessage)
                    throw new Exception(UiMessages.ExceptionMessages.OperationTerminated);

                if (validationFunc(input))
                    return (T)Convert.ChangeType(input, typeof(T));

            } while (true);
        }
    }
}