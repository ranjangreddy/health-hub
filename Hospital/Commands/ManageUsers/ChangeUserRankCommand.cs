using Hospital.Enums;
using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageUsers
{
    public class ChangeUserRankCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;

        public ChangeUserRankCommand(
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage)
            : base(UiMessages.ChangeUserRankMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            List<User> usersList = _listsStorage.Users;

            if (!usersList.Any())
            {
                _menuHandler.ShowMessage(UiMessages.ChangeUserRankMessages.NoUsersPrompt);
                return;
            }

            User user = _menuHandler.SelectObject(usersList, UiMessages.ChangeUserRankMessages.SelectUserPrompt);
            Rank rank = _menuHandler.ShowInteractiveMenu<Rank>();
            user.Rank = rank;

            _listManage.Update(user, usersList);

            _menuHandler.ShowMessage(string.Format(UiMessages.ChangeUserRankMessages.OperationSuccessPrompt,
                user.Login, rank));
        }
    }
}