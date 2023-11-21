using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;

namespace my_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticController : ControllerBase
    {
        public static int _noOfCreation = 0;
        public static int _noOfUpdate = 0;
        public static int _noOfView = 0;
        public static int _noOfDelete = 0;

        #region Statistic service
        // GET api/<BookController>
        [HttpGet("view")]
        public ActionResult<ApiResponse<StatisticDto>> Statistic(int id)
        {
            return Ok(ApiResponse<StatisticDto>.Ok(new StatisticDto
            {
                NoOfCreation = _noOfCreation,
                NoOfUpdate = _noOfUpdate,
                NoOfView = _noOfView,
                NoOfDelete = _noOfDelete
            }));
        }
        #endregion
    }
}
