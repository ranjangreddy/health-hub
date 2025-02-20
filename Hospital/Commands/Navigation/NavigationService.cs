using Hospital.Commands.ManageEmployees;
using Hospital.Commands.ManagePatients;
using Hospital.Commands.ManageUsers;
using Hospital.Commands.ManageWards;

namespace Hospital.Commands.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Stack<Command> CommandStack = new();
        private readonly List<Type> NavigationCommands = new()
        {
            typeof(MainWindowCommand),
            typeof(ManageEmployeesCommand),
            typeof(ManagePatientsCommand),
            typeof(ManageWardsCommand),
            typeof(ManageUsersCommand)
        };

        public Command GetPreviousCommand()
        {
            CommandStack.Pop();
            return CommandStack.Peek();
        }

        public Command GetCurrentCommand()
        {
            return CommandStack.Peek();
        }

        public void Queue(Command command)
        {
            if (NavigationCommands.Contains(command.GetType()))
                CommandStack.Push(command);
        }
    }
}