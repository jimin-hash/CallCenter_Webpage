using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using CasestudyWebsite.Reports;

namespace CasestudyWebsite.Controllers
{
    public class ReportController : ControllerBase
    {
        private IHostingEnvironment _env;

        public ReportController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("api/employeereport")]
        public IActionResult GetEmployeeReport()
        {
            EmployeeReport report = new EmployeeReport();
            report.generatedReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }

        [Route("api/callreport")]
        public IActionResult GetCallReport()
        {
            CallReport report = new CallReport();
            report.generatedReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }
    }
}