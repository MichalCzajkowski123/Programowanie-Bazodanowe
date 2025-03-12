using BLL.DTOModels.BLL.DTOModels;
using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IOrderService
    {
        void GenerateOrder(int userId);
        void PayOrder(int orderId, double amount);
        IEnumerable<OrderResponseDTO> GetOrders(bool? paidStatus = null, int? orderId = null, string? sortBy = "Date", bool descending = false);
        IEnumerable<OrderDetailsResponseDTO> GetOrderDetails(int orderId);
    }
}
