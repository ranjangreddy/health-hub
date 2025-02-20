using Hospital.Entities.Interfaces;
using Hospital.Utilities.UserInterface.Interfaces;
using System.Text;

namespace Hospital.Utilities.UserInterface
{
    public class MenuHandler : IMenuHandler
    {
        private readonly IConsoleService _consoleService;

        public MenuHandler(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public T ShowInteractiveMenu<T>(List<T> items) where T : IIntroduceString
        {
            var selectedLineIndex = 0;
            ConsoleKey pressedKey;

            do
            {
                UpdateInteractiveMenu(items, selectedLineIndex);
                pressedKey = Console.ReadKey().Key;

                if (pressedKey == ConsoleKey.DownArrow && selectedLineIndex + 1 < items.Count)
                    selectedLineIndex++;

                else if (pressedKey == ConsoleKey.UpArrow && selectedLineIndex - 1 >= 0)
                    selectedLineIndex--;

            } while (pressedKey != ConsoleKey.Enter);

            return items[selectedLineIndex];
        }

        public string ShowInteractiveMenu(List<string> options)
        {
            int selectedLineIndex = 0;
            ConsoleKey pressedKey;

            do
            {
                UpdateInteractiveMenu(options, selectedLineIndex);
                pressedKey = Console.ReadKey().Key;

                if (pressedKey == ConsoleKey.DownArrow && selectedLineIndex + 1 < options.Count)
                    selectedLineIndex++;

                else if (pressedKey == ConsoleKey.UpArrow && selectedLineIndex - 1 >= 0)
                    selectedLineIndex--;

            } while (pressedKey != ConsoleKey.Enter);

            return options[selectedLineIndex];
        }

        public T ShowInteractiveMenu<T>() where T : Enum
        {
            var options = Enum.GetNames(typeof(T)).ToList();
            int selectedLineIndex = 0;
            ConsoleKey pressedKey;

            do
            {
                UpdateInteractiveMenu(options, selectedLineIndex);
                pressedKey = Console.ReadKey().Key;

                if (pressedKey == ConsoleKey.DownArrow && selectedLineIndex + 1 < options.Count)
                    selectedLineIndex++;
                else if (pressedKey == ConsoleKey.UpArrow && selectedLineIndex - 1 >= 0)
                    selectedLineIndex--;

            } while (pressedKey != ConsoleKey.Enter);

            return (T)Enum.Parse(typeof(T), options[selectedLineIndex]);
        }

        private void UpdateInteractiveMenu<T>(List<T> items, int selectedIndex) where T : IIntroduceString
        {
            _consoleService.Clear();

            for (int i = 0; i < items.Count; i++)
            {
                bool isSelected = i == selectedIndex;
                if (isSelected)
                    DrawSelectedMenu(items[i].IntroduceString);
                else
                    Console.WriteLine(items[i].IntroduceString);
            }
        }

        private void UpdateInteractiveMenu(List<string> options, int selectedIndex)
        {
            _consoleService.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                bool isSelected = i == selectedIndex;
                if (isSelected)
                    DrawSelectedMenu(options[i]);
                else
                    _consoleService.WriteLine($"  {options[i]}");
            }
        }

        public void ShowMessage(string message)
        {
            ConsoleKey pressedKey;

            _consoleService.Clear();
            _consoleService.WriteLine(message);
            DrawSelectedMenu("Ok!");
            do
            {
                pressedKey = Console.ReadKey().Key;

            } while (pressedKey != ConsoleKey.Enter);
        }

        public T SelectObject<T>(List<T> list, string selectString) where T : IIntroduceString
        {
            ShowMessage(selectString);
            var obj = ShowInteractiveMenu(list);

            return obj;
        }

        public void DisplayList<TEntity>(List<TEntity> objects) where TEntity : IIntroduceString
        {
            StringBuilder introduceStrings = new();

            foreach (var obj in objects)
            {
                introduceStrings.AppendLine(obj.IntroduceString);
            }

            ShowMessage(introduceStrings.ToString());
        }

        public static void DrawSelectedMenu(string option)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"> {option}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}