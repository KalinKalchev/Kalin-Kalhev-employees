using System.ComponentModel.DataAnnotations;

namespace PairOfEmployeesWhoHaveWorkedTogether.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }
        public int EmpID { get; set; }
        public int ProjectID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
