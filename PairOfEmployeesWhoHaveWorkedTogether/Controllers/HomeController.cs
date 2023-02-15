using Microsoft.AspNetCore.Mvc;
using PairOfEmployeesWhoHaveWorkedTogether.Services;

namespace PairOfEmployeesWhoHaveWorkedTogether.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IPairOfEmployeesWhoHaveWorkedTogetherService _pairOfEmployeesWhoHaveWorkedTogetherService;

        public HomeController(IPairOfEmployeesWhoHaveWorkedTogetherService pairOfEmployeesWhoHaveWorkedTogetherService)
        {
            _pairOfEmployeesWhoHaveWorkedTogetherService = pairOfEmployeesWhoHaveWorkedTogetherService;
        }

        [HttpPost("pairOfEmployeesWhoHaveWorkedTogether")]
        public IActionResult GetPairOfEmployeesWhoHaveWorkedTogether([FromForm] Models.File file)
        {
            if (file.FileContent != null && file.FileContent.Length == 0)
            {
                return NoContent();
            }

            var result = _pairOfEmployeesWhoHaveWorkedTogetherService.GetResult(file.FileContent!);

            return Ok(result);
        }
    }
}