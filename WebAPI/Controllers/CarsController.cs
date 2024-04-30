using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using DataAccess.Concrete.EntityFremework;
using Entities.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController  : ControllerBase
    {

        private readonly ICarService _carService;
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }
        

        [HttpGet]
        public IActionResult Get()
        {
            var result = _carService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("insert")]
public async Task<IActionResult> Insert([FromForm] CarDetailDto carDto)
{
    var file = carDto.PermitImage;

    var car = new Car
    {
        CarName = carDto.CarName,
        NumberPlate = carDto.NumberPlate,
        ModelYear = carDto.ModelYear,
        InspectionDate = carDto.InspectionDate
    };

    var result = await _carService.AddWithImageAsync(car, file);
    if (result.Success)
    {
        return Ok(result);
    }
    return BadRequest(result);
}




        [HttpPost("delete")]
        public IActionResult Delete(int carId)
        {
            var result = _carService.Delete(carId);
            if (result.Success)
            {
                return Ok(result);

            }
            return BadRequest(result);
        }
        [HttpPost("update")]
        public IActionResult Update(Car car)
        {
            var result = _carService.Updated(car);
            if (result.Success)
            {
                return Ok(result);

            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _carService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

       

    }
}

