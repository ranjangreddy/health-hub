using Hospital.Database.Interfaces;
using Hospital.Utilities;
using Hospital.Utilities.ErrorLogger;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Hospital.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ILogger _logger;
        private const string DatabaseName = "HospitalDB.db";
        public static string DatabasePath = GetDatabasePath();

        public DatabaseService(
            ILogger logger)
        {
            _logger = logger;
        }

        public void EnsureDatabaseExists(Configuration cfg)
        {
            try
            {
                if (!File.Exists(GetDatabasePath()))
                {
                    CreateDatabase(cfg);
                }
                else
                {
                    UpdateDatabaseSchema(cfg);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog(ex);
                throw;
            }
        }

        private static string GetDatabasePath()
        {
            return Path.Combine(FileService.DirectoryPath, DatabaseName);
        }

        private void CreateDatabase(Configuration cfg)
        {
            new SchemaExport(cfg).Create(false, true);
        }

        private void UpdateDatabaseSchema(Configuration cfg)
        {
            new SchemaUpdate(cfg).Execute(false, true);
        }
    }
}