using Autofac;
using Hospital.Commands.LoginWindow;
using Hospital.Commands.ManageEmployees;
using Hospital.Commands.ManagePatients;
using Hospital.Commands.ManagePatients.ManagePatient;
using Hospital.Commands.ManageUsers;
using Hospital.Commands.ManageWards;
using Hospital.Commands.Navigation;
using Hospital.Database;
using Hospital.Database.Interfaces;
using Hospital.Utilities;
using Hospital.Utilities.EntitiesFactory;
using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.ErrorLogger;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.ListManagement;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands
{
    internal class AutofacConfig
    {
        public IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            //Utilities / Database
            builder.RegisterType<ConsoleService>().As<IConsoleService>().SingleInstance();
            builder.RegisterType<MenuHandler>().As<IMenuHandler>().SingleInstance();
            builder.RegisterType<DTOFactory>().As<IDTOFactory>().SingleInstance();
            builder.RegisterType<ObjectsFactory>().As<IObjectsFactory>().SingleInstance();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<InputHandler>().As<IInputHandler>().SingleInstance();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<DatabaseService>().As<IDatabaseService>().SingleInstance();
            builder.RegisterType<CreateSession>().AsSelf().SingleInstance();
            builder.RegisterType<DatabaseOperations>().As<IDatabaseOperations>().SingleInstance();
            builder.RegisterType<ListsStorage>().As<IListsStorage>().SingleInstance();
            builder.RegisterType<ListManage>().As<IListManage>().SingleInstance();
            builder.RegisterType<ManageCapacity>().As<IManageCapacity>().SingleInstance();
            builder.RegisterType<Validators>().As<IValidators>().SingleInstance();
            builder.RegisterType<ValidateObjects>().As<IValidateObjects>().SingleInstance();
            builder.Register(c => new StreamWriter(FileService.LogFilePath, true)).As<StreamWriter>().SingleInstance();

            //Commands
            builder.RegisterType<ExitCommand>().AsSelf().SingleInstance();
            builder.RegisterType<BackCommand>().AsSelf().SingleInstance();
            builder.RegisterType<LogoutCommand>().AsSelf().SingleInstance();

            //LoginWindowCommand
            builder.RegisterType<LoginCommand>().AsSelf().SingleInstance();
            builder.RegisterType<LogoutCommand>().AsSelf().SingleInstance();
            builder.RegisterType<LoginWindowCommand>().AsSelf().SingleInstance();

            //ManageEmployeesCommand
            builder.RegisterType<DeleteEmployeeCommand>().AsSelf().SingleInstance();
            builder.RegisterType<DisplayEmployeesCommand>().AsSelf().SingleInstance();
            builder.RegisterType<CreateEmployeeCommand>().AsSelf().SingleInstance();
            builder.RegisterType<ManageEmployeesCommand>().AsSelf().SingleInstance();

            //ManagePatientsCommand
            builder.RegisterType<CreatePatientCommand>().AsSelf().SingleInstance();
            builder.RegisterType<DisplayPatientsCommand>().AsSelf().SingleInstance();
            builder.RegisterType<AssignToDoctorCommand>().AsSelf().SingleInstance();
            builder.RegisterType<ChangeHealthStatusCommand>().AsSelf().SingleInstance();
            builder.RegisterType<DeletePatientCommand>().AsSelf().SingleInstance();
            builder.RegisterType<ManagePatientsCommand>().AsSelf().SingleInstance();

            //ManageUsersCommand
            builder.RegisterType<CreateUserCommand>().AsSelf().SingleInstance();
            builder.RegisterType<ChangeUserRankCommand>().AsSelf().SingleInstance();
            builder.RegisterType<DisplayUsersCommand>().AsSelf().SingleInstance();
            builder.RegisterType<DeleteUserCommand>().AsSelf().SingleInstance();
            builder.RegisterType<ManageUsersCommand>().AsSelf().SingleInstance();

            //ManageWardsCommand
            builder.RegisterType<CreateWardCommand>().AsSelf().SingleInstance();
            builder.RegisterType<DisplayWardCommand>().AsSelf().SingleInstance();
            builder.RegisterType<ChangeWardOwners>().AsSelf().SingleInstance();
            builder.RegisterType<DeleteWardCommand>().AsSelf().SingleInstance();
            builder.RegisterType<ManageWardsCommand>().AsSelf().SingleInstance();

            //MainWindowCommand
            builder.RegisterType<MainWindowCommand>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}