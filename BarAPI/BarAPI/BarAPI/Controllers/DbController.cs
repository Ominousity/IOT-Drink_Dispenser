using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private IDbService _dbService;

        public DbController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        [Route("RebuildDatabase")]
        public ActionResult RebuildDatabase()
        {
            try
            {
                _dbService.RecreateDb();
                return Ok();
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
