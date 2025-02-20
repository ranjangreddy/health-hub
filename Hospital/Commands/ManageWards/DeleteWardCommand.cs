using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageWards
{
    public class DeleteWardCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;

        public DeleteWardCommand(
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage)
            : base(UiMessages.DeleteWardMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            if (!_listsStorage.Wards.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DeleteWardMessages.NoWardPrompt);
                return;
            }

            var selectedWard = _menuHandler.SelectObject(
                _listsStorage.Wards, UiMessages.DeleteWardMessages.SelectWardPrompt);

            if (selectedWard.AssignedPatients.Any() || selectedWard.AssignedEmployees.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DeleteWardMessages.WardNonEmptyPrompt);
                return;
            }

            _listManage.SoftDelete(selectedWard, _listsStorage.Wards);

            _menuHandler.ShowMessage(string.Format(UiMessages.DeleteWardMessages.OperationSuccessPrompt,
                selectedWard.Name));
        }
    }
}