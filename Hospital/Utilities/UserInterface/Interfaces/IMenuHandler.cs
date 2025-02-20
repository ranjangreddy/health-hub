using Hospital.Entities.Interfaces;

namespace Hospital.Utilities.UserInterface.Interfaces
{
    public interface IMenuHandler
    {
        T ShowInteractiveMenu<T>(List<T> items) where T : IIntroduceString;
        string ShowInteractiveMenu(List<string> options);
        T ShowInteractiveMenu<T>() where T : Enum;
        void ShowMessage(string message);
        T SelectObject<T>(List<T> list, string selectString) where T : IIntroduceString;
        void DisplayList<TEntity>(List<TEntity> objects) where TEntity : IIntroduceString;
    }
}