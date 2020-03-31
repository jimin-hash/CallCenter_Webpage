using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using HelpdeskViewModels;
using Microsoft.Extensions.Logging;

namespace CasestudyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly ILogger _logger;

        public CallController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                CallViewModel viewModel = new CallViewModel();
                List<CallViewModel> allCALLS = viewModel.GetAll();
                return Ok(allCALLS);
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        [HttpPost]
        public IActionResult Post(CallViewModel viewModel)
        {
            try
            {
                viewModel.Add();
                return viewModel.Id > 1
                    ? Ok(new { msg = "Call " + viewModel.Id + " added!" })
                    : Ok(new { msg = "Call " + viewModel.Id + " not added!" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]  CallViewModel viewModel)
        {
            try
            {
                int retVal = viewModel.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok(new { msg = "Call " + viewModel.Id + " updated!" });
                    case -1:
                        return Ok(new { msg = "Call " + viewModel.Id + " not updated!" });
                    case -2:
                        return Ok(new { msg = "Data is stale for " + viewModel.Id + ", Call not updated!" });
                    default:
                        return Ok(new { msg = "Call " + viewModel.Id + " not updated!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                CallViewModel viewModel = new CallViewModel();
                viewModel.Id = id;
                return viewModel.Delete() == 1
                    ? Ok(new { msg = "Call " + id + " deleted!" })
                    : Ok(new { msg = "Call " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }
    }
}
