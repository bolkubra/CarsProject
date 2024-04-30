using Core.DataAccess.EntityFramework;
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

namespace DataAccess.Concrete.EntityFremework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, CarProjectContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails(int carId)
        {
            using (CarProjectContext context = new CarProjectContext())
            {
                var result = from car in context.Cars
                             select new CarDetailDto
                             {
                                 CarId = car.CarId,
                                 CarName = car.CarName,
                                 NumberPlate = car.NumberPlate,
                                 ModelYear = car.ModelYear,
                                 InspectionDate = car.InspectionDate,
                                
                             };

                return result.ToList();
            }
        }

        public Car GetById(int carId)
        {
            using (CarProjectContext context = new CarProjectContext())
            {
                return context.Cars.FirstOrDefault(c => c.CarId == carId);
            }
        }

         public async Task<IResult> AddWithImageAsync(Car car, IFormFile file)
         {
             if (file != null && file.Length > 0)
             {
                 using (var memoryStream = new MemoryStream())
                 {
                     await file.CopyToAsync(memoryStream);
                     var bytes = memoryStream.ToArray();
                     car.PermitImage = Convert.ToBase64String(bytes);
                 }
             }

             Add(car);
             return new SuccessResult("Car Added with Image");
         }

       

    }
}
