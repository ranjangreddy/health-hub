using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageUsers
{
    public class DisplayUsersCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListsStorage _listsStorage;

        public DisplayUsersCommand(
            IMenuHandler menuHandler,
            IListsStorage listsStorage)
            : base(UiMessages.DisplayUsersCommandMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            _menuHandler.DisplayList(_listsStorage.Users);
        }
    }
}