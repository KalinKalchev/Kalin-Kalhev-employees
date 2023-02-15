namespace PairOfEmployeesWhoHaveWorkedTogether.Models
{
    public class ViewModel
    {
        public string? ShortResult { get; set; }
        public IList<EmployeePairDetailedViewModel>? EmployeePairDetailed { get; set; }
    }
}
