using Hospital.Entities.Interfaces;

namespace Hospital.Utilities.ListManagement.Interfaces
{
    public interface IListManage
    {
        void Add<T>(T item, List<T> list) where T : IIntroduceString;
        void Delete<T>(T item, List<T> list) where T : IIntroduceString;
        void Update<T>(T item, List<T> list) where T : IIdentifier, IIntroduceString;
        void SoftDelete<T>(T item, List<T> list) where T : IIsDeleted, IIdentifier, IIntroduceString;
    }
}