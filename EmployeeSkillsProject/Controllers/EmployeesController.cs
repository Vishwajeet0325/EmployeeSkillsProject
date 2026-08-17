using EmployeeSkillsManagement.Data;
using EmployeeSkillsManagement.Models;
using EmployeeSkillsManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSkillsManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(
        string firstName,
        string lastName,
        string dateOfBirth,
        string phone,
        string skill,
        int page = 1)
        {
            const int pageSize = 5;

            if (page < 1)
            {
                page = 1;
            }

            var query = _context.Employees
                .Include(e => e.EmployeeSkills)
                .ThenInclude(es => es.Skill)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(e =>
                    e.LastName.Contains(lastName));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(e =>
                    e.Phone.Contains(phone));
            }

            if (!string.IsNullOrWhiteSpace(skill))
            {
                query = query.Where(e =>
                    e.EmployeeSkills.Any(es =>
                        es.Skill.Name.Contains(skill)));
            }

            if (!string.IsNullOrWhiteSpace(dateOfBirth)
                && DateTime.TryParse(dateOfBirth, out DateTime dob))
            {
                query = query.Where(e =>
                    e.DateOfBirth.Date == dob.Date);
            }

            var totalEmployees = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalEmployees / (double)pageSize);

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var employees = await query
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new EmployeeListViewModel
            {
                Employees = employees,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalEmployees = totalEmployees
            };

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.DateOfBirth = dateOfBirth;
            ViewBag.Phone = phone;
            ViewBag.Skill = skill;

            return View(viewModel);
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = await CreateEmployeeViewModelAsync();
            viewModel.DateOfBirth = null;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSkillsAsync(viewModel);
                return View(viewModel);
            }

            var employee = new Employee
            {
                FirstName = viewModel.FirstName.Trim(),
                LastName = viewModel.LastName.Trim(),
                DateOfBirth = viewModel.DateOfBirth!.Value,
                Phone = viewModel.Phone.Trim()
            };

            foreach (var skillId in viewModel.SelectedSkillIds.Distinct())
            {
                employee.EmployeeSkills.Add(
                    new EmployeeSkill
                    {
                        SkillId = skillId
                    });
            }

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Employee created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.Include(e => e.EmployeeSkills) .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            var skills = await _context.Skills
                .OrderBy(s => s.Name)
                .ToListAsync();

            var selectedSkillIds = employee.EmployeeSkills
                .Select(es => es.SkillId)
                .ToList();

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                Phone = employee.Phone,
                SelectedSkillIds = selectedSkillIds,
                Skills = skills.Select(s => new SkillSelectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsSelected = selectedSkillIds.Contains(s.Id)
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            EmployeeViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateSkillsAsync(viewModel);
                return View(viewModel);
            }

            var employee = await _context.Employees
                .Include(e => e.EmployeeSkills)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = viewModel.FirstName.Trim();
            employee.LastName = viewModel.LastName.Trim();
            employee.DateOfBirth = viewModel.DateOfBirth!.Value;
            employee.Phone = viewModel.Phone.Trim();

            employee.EmployeeSkills.Clear();

            foreach (var skillId in viewModel.SelectedSkillIds.Distinct())
            {
                employee.EmployeeSkills.Add(
                    new EmployeeSkill
                    {
                        EmployeeId = employee.Id,
                        SkillId = skillId
                    });
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Employee updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.EmployeeSkills)
                .ThenInclude(es => es.Skill)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Employee deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<EmployeeViewModel>
            CreateEmployeeViewModelAsync()
        {
            var skills = await _context.Skills
                .OrderBy(s => s.Name)
                .ToListAsync();

            return new EmployeeViewModel
            {
                Skills = skills.Select(s =>
                    new SkillSelectionViewModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToList()
            };
        }

        private async Task PopulateSkillsAsync(
            EmployeeViewModel viewModel)
        {
            var skills = await _context.Skills
                .OrderBy(s => s.Name)
                .ToListAsync();

            viewModel.Skills = skills.Select(s =>
                new SkillSelectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsSelected =
                        viewModel.SelectedSkillIds.Contains(s.Id)
                })
                .ToList();
        }
    }
}