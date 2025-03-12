using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IBasketService
    {
        void AddToBasket(BasketRequestDTO basket);
        void UpdateBasketAmount(int productId, int userId, int newAmount);
        void RemoveFromBasket(int productId, int userId);
    }
}
