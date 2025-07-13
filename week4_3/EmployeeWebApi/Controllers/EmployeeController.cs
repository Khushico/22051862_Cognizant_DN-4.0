using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeWebApi.Models;
using EmployeeWebApi.Filters;

namespace EmployeeWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CustomAuthFilter] // Apply custom authorization filter to all actions
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> employees;

        // Constructor - Initialize sample data
        public EmployeeController()
        {
            if (employees == null)
            {
                employees = GetStandardEmployeeList();
            }
        }

        // Private method to create sample employee data
        private List<Employee> GetStandardEmployeeList()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "John Doe",
                    Salary = 75000,
                    Permanent = true,
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Department = new Department { Id = 1, Name = "IT", Location = "Building A" },
                    Skills = new List<Skill>
                    {
                        new Skill { Id = 1, Name = "C#", Level = "Advanced" },
                        new Skill { Id = 2, Name = "ASP.NET Core", Level = "Intermediate" }
                    }
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Salary = 85000,
                    Permanent = true,
                    DateOfBirth = new DateTime(1988, 8, 22),
                    Department = new Department { Id = 2, Name = "HR", Location = "Building B" },
                    Skills = new List<Skill>
                    {
                        new Skill { Id = 3, Name = "Communication", Level = "Advanced" },
                        new Skill { Id = 4, Name = "Leadership", Level = "Intermediate" }
                    }
                },
                new Employee
                {
                    Id = 3,
                    Name = "Mike Johnson",
                    Salary = 65000,
                    Permanent = false,
                    DateOfBirth = new DateTime(1992, 12, 10),
                    Department = new Department { Id = 1, Name = "IT", Location = "Building A" },
                    Skills = new List<Skill>
                    {
                        new Skill { Id = 5, Name = "JavaScript", Level = "Intermediate" },
                        new Skill { Id = 6, Name = "React", Level = "Beginner" }
                    }
                },
                new Employee
                {
                    Id = 4,
                    Name = "Sarah Wilson",
                    Salary = 92000,
                    Permanent = true,
                    DateOfBirth = new DateTime(1985, 3, 8),
                    Department = new Department { Id = 3, Name = "Finance", Location = "Building C" },
                    Skills = new List<Skill>
                    {
                        new Skill { Id = 7, Name = "Excel", Level = "Advanced" },
                        new Skill { Id = 8, Name = "Financial Analysis", Level = "Expert" }
                    }
                }
            };
        }

        // GET: api/Employee - Get all employees
        [HttpGet]
        [AllowAnonymous] // This endpoint doesn't require auth for demo purposes
        [ProducesResponseType(typeof(List<Employee>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<List<Employee>> GetStandard()
        {
            return Ok(employees);
        }

        // GET: api/Employee/{id} - Get employee by ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Employee> GetById(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok(employee);
        }

        // POST: api/Employee - Create new employee
        [HttpPost]
        [ProducesResponseType(typeof(Employee), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is required");
            }

            if (string.IsNullOrEmpty(employee.Name))
            {
                return BadRequest("Employee name is required");
            }

            if (employee.Salary <= 0)
            {
                return BadRequest("Employee salary must be greater than 0");
            }

            // Generate new ID
            employee.Id = employees.Any() ? employees.Max(e => e.Id) + 1 : 1;
            
            employees.Add(employee);
            
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        // PUT: api/Employee/{id} - Update existing employee
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Employee> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is required");
            }

            if (string.IsNullOrEmpty(employee.Name))
            {
                return BadRequest("Employee name is required");
            }

            var existingEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            // Update properties
            existingEmployee.Name = employee.Name;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Permanent = employee.Permanent;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.Department = employee.Department;
            existingEmployee.Skills = employee.Skills;

            return Ok(existingEmployee);
        }

        // DELETE: api/Employee/{id} - Delete employee
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult DeleteEmployee(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            employees.Remove(employee);
            return NoContent();
        }

        // GET: api/Employee/test-exception - Test endpoint for exception filter
        [HttpGet("test-exception")]
        [ProducesResponseType(500)]
        public ActionResult TestException()
        {
            throw new Exception("This is a test exception to demonstrate the custom exception filter working properly!");
        }

        // GET: api/Employee/department/{departmentId} - Get employees by department
        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(typeof(List<Employee>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<List<Employee>> GetByDepartment(int departmentId)
        {
            var departmentEmployees = employees.Where(e => e.Department?.Id == departmentId).ToList();
            
            if (!departmentEmployees.Any())
            {
                return NotFound($"No employees found in department with ID {departmentId}");
            }
            
            return Ok(departmentEmployees);
        }

        // GET: api/Employee/permanent - Get only permanent employees
        [HttpGet("permanent")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Employee>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<List<Employee>> GetPermanentEmployees()
        {
            var permanentEmployees = employees.Where(e => e.Permanent).ToList();
            return Ok(permanentEmployees);
        }
    }
}