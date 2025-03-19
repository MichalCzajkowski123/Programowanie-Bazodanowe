using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class BasketResponseDTO
    {
        public int ProductID { get; init; }
        public string ProductName { get; init; }
        public int Amount { get; init; }
    }
}
