using FluentNHibernate;
using Hospital.Database.Interfaces;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Reflection;

namespace Hospital.Database
{
    public class CreateSession
    {
        private readonly IDatabaseService _databaseService;

        public CreateSession(
            IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory ??= CreateSessionFactory(); }
        }

        private ISessionFactory CreateSessionFactory()
        {
            var configuration = new NHibernate.Cfg.Configuration();

            configuration.DataBaseIntegration(x =>
            {
                x.ConnectionString = $"Data Source={DatabaseService.DatabasePath};Version=3;";
                x.Driver<SQLite20Driver>();
                x.Dialect<SQLiteDialect>();
            });

            configuration.AddMappingsFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddAssembly(Assembly.GetExecutingAssembly());

            _databaseService.EnsureDatabaseExists(configuration);

            return configuration.BuildSessionFactory();
        }

        private ISessionFactory? _sessionFactory;
    }
}