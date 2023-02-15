namespace PairOfEmployeesWhoHaveWorkedTogether.Models
{
    public class EmployeePair
    {
        public int TotalPeriod { get; set; }
        public IList<EmployeePairDetailed>? EmployeePairDetailed { get; set; }
    }
}
