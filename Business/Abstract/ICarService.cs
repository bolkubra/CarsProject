﻿using Core.Utilities.Result;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICarService
    {
        IDataResult<List<Car>> GetAll();
        IResult Add(Car car);
        IResult Delete(int carId);
        IResult Updated(Car car);
        IDataResult<Car> GetById(int carId);
        IDataResult<List<CarDetailDto>> GetCarDetails(int carId);

        Task<IResult> UploadImageAsync(IFormFile file);
        Task<IResult> AddWithImageAsync(CarDetailDto carDto, string imageName);
    }
}
