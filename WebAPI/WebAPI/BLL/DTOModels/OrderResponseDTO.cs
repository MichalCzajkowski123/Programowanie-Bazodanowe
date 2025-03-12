using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    namespace BLL.DTOModels
    {
        public class OrderResponseDTO
        {
            public int ID { get; init; }
            public double Value { get; init; }
            public bool IsPaid { get; init; }
            public DateTime Date { get; init; }
        }
    }
}
