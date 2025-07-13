using System.ComponentModel.DataAnnotations;

namespace EmployeeCrudAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Department { get; set; }
        
        [Required]
        public decimal Salary { get; set; }
        
        public string Email { get; set; }
        
        public DateTime JoiningDate { get; set; }
    }
}