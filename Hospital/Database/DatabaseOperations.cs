using Hospital.Database.Interfaces;
using Hospital.Entities.Interfaces;
using Hospital.Utilities.ErrorLogger;
using Hospital.Utilities.UserInterface;
using NHibernate;

namespace Hospital.Database
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly ILogger _logger;
        private delegate void DatabaseOperation<T>(T entity, ISession session);

        public DatabaseOperations(
            ILogger logger)
        {
            _logger = logger;
        }

        private bool ExecuteInTransaction<T>(T entity, ISession session, DatabaseOperation<T> operation)
        {
            using var transaction = session.BeginTransaction();

            try
            {
                operation(entity, session);
                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.WriteLog(ex);

                return false;
            }
        }

        public bool Add<T>(T entity, ISession session) where T : IIntroduceString
        {
            if (ExecuteInTransaction(entity, session, (e, s) => s.Save(e)))
                return true;

            return false;
        }

        public bool Delete<T>(T entity, ISession session) where T : IIntroduceString
        {
            if (ExecuteInTransaction(entity, session, (e, s) => s.Delete(e)))
                return true;

            return false;
        }

        public bool Update<T>(T entity, ISession session) where T : IIntroduceString
        {
            if (ExecuteInTransaction(entity, session, (e, s) => s.Update(e)))
                return true;

            return false;
        }

        public List<T> GetAll<T>(ISession session) where T : IIntroduceString
        {
            try
            {
                return session.Query<T>().ToList();
            }
            catch (Exception ex)
            {
                _logger.WriteLog(ex);
                throw new(UiMessages.DatabaseExceptions.QueryException);
            }
        }
    }
}