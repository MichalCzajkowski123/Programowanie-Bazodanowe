using BLL.DTOModels;
using BLL.ServicesInterfaces;
using DAL.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL_DB
{
    public class BasketServiceDB : IBasketService
    {
        private IDbConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NewProducts;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        public void AddToBasket(BasketRequestDTO basketDto)
        { 
            connection.Execute("EXEC dbo.AddToBasket @UserID, @ProductID, @Amount",
                new { UserID = basketDto.UserID, ProductID = basketDto.ProductID, Amount = basketDto.Amount });
        }


        public void UpdateBasketAmount(int productId, int userId, int newAmount)
        {
            connection.Execute("EXEC dbo.UpdateBasketAmount @UserID, @ProductID, @NewAmount",
                new { UserID = userId, ProductID = productId, NewAmount = newAmount });
        }

        public void RemoveFromBasket(int productId, int userId)
        {
            connection.Execute("EXEC dbo.RemoveFromBasket @UserID, @ProductID",
                new { UserID = userId, ProductID = productId });
        }
        public IEnumerable<BasketResponseDTO> GetUserBasket(int userId)
        {
            return connection.Query<BasketResponseDTO>(
                "EXEC dbo.GetUserBasket @UserID", new { UserID = userId });
        }

    }
}
