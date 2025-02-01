﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Network.Dto
{
    public class OfferDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
