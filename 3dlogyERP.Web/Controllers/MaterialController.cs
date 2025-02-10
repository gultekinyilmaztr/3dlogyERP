using _3dlogyERP.Application.Dtos.MaterialDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _3dlogyERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialController(IMaterialService materialService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _materialService = materialService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetAll()
        {
            var materials = await _materialService.GetAllMaterialsAsync();
            return Ok(materials);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> Get(int id)
        {
            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null)
                return NotFound();

            return Ok(material);
        }

        [HttpPost]
        public async Task<ActionResult<Material>> Create(Material material)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // StockCategory kontrolü
            if (material.StockCategoryId.HasValue)
            {
                var stockCategory = await _unitOfWork.StockCategories.GetByIdAsync(material.StockCategoryId.Value);
                if (stockCategory == null)
                    return BadRequest($"StockCategory with id {material.StockCategoryId} not found.");
            }

            var materialDto = _mapper.Map<MaterialCreateDto>(material);
            var createdMaterial = await _materialService.CreateMaterialAsync(materialDto);
            return CreatedAtAction(nameof(Get), new { id = createdMaterial.Id }, createdMaterial);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Material material)
        {
            if (id != material.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // StockCategory kontrolü
            if (material.StockCategoryId.HasValue)
            {
                var stockCategory = await _unitOfWork.StockCategories.GetByIdAsync(material.StockCategoryId.Value);
                if (stockCategory == null)
                    return BadRequest($"StockCategory with id {material.StockCategoryId} not found.");
            }

            var materialDto = _mapper.Map<MaterialUpdateDto>(material);
            var updatedMaterial = await _materialService.UpdateMaterialAsync(id, materialDto);
            if (updatedMaterial == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _materialService.DeleteMaterialAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/transaction")]
        public async Task<IActionResult> AddTransaction(int id, MaterialTransaction transaction)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != transaction.MaterialId)
                return BadRequest("Material ID mismatch");

            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null)
                return NotFound();

            // Stok miktarını güncelle
            decimal newStock = material.CurrentStock;
            switch (transaction.Type)
            {
                case TransactionType.StockIn:
                    newStock += transaction.Quantity;
                    break;
                case TransactionType.StockOut:
                    if (material.CurrentStock < transaction.Quantity)
                        return BadRequest("Yetersiz stok");
                    newStock -= transaction.Quantity;
                    break;
                case TransactionType.Return:
                    newStock += transaction.Quantity;
                    break;
                case TransactionType.Adjustment:
                    newStock = transaction.Quantity;
                    break;
            }

            // İşlem kaydını oluştur
            transaction.TransactionDate = DateTime.UtcNow;
            transaction.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.MaterialTransactions.AddAsync(transaction);

            // Stok miktarını güncelle
            await _materialService.UpdateMaterialStockAsync(id, (int)newStock);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "İşlem başarıyla kaydedildi", currentStock = newStock });
        }

        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<MaterialTransaction>>> GetTransactions(int id)
        {
            var transactions = await _unitOfWork.MaterialTransactions.FindAsync(t => t.MaterialId == id);
            return Ok(transactions);
        }

        [HttpGet("ByStockCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<MaterialListDto>>> GetByStockCategory(int categoryId)
        {
            var materials = await _materialService.GetMaterialsByStockCategoryAsync(categoryId);
            return Ok(materials); // NotFound kontrolünü kaldırıyoruz, boş liste dönebilir
        }

    }
}
