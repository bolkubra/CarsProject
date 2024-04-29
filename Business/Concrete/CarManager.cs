using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
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
    }
}
