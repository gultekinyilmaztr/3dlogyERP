using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Application.Services;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _3dlogyERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockCategoryController : Controller
    {
        private readonly IStockCategoryService _stockCategoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockCategoryController(IStockCategoryService stockCategoryService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _stockCategoryService = stockCategoryService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<StockCategory>> GetAll()
        {
            var materials = await _stockCategoryService.GetAllStockCategoriesAsync();
            return Ok(materials);
        }
    }
}
