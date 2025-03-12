using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IProductGroupService
    {
        IEnumerable<ProductGroupResponseDTO> GetProductGroups(int? parentGroupId = null, string? sortBy = "Name", bool descending = false);
        void AddProductGroup(ProductGroupRequestDTO group);
    }
}
