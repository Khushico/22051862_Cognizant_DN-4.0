using Microsoft.AspNetCore.Mvc;

namespace MyFirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // यह बाद में change करेंगे
    public class EmployeeController : ControllerBase
    {
        // Fake Employee Data
        private static List<Employee> employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Rahul Sharma", Department = "IT", Salary = 50000 },
            new Employee { Id = 2, Name = "Priya Patel", Department = "HR", Salary = 45000 },
            new Employee { Id = 3, Name = "Amit Kumar", Department = "Finance", Salary = 55000 }
        };

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            return Ok(employees);
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        [HttpGet("{id}", Name = "GetEmployeeById")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Employee> GetEmployee(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok(employee);
        }

        /// <summary>
        /// Create new employee
        /// </summary>
        /// <param name="employee">Employee object</param>
        /// <returns>Created employee</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Employee), 201)]
        [ProducesResponseType(400)]
        public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is required");
            }

            employee.Id = employees.Count + 1;
            employees.Add(employee);
            
            return CreatedAtRoute("GetEmployeeById", new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Update existing employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="employee">Updated employee data</param>
        /// <returns>Updated employee</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult<Employee> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            var existingEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Department = employee.Department;
            existingEmployee.Salary = employee.Salary;

            return Ok(existingEmployee);
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
    }

    // Employee Model Class
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}