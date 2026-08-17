using EmployeeSkillsManagement.Models;

namespace EmployeeSkillsManagement.ViewModels
{
    public class EmployeeListViewModel
    {
        public List<Employee> Employees { get; set; } = new();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalEmployees { get; set; }
    }
}