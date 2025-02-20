using Hospital.Entities.Employee;
using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Utilities.EntitiesFactory
{
    public class ValidateObjects : IValidateObjects
    {
        private readonly IValidators _validate;
        private readonly IMenuHandler _menuHandler;

        public ValidateObjects(
            IValidators validate,
            IMenuHandler menuHandler)
        {
            _validate = validate;
            _menuHandler = menuHandler;
        }

        private bool ValidateField<T>(T value, Func<T, bool> validationFunction, string errorDescription)
        {
            if (!validationFunction(value))
            {
                _menuHandler.ShowMessage(errorDescription);
                return false;
            }

            return true;
        }

        private bool ValidatePerson(string name, string surname, Gender gender, DateTime birthday)
        {
            return ValidateField(name, _validate.ValidateString, UiMessages.ExceptionMessages.String)
                && ValidateField(surname, _validate.ValidateString, UiMessages.ExceptionMessages.String)
                && ValidateField(gender, _validate.ValidateGender, UiMessages.ExceptionMessages.Gender)
                && ValidateField(birthday, _validate.ValidateBirthday, UiMessages.ExceptionMessages.Date);
        }

        public bool ValidateEmployeeObject(EmployeeDTO dto)
        {
            return ValidatePerson(dto.Name, dto.Surname, dto.Gender, dto.Birthday)
                && ValidateField(dto.Position, _validate.ValidatePosition, UiMessages.ExceptionMessages.String);
        }

        public bool ValidatePatientObject(PatientDTO dto)
        {
            return ValidatePerson(dto.Name, dto.Surname, dto.Gender, dto.Birthday)
                && ValidateField(dto.PersonalIdNumber, _validate.ValidatePersonalIdNumber, UiMessages.ExceptionMessages.PersonalIdNumber)
                && ValidateField(dto.AssignedWard, _validate.ValidatePossibilityAssignToWard, UiMessages.ExceptionMessages.WardFull);
        }

        public bool ValidateUserObject(UserDTO dto)
        {
            return ValidateField(dto.Name, _validate.ValidateString, UiMessages.ExceptionMessages.String)
                && ValidateField(dto.Surname, _validate.ValidateString, UiMessages.ExceptionMessages.String)
                && ValidateField(dto.Login, _validate.ValidateLogin, UiMessages.ExceptionMessages.Login)
                && ValidateField(dto.Password, _validate.ValidatePassword, UiMessages.ExceptionMessages.Password);
        }

        public bool ValidateWardObject(WardDTO dto)
        {
            return ValidateField(dto.Name, _validate.ValidateWardName, UiMessages.ExceptionMessages.WardName)
                && ValidateField(dto.Capacity, _validate.ValidateCapacity, UiMessages.ExceptionMessages.Capacity);
        }
    }
}