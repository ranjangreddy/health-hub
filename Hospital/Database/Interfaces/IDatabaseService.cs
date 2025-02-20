using NHibernate.Cfg;

namespace Hospital.Database.Interfaces
{
    public interface IDatabaseService
    {
        void EnsureDatabaseExists(Configuration configuration);
    }
}