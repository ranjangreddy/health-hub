using Hospital.Utilities.UserInterface;

namespace Hospital.Commands.Navigation
{
    public class ExitCommand : Command
    {
        public ExitCommand() : base(UiMessages.ExitCommandMessages.Introduce) { }

        public override void Execute()
        {
            Environment.Exit(0);
        }
    }
}
