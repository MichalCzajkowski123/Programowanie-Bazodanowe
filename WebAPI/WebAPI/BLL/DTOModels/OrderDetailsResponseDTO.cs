﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class OrderDetailsResponseDTO
    {
        public string ProductName { get; init; }
        public double Price { get; init; }
        public int Amount { get; init; }
        public double Total { get; init; }
    }
}
