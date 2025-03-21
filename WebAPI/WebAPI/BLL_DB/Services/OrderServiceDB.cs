using BLL.DTOModels;
using BLL.DTOModels.BLL.DTOModels;
using BLL.ServicesInterfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_DB.Services
{
    public class OrderServiceDB : IOrderService
    {
        private IDbConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NewProducts;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        public void GenerateOrder(int userId)
        {
            connection.Execute("EXEC dbo.GenerateOrder @UserID", new { UserID = userId });
        }
        public IEnumerable<OrderDetailsResponseDTO> GetOrderDetails(int orderId)
        {
            return connection.Query<OrderDetailsResponseDTO>(
                "EXEC dbo.GetOrderDetails @OrderID",
                new { OrderID = orderId });
        }


        public IEnumerable<OrderResponseDTO> GetOrders(bool? paidStatus = null, int? orderId = null, string? sortBy = "Date", bool descending = false)
        {
            return connection.Query<OrderResponseDTO>(
                "EXEC dbo.GetOrders @PaidStatus, @OrderID, @SortBy, @Descending",
                new { PaidStatus = paidStatus, OrderID = orderId, SortBy = sortBy, Descending = descending ? 1 : 0 });
        }

        public void PayOrder(int orderId, double amount)
        {
            connection.Execute("EXEC dbo.PayOrder @OrderID, @Amount",
                new { OrderID = orderId, Amount = amount });
        }

    }
}
