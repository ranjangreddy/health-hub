using Autofac;
using Hospital.Commands;
using Hospital.Commands.LoginWindow;
using Hospital.Commands.Navigation;
using Hospital.Enums;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities;
using Hospital.Utilities.ErrorLogger;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital
{
    internal static class Program
    {
        public static IContainer Container;

        private static void Main()
        {
            var fileService = new FileService();
            var config = new AutofacConfig();

            InitializeApplication(fileService, config);
            ExecuteApplication();
        }

        private static void InitializeApplication(FileService fileService, AutofacConfig config)
        {
            fileService.CreateDirectory();
            fileService.CreateLogFile();
            Container = config.ConfigureContainer();
        }

        private static void CreateDefaultUser(IListsStorage listStorage, IListManage listManage)
        {
            if (!listStorage.Users.Any())
            {
                var user = new User(
                    "admin",
                    "admin",
                    Gender.Male,
                    new DateTime(),
                    "admin",
                    "admin",
                    Rank.Admin,
                    new List<Ward>());

                listManage.Add(user, listStorage.Users);
            }
        }

        private static void ExecuteApplication()
        {
            var loginCommand = Container.Resolve<LoginCommand>();
            var loginWindow = Container.Resolve<LoginWindowCommand>();
            var mainWindow = Container.Resolve<MainWindowCommand>();
            var logger = Container.Resolve<ILogger>();
            var menuHandler = Container.Resolve<IMenuHandler>();
            var mainQueue = Container.Resolve<INavigationService>();

            mainQueue.Queue(loginWindow);
            mainQueue.Queue(mainWindow);

            CreateDefaultUser(Container.Resolve<IListsStorage>(), Container.Resolve<IListManage>());

            while (true)
            {
                try
                {
                    if (!loginCommand.IsLoggedIn)
                    {
                        loginWindow.Execute();
                    }
                    else
                    {
                        mainQueue.GetCurrentCommand().Execute();
                    }
                }
                catch (Exception ex)
                {
                    menuHandler.ShowMessage(ex.Message);
                    logger.WriteLog(ex);
                }
            }
        }
    }
}