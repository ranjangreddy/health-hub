using Hospital.Enums;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageWards
{
    public class ChangeWardOwners : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;
        private Ward managedWard;

        public ChangeWardOwners(
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage)
            : base(UiMessages.ChangeWardOwners.Introduce)
        {
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            List<User> usersList = _listsStorage.Users;
            List<Ward> wardsList = _listsStorage.Wards;

            User user = _menuHandler.SelectObject(usersList, UiMessages.ChangeWardOwners.SelectUserPrompt);
            List<Ward> availableWardsForUser = wardsList.Where(ward => !user.AssignedWards.Contains(ward))
                                                        .ToList();
            Operation operation = _menuHandler.ShowInteractiveMenu<Operation>();

            if (!ValidateOperation(user, operation, wardsList))
            {
                return;
            }

            switch (operation)
            {
                case Operation.Add:
                    AddUserToWard(user, availableWardsForUser);
                    break;
                case Operation.Remove:
                    RemoveWardFromUser(user);
                    break;
            }

            _listManage.Update(user, usersList);
            _listManage.Update(managedWard, wardsList);

            _menuHandler.ShowMessage(string.Format(UiMessages.ChangeWardOwners.OperationSuccessPrompt,
                managedWard.Name, operation, user.Login));
        }

        private bool ValidateOperation(User user, Operation operation, List<Ward> availableWardsForUser)
        {
            if (operation == Operation.Add && !availableWardsForUser.Any())
            {
                _menuHandler.ShowMessage(UiMessages.ChangeWardOwners.NoWardToAssign);
                return false;
            }

            if (operation == Operation.Remove && !user.AssignedWards.Any())
            {
                _menuHandler.ShowMessage(UiMessages.ChangeWardOwners.NoWardPrompt);
                return false;
            }

            return true;
        }

        private void AddUserToWard(User user, List<Ward> availableWardsForUser)
        {
            managedWard = _menuHandler.SelectObject(availableWardsForUser,
                UiMessages.ChangeWardOwners.AssignUserToWardPrompt);
            user.AssignedWards.Add(managedWard);
            managedWard.AssignedUsers.Add(user);
        }

        private void RemoveWardFromUser(User user)
        {
            managedWard = _menuHandler.SelectObject(user.AssignedWards.ToList(),
                UiMessages.ChangeWardOwners.RemoveUserFromWardPrompt);
            user.AssignedWards.Remove(managedWard);
            managedWard.AssignedUsers.Remove(user);
        }
    }
}