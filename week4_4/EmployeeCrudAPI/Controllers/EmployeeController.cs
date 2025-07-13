using Microsoft.AspNetCore.Mvc;
using EmployeeCrudAPI.Models;

namespace EmployeeCrudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        // Hardcoded Employee Data
        private static List<Employee> employees = new List<Employee>
        {
            new Employee 
            { 
                Id = 1, 
                Name = "John Doe", 
                Department = "IT", 
                Salary = 50000, 
                Email = "john@company.com",
                JoiningDate = DateTime.Now.AddYears(-2)
            },
            new Employee 
            { 
                Id = 2, 
                Name = "Jane Smith", 
                Department = "HR", 
                Salary = 45000, 
                Email = "jane@company.com",
                JoiningDate = DateTime.Now.AddYears(-1)
            },
            new Employee 
            { 
                Id = 3, 
                Name = "Mike Johnson", 
                Department = "Finance", 
                Salary = 55000, 
                Email = "mike@company.com",
                JoiningDate = DateTime.Now.AddMonths(-6)
            }
        };

        // GET - Read All Employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            return Ok(employees);
        }

        // GET - Read Employee by ID
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee id");
            }

            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return BadRequest("Invalid employee id");
            }

            return Ok(employee);
        }

        // POST - Create New Employee
        [HttpPost]
        public ActionResult<Employee> CreateEmployee([FromBody] Employee newEmployee)
        {
            if (newEmployee == null)
            {
                return BadRequest("Employee data is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate new ID
            newEmployee.Id = employees.Max(e => e.Id) + 1;
            employees.Add(newEmployee);

            return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.Id }, newEmployee);
        }

        // PUT - Update Employee (MAIN FOCUS)
        [HttpPut("{id}")]
        public ActionResult<Employee> UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            // Validate ID
            if (id <= 0)
            {
                return BadRequest("Invalid employee id");
            }

            // Check if employee exists
            var existingEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
            {
                return BadRequest("Invalid employee id");
            }

            // Validate input data
            if (updatedEmployee == null)
            {
                return BadRequest("Employee data is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Update the employee data
            existingEmployee.Name = updatedEmployee.Name;
            existingEmployee.Department = updatedEmployee.Department;
            existingEmployee.Salary = updatedEmployee.Salary;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.JoiningDate = updatedEmployee.JoiningDate;

            // Return updated employee
            return Ok(existingEmployee);
        }

        // DELETE - Delete Employee
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee id");
            }

            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return BadRequest("Invalid employee id");
            }

            employees.Remove(employee);
            return Ok($"Employee with ID {id} has been deleted successfully");
        }
    }
}