using BLL.DTOModels;
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
    public class ProductServiceDB : IProductService
    {
        private IDbConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NewProducts;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        public void ActivateProduct(int productId)
        {
            connection.Execute("EXEC dbo.ActivateProduct @ProductID", new { ProductID = productId });
        }

        public void AddProduct(ProductRequestDTO product)
        {
            connection.Execute("EXEC dbo.AddProduct @Name, @Price, @GroupID",
                new { Name = product.Name, Price = product.Price, GroupID = product.GroupID });
        }
        public void DeactivateProduct(int productId)
        {
            connection.Execute("EXEC dbo.DeactivateProduct @ProductID", new { ProductID = productId });
        }
        public void DeleteProduct(int productId)
        {
            connection.Execute("EXEC dbo.DeleteProduct @ProductID", new { ProductID = productId });
        }

        public List<int> GetAllGroupIds(int groupId)
        {
            return connection.Query<int>("EXEC dbo.GetAllGroupIds @GroupID", new { GroupID = groupId }).ToList();
        }

        public string GetFullGroupName(int groupId)
        {
            return connection.QuerySingleOrDefault<string>(
                "EXEC dbo.GetFullGroupName @GroupID",
                new { GroupID = groupId }) ?? "Unknown";
        }


        public IEnumerable<ProductResponseDTO> GetProducts(
    string? nameFilter = null, string? groupNameFilter = null, int? groupIdFilter = null,
    bool includeInactive = false, string? sortBy = "Name", bool descending = false)
        {
            var sql = "SELECT * FROM dbo.GetProducts WHERE 1=1";

            if (!string.IsNullOrEmpty(nameFilter))
                sql += " AND Name LIKE '%' + @NameFilter + '%'";

            if (!string.IsNullOrEmpty(groupNameFilter))
                sql += " AND GroupName LIKE '%' + @GroupNameFilter + '%'";

            if (groupIdFilter.HasValue)
                sql += " AND GroupID = @GroupID";

            if (!includeInactive)
                sql += " AND IsActive = 1";

            sql += sortBy?.ToLower() switch
            {
                "id" => " ORDER BY ID",
                "name" => " ORDER BY Name",
                "price" => " ORDER BY Price",
                "groupname" => " ORDER BY GroupName",
                _ => " ORDER BY Name"
            };

            if (descending)
                sql += " DESC";
            else
                sql += " ASC";

            return connection.Query<ProductResponseDTO>(sql, new
            {
                NameFilter = nameFilter,
                GroupNameFilter = groupNameFilter,
                GroupID = groupIdFilter
            }).ToList();
        }


    }
}