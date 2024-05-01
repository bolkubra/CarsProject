using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class CarDetailDto : IDto
    {
        public int CarId { get; set; }
        public string CarName { get; set; }
        public string NumberPlate { get; set; }
        public int ModelYear { get; set; }
        public DateTime InspectionDate { get; set; }
        public IFormFile PermitImage { get; set; }
        public string ImageName { get; set; }




    }
}
