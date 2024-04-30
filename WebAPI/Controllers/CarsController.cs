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
    public class CarsController : ControllerBase
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
            if (carDto.PermitImage == null || carDto.PermitImage.Length == 0)
            {
                return BadRequest("Resim seçilmedi.");
            }

            // Resim sunucuya yükle ve arabayı veritabanına ekle
            var result = await _carService.AddWithImageAsync(carDto, carDto.PermitImage.FileName);

            // imageName parametresini ImageName property'sinden alın

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
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