using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;

namespace _3dlogyERP.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialService(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public async Task<Material> GetMaterialByIdAsync(int id)
        {
            return await _unitOfWork.Materials.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Material>> GetAllMaterialsAsync()
        {
            return await _unitOfWork.Materials.GetAllAsync();
        }

        public async Task<Material> CreateMaterialAsync(Material material)
        {
            ArgumentNullException.ThrowIfNull(material);

            await _unitOfWork.Materials.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();
            return material;
        }

        public async Task<Material> UpdateMaterialAsync(Material material)
        {
            ArgumentNullException.ThrowIfNull(material);

            var existingMaterial = await _unitOfWork.Materials.GetByIdAsync(material.Id);
            if (existingMaterial == null)
                return null;

            existingMaterial.Name = material.Name;
            existingMaterial.Brand = material.Brand;
            existingMaterial.MaterialTypeId = material.MaterialTypeId;
            existingMaterial.Color = material.Color;
            existingMaterial.UnitCost = material.UnitCost;
            existingMaterial.CurrentStock = material.CurrentStock;
            existingMaterial.MinimumStock = material.MinimumStock;
            existingMaterial.ReorderPoint = material.ReorderPoint;
            existingMaterial.SKU = material.SKU;
            existingMaterial.BatchNumber = material.BatchNumber;
            existingMaterial.WeightPerUnit = material.WeightPerUnit;
            existingMaterial.Location = material.Location;
            existingMaterial.Specifications = material.Specifications;
            existingMaterial.IsActive = material.IsActive;

            _unitOfWork.Materials.Update(existingMaterial);
            await _unitOfWork.SaveChangesAsync();

            return existingMaterial;
        }

        public async Task<bool> DeleteMaterialAsync(int id)
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material == null)
                return false;

            _unitOfWork.Materials.Remove(material);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Material>> GetMaterialsByTypeAsync(int materialTypeId)
        {
            return await _unitOfWork.Materials.FindAsync(m => m.MaterialTypeId == materialTypeId);
        }

        public async Task<bool> UpdateMaterialStockAsync(int id, int quantity)
        {
            ArgumentNullException.ThrowIfNull(quantity);

            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material == null)
                return false;

            material.CurrentStock = quantity;
            _unitOfWork.Materials.Update(material);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
