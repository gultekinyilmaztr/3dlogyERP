using _3dlogyERP.Application.Dtos.MaterialDtos;
using _3dlogyERP.Application.DTOs.StockCategoryDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dlogyERP.Application.Services
{
    public class StockCategoryService : IStockCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StockCategoryListDto>> GetAllStockCategoriesAsync()
        {
            var categories = await _unitOfWork.StockCategories.GetAllAsync();
            return _mapper.Map<IEnumerable<StockCategoryListDto>>(categories);
        }
    }
}
