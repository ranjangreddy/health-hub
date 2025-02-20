using Hospital.Entities.Interfaces;

namespace Hospital.Commands
{
    public abstract class Command : IIntroduceString
    {
        public string IntroduceString { get; }

        protected Command(string introduceString)
        {
            IntroduceString = introduceString;
        }

        public abstract void Execute();
    }
}