using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _3dlogyERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialType>>> GetAll()
        {
            var materialTypes = await _unitOfWork.MaterialTypes.GetAllAsync();
            return Ok(materialTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialType>> Get(int id)
        {
            var materialType = await _unitOfWork.MaterialTypes.GetByIdAsync(id);
            if (materialType == null)
                return NotFound();

            return Ok(materialType);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialType>> Create(MaterialType materialType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.MaterialTypes.AddAsync(materialType);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = materialType.Id }, materialType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MaterialType materialType)
        {
            if (id != materialType.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingMaterialType = await _unitOfWork.MaterialTypes.GetByIdAsync(id);
            if (existingMaterialType == null)
                return NotFound();

            _unitOfWork.MaterialTypes.Update(materialType);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var materialType = await _unitOfWork.MaterialTypes.GetByIdAsync(id);
            if (materialType == null)
                return NotFound();

            _unitOfWork.MaterialTypes.Remove(materialType);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
