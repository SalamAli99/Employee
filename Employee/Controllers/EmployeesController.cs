using Employee.Data;
using Employee.Models;
using Employee.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext employeeDbContext;
        public EmployeesController(EmployeeDbContext employeeDbContext)
        {
            this.employeeDbContext = employeeDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           var employees= await employeeDbContext.Employees.ToListAsync();
            return View(employees);
            
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmpReq)
        {
            var employee = new Employe()
            {
                Id = Guid.NewGuid(),
                Name = addEmpReq.Name,
                Email = addEmpReq.Email,
                Salary = addEmpReq.Salary,
                Department = addEmpReq.Department,
                DateOfBirth = addEmpReq.DateOfBirth

            };
           await employeeDbContext.Employees.AddAsync(employee);
           await employeeDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
           var employee= await employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var ViewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth

                };
                return await  Task.Run(()=>View("View",ViewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await employeeDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {

                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;

                employee.DateOfBirth = model.DateOfBirth;

                employee.Department = model.Department;

                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult>Delete(UpdateEmployeeViewModel model)
        {
            var employee =await employeeDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employeeDbContext.Employees.Remove(employee);
                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
