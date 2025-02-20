namespace Hospital.Commands.Navigation
{
    public interface INavigationService
    {
        Command GetPreviousCommand();
        Command GetCurrentCommand();
        void Queue(Command command);
    }
}