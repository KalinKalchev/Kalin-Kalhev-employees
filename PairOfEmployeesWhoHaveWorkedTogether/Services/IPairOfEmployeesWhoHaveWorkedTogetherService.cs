using PairOfEmployeesWhoHaveWorkedTogether.Models;

namespace PairOfEmployeesWhoHaveWorkedTogether.Services
{
    public interface IPairOfEmployeesWhoHaveWorkedTogetherService
    {
        public ViewModel GetResult(IFormFile fileContent);
    }
}
