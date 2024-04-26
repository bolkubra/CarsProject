﻿using Core.Entities.Concrete;
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
        public int NumberPlate { get; set; }
        public int ModelYear { get; set; }
        public DateTime InspectionDate { get; set; }
        public string PermitImage { get; set; }
    }
}
