using System.ComponentModel.DataAnnotations;

namespace PairOfEmployeesWhoHaveWorkedTogether.Models
{
    public class File
    {
        [Required]
        public IFormFile? FileContent { get; set; }
    }
}
