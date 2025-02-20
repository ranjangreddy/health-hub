using Hospital.Utilities.UserInterface;

namespace Hospital.Commands.Navigation
{
    public class BackCommand : Command
    {
        private readonly INavigationService _navigationService;

        public BackCommand(
            INavigationService navigationService)
            : base(UiMessages.BackCommandMessages.Introduce)
        {
            _navigationService = navigationService;
        }

        public override void Execute()
        {
            var previousCommand = _navigationService.GetPreviousCommand() ?? throw new Exception(UiMessages.ExceptionMessages.Command);
            previousCommand.Execute();
        }
    }
}