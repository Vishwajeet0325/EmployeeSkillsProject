using System.ComponentModel.DataAnnotations;

namespace EmployeeSkillsManagement.Validation
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is DateTime date)
            {
                return date.Date >= new DateTime(1900, 1, 1)
                    && date.Date < DateTime.Today;
            }

            return false;
        }
    }
}