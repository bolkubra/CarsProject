using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Car : IEntity
    {
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int NumberPlate { get; set; }
        public int ModelYear { get; set; }
        public DateTime InspectionDate { get; set; }
        public string permitImage { get; set; }


    }
}
