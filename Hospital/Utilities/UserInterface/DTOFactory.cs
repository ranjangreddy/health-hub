using Hospital.Entities.Employee;
using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Utilities.UserInterface
{
    public class DTOFactory : IDTOFactory
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IInputHandler _inputHandler;

        public DTOFactory(IMenuHandler menuHandler, IInputHandler inputHandler)
        {
            _menuHandler = menuHandler;
            _inputHandler = inputHandler;
        }

        public PersonDTO GatherPersonData()
        {
            string name = _inputHandler.GetInput(UiMessages.FactoryMessages.ProvideNamePrompt);
            string surname = _inputHandler.GetInput(UiMessages.FactoryMessages.ProvideSurnamePrompt);
            Gender gender = _menuHandler.ShowInteractiveMenu<Gender>();
            DateTime birthday = _inputHandler.GetDateTimeInput(UiMessages.FactoryMessages.ProvideBirthdayPrompt);

            return new PersonDTO
            {
                Name = name,
                Surname = surname,
                Gender = gender,
                Birthday = birthday,
            };
        }

        public EmployeeDTO GatherEmployeeData(List<Ward> wards)
        {
            PersonDTO baseData = GatherPersonData();
            Ward ward = _menuHandler.ShowInteractiveMenu(wards);
            Position position = _menuHandler.ShowInteractiveMenu<Position>();
            List<Patient> assignedPatients = new();

            return new EmployeeDTO(baseData)
            {
                AssignedWard = ward,
                Position = position,
                AssignedPatients = assignedPatients
            };
        }

        public PatientDTO GatherPatientData(List<Ward> wards)
        {
            PersonDTO baseData = GatherPersonData();
            string personalIdNumber = _inputHandler.GetInput(UiMessages.FactoryMessages.ProvidePersonalIdNumberPrompt);
            Ward ward = _menuHandler.ShowInteractiveMenu(wards);

            return new PatientDTO(baseData)
            {
                PersonalIdNumber = personalIdNumber,
                AssignedWard = ward
            };
        }

        public UserDTO GatherUserData()
        {
            PersonDTO baseData = GatherPersonData();
            string login = _inputHandler.GetInput(UiMessages.FactoryMessages.ProvideLoginPrompt);
            string password = _inputHandler.GetInput(UiMessages.FactoryMessages.ProvidePasswordPrompt);
            Rank rank = _menuHandler.ShowInteractiveMenu<Rank>();
            List<Ward> assignedWards = new();

            return new UserDTO(baseData)
            {
                Login = login,
                Password = password,
                Rank = rank,
                AssignedWards = assignedWards
            };
        }

        public WardDTO GatherWardData()
        {
            string name = _inputHandler.GetInput(UiMessages.FactoryMessages.ProvideNamePrompt);
            int capacity = _inputHandler.GetIntInput(UiMessages.FactoryMessages.ProvideCapacityPrompt);
            List<Patient> assignedPatients = new();
            List<Employee> assignedEmployees = new();
            List<User> assignedUsers = new();

            return new WardDTO()
            {
                Name = name,
                Capacity = capacity,
                AssignedPatients = assignedPatients,
                AssignedEmployees = assignedEmployees,
                AssignedUsers = assignedUsers
            };
        }
    }
}