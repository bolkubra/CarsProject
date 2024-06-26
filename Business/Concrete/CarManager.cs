﻿using Business.Abstract;
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
            return new SuccessResult("Araç Bilgisi Eklendi");
        }

        public async Task<IResult> AddWithImageAsync(CarDetailDto carDto, string imageName)
        {
            var car = new Car
            {
                CarName = carDto.CarName,
                NumberPlate = carDto.NumberPlate,
                ModelYear = carDto.ModelYear,
                InspectionDate = carDto.InspectionDate
            };

            // Resmi sunucuya yükleme işlemi
            var imageResultTask = UploadImageAsync(carDto.PermitImage);
            var imageResult = await imageResultTask;

            if (!imageResult.Success)
            {
                return imageResult; 
            }

            // Resim dosya adını Car nesnesine ekle
            car.PermitImage = imageName; // Dosya adını kullanarak ekleme

            // Arabayı veritabanına ekle
            var addResult = await _carDal.Add(car);

            if (!addResult.Success)
            {
                return addResult; 
            }

            return new SuccessResult("Araba Resim ile Eklendi");
        }

        public IResult Delete(int carId)
        {
            var carToDelete = _carDal.Get(c => c.CarId == carId);
            if (carToDelete == null)
            {
                return new ErrorResult("Araç Bulunamadı");
            }

            _carDal.Delete(carToDelete);
            return new SuccessResult("Araç Silindi");
        }

        public IDataResult<List<Car>> GetAll()
        {
            return new DataResult<List<Car>>(_carDal.GetAll(), true, "Araba Bilgileri Listelendi");
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
            return new SuccessResult("Araç Güncellendi");
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