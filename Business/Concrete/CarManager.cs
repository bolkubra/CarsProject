using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;


        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        public IResult Add(Car car)
        {
            _carDal.Add(car);
            return new SuccessResult("Cars Added");
        }

        /*public async Task<IResult> AddWithImageAsync(Car car, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();
                    // Bayt dizisini Base64'e kodla
                    car.PermitImage = Convert.ToBase64String(bytes);
                }
            }

            // Dosyanın adını al
            var fileName = Path.GetFileName(file.FileName);

            // Car nesnesine dosya adını atayın
            car.PermitImage = fileName;

            // Veritabanına kaydetme işlemi
            _carDal.Add(car);
            return new SuccessResult("Car Added with Image");
        }*/

        public async Task<IResult> AddWithImageAsync(CarDetailDto carDto)
        {
            var car = new Car
            {
                CarName = carDto.CarName,
                NumberPlate = carDto.NumberPlate,
                ModelYear = carDto.ModelYear,
                InspectionDate = carDto.InspectionDate
            };

            // Resmi sunucuya yükle
            var imageResult = await UploadImageAsync(carDto.PermitImage);

            if (!imageResult.Success)
            {
                return imageResult; // Resim yükleme başarısız olduysa hata döndür
            }

            // Arabayı veritabanına ekle
            return await _carDal.AddWithImageAsync(car, carDto.PermitImage);
        }



        public IResult Delete(int carId)
        {
            var carToDelete = _carDal.Get(c => c.CarId == carId);
            if (carToDelete == null)
            {
                return new ErrorResult("araç bulunamadı");
            }

            _carDal.Delete(carToDelete);
            return new SuccessResult("araç silindi");
        }

        public IDataResult<List<Car>> GetAll()
        {
            return new DataResult<List<Car>>(_carDal.GetAll(), true, "Cars Listed");
        }

        public IDataResult<Car> GetById(int carId)
        {
            return new SuccessDataResult<Car>(_carDal.Get(c => c.CarId == carId));
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails(int carId)
        {
            var car = _carDal.GetCarDetails(carId); 
            var carDetails = car.Select(cp => new CarDetailDto
            {
               CarId = cp.CarId,
               CarName = cp.CarName,
               NumberPlate = cp.NumberPlate,
               ModelYear = cp.ModelYear,
               InspectionDate =   cp.InspectionDate,
               PermitImage = cp.PermitImage,


            }).ToList();

            return new SuccessDataResult<List<CarDetailDto>>(carDetails, "Araçlar listelendi.");
        }

        

        public IResult Updated(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult("Cars Added");
        }

        public async Task<IResult> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new ErrorResult("Dosya boş veya seçilmedi.");
            }

          
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Images");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new SuccessDataResult<string>(fileName, "Resim başarıyla yüklendi.");
        }
    }
}

