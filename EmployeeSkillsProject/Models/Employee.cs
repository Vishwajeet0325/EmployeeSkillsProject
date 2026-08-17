using EmployeeSkillsManagement.Validation;
using System.ComponentModel.DataAnnotations;

namespace EmployeeSkillsManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of birth must be in the past.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [RegularExpression( @"^[0-9+\-\s()]{7,20}$",
            ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; } = string.Empty;

        public ICollection<EmployeeSkill> EmployeeSkills { get; set; }
            = new List<EmployeeSkill>();
    }
}