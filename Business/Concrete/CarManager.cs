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

        public async Task<IResult> AddWithImageAsync(CarDetailDto carDto, string imageName)
        {
            var car = new Car
            {
                CarName = carDto.CarName,
                NumberPlate = carDto.NumberPlate,
                ModelYear = carDto.ModelYear,
                InspectionDate = carDto.InspectionDate
            };

            // Resmi sunucuya yükle
            var imageResultTask = UploadImageAsync(carDto.PermitImage);
            var imageResult = await imageResultTask;

            if (!imageResult.Success)
            {
                return imageResult; // Resim yükleme başarısız olduysa hata döndür
            }

            // Resim dosya adını Car nesnesine ekle
            car.PermitImage = imageName; // Dosya adını kullanarak ekleyin

            // Arabayı veritabanına ekle
            var addResult = await _carDal.Add(car);

            if (!addResult.Success)
            {
                return addResult; // Araba ekleme başarısız olduysa hata döndür
            }

            return new SuccessResult("Car Added with Image");
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
                InspectionDate = cp.InspectionDate,
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
            try
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

                var filePath = Path.Combine(uploadPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new SuccessDataResult<string>(file.FileName, "Resim başarıyla yüklendi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Resim yükleme sırasında bir hata oluştu: {ex.Message}");
            }
        }




    }
}