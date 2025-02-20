using Hospital.Commands.LoginWindow;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageWards
{
    public class DisplayWardCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListsStorage _listsStorage;

        public DisplayWardCommand(
            IMenuHandler menuHandler,
            IListsStorage listsStorage)
            : base(UiMessages.DisplayWardMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            List<Ward> wardsList = _listsStorage.Wards;

            if (!wardsList.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DisplayWardMessages.NoWardPrompt);
                return;
            }

            List<Ward> availableWardsForUser = LoginCommand.CurrentlyLoggedIn.AssignedWards.ToList();
            if (!availableWardsForUser.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DisplayWardMessages.NoWardAssignedToUser);
                return;
            }

            Ward selectedWard = _menuHandler.SelectObject(availableWardsForUser, UiMessages.DisplayWardMessages.SelectWardPrompt);

            DisplayWardInformation(selectedWard);
        }

        private void DisplayWardInformation(Ward ward)
        {
            string wardInformation = string.Format(
                UiMessages.DisplayWardMessages.DisplayInformationPrompt,
                ward.Name,
                ward.PatientsNumber,
                ward.Capacity,
                ward.PatientsNumber / ward.Capacity,
                ward.AssignedEmployees.Count
            );

            _menuHandler.ShowMessage(wardInformation);
        }
    }
}