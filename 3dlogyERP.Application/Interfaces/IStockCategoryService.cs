using _3dlogyERP.Application.Dtos.MaterialDtos;
using _3dlogyERP.Application.DTOs.StockCategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IStockCategoryService
    {
        Task<IEnumerable<StockCategoryListDto>> GetAllStockCategoriesAsync();
    }
}
