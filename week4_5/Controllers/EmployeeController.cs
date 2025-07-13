using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fifth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // This entire controller requires authentication
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEmployees()
        {
            // Get current user details from JWT token
            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            var employees = new[]
            {
                new { Id = 1, Name = "John Doe", Department = "IT", Salary = 50000 },
                new { Id = 2, Name = "Jane Smith", Department = "HR", Salary = 45000 },
                new { Id = 3, Name = "Bob Johnson", Department = "Finance", Salary = 55000 },
                new { Id = 4, Name = "Alice Brown", Department = "Marketing", Salary = 48000 }
            };

            return Ok(new
            {
                Message = "Employees data fetched successfully!",
                RequestedBy = new
                {
                    UserId = userId,
                    UserRole = userRole,
                    UserName = userName
                },
                Data = employees,
                TotalCount = employees.Length
            });
        }

        [HttpGet]
        [Route("AdminOnly")]
        [Authorize(Roles = "Admin")] // Only Admin role can access
        public IActionResult GetAdminData()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var adminData = new
            {
                SalaryBudget = 2500000,
                EmployeeCount = 150,
                DepartmentBudgets = new[]
                {
                    new { Department = "IT", Budget = 800000 },
                    new { Department = "HR", Budget = 300000 },
                    new { Department = "Finance", Budget = 500000 },
                    new { Department = "Marketing", Budget = 400000 }
                }
            };

            return Ok(new
            {
                Message = "This is admin-only data!",
                AccessedBy = new
                {
                    UserId = userId,
                    UserRole = userRole
                },
                AdminData = adminData
            });
        }

        [HttpGet]
        [Route("MultipleRoles")]
        [Authorize(Roles = "Admin,POC")] // Both Admin and POC can access
        public IActionResult GetMultiRoleData()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var multiRoleData = new
            {
                ProjectStatus = new[]
                {
                    new { Project = "Website Redesign", Status = "In Progress", Progress = 75 },
                    new { Project = "Mobile App", Status = "Testing", Progress = 90 },
                    new { Project = "Database Migration", Status = "Planning", Progress = 25 }
                },
                TeamMembers = new[]
                {
                    new { Name = "John Doe", Role = "Developer" },
                    new { Name = "Jane Smith", Role = "Designer" },
                    new { Name = "Bob Johnson", Role = "Tester" }
                }
            };

            return Ok(new
            {
                Message = "Admin or POC can access this data!",
                AccessedBy = new
                {
                    UserId = userId,
                    UserRole = userRole
                },
                ProjectData = multiRoleData
            });
        }

        [HttpGet]
        [Route("POCOnly")]
        [Authorize(Roles = "POC")] // Only POC role can access
        public IActionResult GetPOCData()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "This is POC-only data!",
                AccessedBy = new
                {
                    UserId = userId,
                    UserRole = userRole
                },
                POCData = new
                {
                    TasksAssigned = 15,
                    TasksCompleted = 12,
                    PendingReviews = 3
                }
            });
        }

        [HttpPost]
        [Route("AddEmployee")]
        [Authorize(Roles = "Admin")] // Only Admin can add employees
        public IActionResult AddEmployee([FromBody] object employeeData)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "Employee added successfully!",
                AddedBy = new
                {
                    UserId = userId,
                    UserRole = userRole
                },
                NewEmployeeId = new Random().Next(100, 999)
            });
        }
    }
}