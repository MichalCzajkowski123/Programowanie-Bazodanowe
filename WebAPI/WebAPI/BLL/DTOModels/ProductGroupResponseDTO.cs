﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class ProductGroupResponseDTO
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public bool HasSubgroups { get; init; }
    }
}
