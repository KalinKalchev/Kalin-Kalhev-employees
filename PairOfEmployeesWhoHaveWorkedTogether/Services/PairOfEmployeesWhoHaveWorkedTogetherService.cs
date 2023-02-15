using PairOfEmployeesWhoHaveWorkedTogether.Models;

namespace PairOfEmployeesWhoHaveWorkedTogether.Services
{
    public class PairOfEmployeesWhoHaveWorkedTogetherService : IPairOfEmployeesWhoHaveWorkedTogetherService
    {
        private const string CsvDelimeter = ",";
        //private const string dateFormat = "yyyy-MM-dd";
        private const string DateNowStr = "NULL";
        private readonly DateTime _baseDate = new(2000, 1, 1); // projects are started after this date

        private static IEnumerable<Employee> GetEmployees(IFormFile fileContent)
        {
            using var fileStream = fileContent.OpenReadStream();
            using var streamReader = new StreamReader(fileStream);
            string? line;
            IList<Employee> employees = new List<Employee>();

            var i = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                var employee = GetEmployee(line);
                employee.Id = i;

                employees.Add(employee);

                i++;
            }

            return employees;
        }

        private static Employee GetEmployee(string lineOfCSV)
        {
            var fieldsInRow = lineOfCSV.Split(CsvDelimeter);

            return new Employee()
            {
                EmpID = int.Parse(fieldsInRow[0]),
                ProjectID = int.Parse(fieldsInRow[1]),
                DateFrom = fieldsInRow[3].Trim() == DateNowStr ? DateTime.Now : DateTime.Parse(fieldsInRow[2].Trim()),
                DateTo = fieldsInRow[3].Trim() == DateNowStr ? DateTime.Now : DateTime.Parse(fieldsInRow[3].Trim())
            };
        }

        public ViewModel GetResult(IFormFile fileContent)
        {
            var employees = GetEmployees(fileContent);
            var employeesList = employees.ToList();

            var results = new Dictionary<string, EmployeePair>();

            for (int i = 0; i < employeesList.Count - 1; i++)
            {
                for (int j = i + 1; j < employeesList.Count; j++)
                {
                    //ProcessPairs(ref results, employeesList[i], employeesList[i]);
                    var employee1 = employeesList[i];
                    var employee2 = employeesList[j];
                    if (employee1.ProjectID == employee2.ProjectID && employee1.EmpID != employee2.EmpID)
                    {
                        int overlapDays = MinDateDays(employee1.DateTo, employee2.DateTo) - MaxDateDays(employee1.DateFrom, employee2.DateFrom);

                        if (overlapDays > 0)
                        {
                            var minEmpID = Math.Min(employee1.EmpID, employee2.EmpID);
                            var maxEmpID = Math.Max(employee1.EmpID, employee2.EmpID);
                            var pair = $"{minEmpID},{maxEmpID}";

                            if (results.ContainsKey(pair))
                            {
                                results[pair].TotalPeriod += overlapDays;
                                results[pair].EmployeePairDetailed!.Add(new EmployeePairDetailed
                                {
                                    Period = overlapDays,
                                    ProjectId = employee2.ProjectID,
                                });
                            }
                            else
                            {
                                var employeePairDetailed = new EmployeePairDetailed
                                {
                                    Period = overlapDays,
                                    ProjectId = employee1.ProjectID,
                                };
                                results.Add(pair, new EmployeePair
                                {
                                    TotalPeriod = overlapDays,
                                    EmployeePairDetailed = new List<EmployeePairDetailed> { employeePairDetailed },
                                });
                            }
                        }
                    }
                }
            }

            var max = 0;
            var employeePairStr = string.Empty;
            foreach (KeyValuePair<string, EmployeePair> pair in results)
            {
                if (pair.Value.TotalPeriod > max)
                {
                    max = pair.Value.TotalPeriod;
                    employeePairStr = pair.Key;
                }
            }

            var employeePairDetailedViewModel = results.FirstOrDefault(x => x.Key == employeePairStr).Value.EmployeePairDetailed;
            return new ViewModel
            {
                ShortResult = $"{employeePairStr},{results.First(x => x.Key == employeePairStr).Value.TotalPeriod}",
                EmployeePairDetailed = employeePairDetailedViewModel?.Select(x => new EmployeePairDetailedViewModel
                {
                    Employee1Id = int.Parse(employeePairStr.Split(",")[0]),
                    Employee2Id = int.Parse(employeePairStr.Split(",")[1]),
                    Period = x.Period,
                    ProjectId = x.ProjectId
                })
                .ToList()
            };
        }
        //private void ProcessPairs(ref Dictionary<string, EmployeePair> results, Employee employee1, Employee employee2)
        //{
        //    if (employee1.ProjectID == employee2.ProjectID && employee1.EmpID != employee2.EmpID)
        //    {
        //        int ovelapDays = MinDateDays(employee1.DateTo, employee2.DateTo) - MaxDateDays(employee1.DateFrom, employee2.DateFrom);

        //        if (ovelapDays > 0)
        //        {
        //            var minEmpID = Math.Min(employee1.EmpID, employee2.EmpID);
        //            var maxEmpID = Math.Max(employee1.EmpID, employee2.EmpID);

        //            var pair = $"{minEmpID},{maxEmpID}";
        //            if (results.ContainsKey(pair))
        //            {
        //                results[pair].TotalPeriod += ovelapDays;
        //                results[pair].EmployeePairDetailed!.Add(new EmployeePairDetailed
        //                {
        //                    Period = ovelapDays,
        //                    ProjectId = employee2.ProjectID,
        //                });
        //            }
        //            else
        //            {
        //                var employeePairDetailed = new EmployeePairDetailed
        //                {
        //                    Period = ovelapDays,
        //                    ProjectId = employee1.ProjectID,
        //                };
        //                results.Add(pair, new EmployeePair
        //                {
        //                    TotalPeriod = ovelapDays,
        //                    EmployeePairDetailed = new List<EmployeePairDetailed> { employeePairDetailed },
        //                });
        //            }
        //        }
        //    }
        //}

        private int MinDateDays(DateTime date1, DateTime date2)
        {
            return Math.Min(date1.Subtract(_baseDate).Days, date2.Subtract(_baseDate).Days);
        }

        private int MaxDateDays(DateTime date1, DateTime date2)
        {
            return Math.Max(date1.Subtract(_baseDate).Days, date2.Subtract(_baseDate).Days);
        }
    }
}
