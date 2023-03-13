using Application.DTOs;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesterController : ControllerBase
    {
        private IDrinkService _drinkService;

        public TesterController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        [HttpPost]
        [Route("test")]
        public ActionResult TestDespenseDrink(DrinkDTO drinkDTO) {

            try
            {
                _drinkService.DespenseDrink(drinkDTO);
                return Ok();
            } catch(ValidationException e)
            {
                return StatusCode(422, e.Message);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
