using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IProductService
    {
        IEnumerable<ProductResponseDTO> GetProducts(
            string? nameFilter = null, string? groupNameFilter = null, int? groupIdFilter = null,
            bool includeInactive = false, string? sortBy = "Name", bool descending = false);

        void AddProduct(ProductRequestDTO product);
        void DeactivateProduct(int productId);
        void ActivateProduct(int productId);
        void DeleteProduct(int productId);
    }
}
