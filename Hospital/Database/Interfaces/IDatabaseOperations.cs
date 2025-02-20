using Hospital.Entities.Interfaces;
using NHibernate;

namespace Hospital.Database.Interfaces;

public interface IDatabaseOperations
{
    bool Add<T>(T entity, ISession session) where T : IIntroduceString;
    bool Delete<T>(T entity, ISession session) where T : IIntroduceString;
    bool Update<T>(T entity, ISession session) where T : IIntroduceString;
    List<T> GetAll<T>(ISession session) where T : IIntroduceString;
}