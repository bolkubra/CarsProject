using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
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
                                 PermitImage = car.PermitImage
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




    }
}
