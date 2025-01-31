using _3dlogyERP.Application.Dtos.MaterialDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _3dlogyERP.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(mapper);

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MaterialListDto> GetMaterialByIdAsync(int id)
        {
            var material = await _unitOfWork.Materials
                .Query()
                .Include(m => m.MaterialType)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<MaterialListDto>(material);
        }

        public async Task<IEnumerable<MaterialListDto>> GetAllMaterialsAsync()
        {
            var materials = await _unitOfWork.Materials
                .Query()
                .Include(m => m.MaterialType)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MaterialListDto>>(materials);
        }

        public async Task<MaterialListDto> CreateMaterialAsync(MaterialCreateDto materialDto)
        {
            ArgumentNullException.ThrowIfNull(materialDto);

            var material = _mapper.Map<Material>(materialDto);
            await _unitOfWork.Materials.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();

            return await GetMaterialByIdAsync(material.Id);
        }

        public async Task<MaterialListDto> UpdateMaterialAsync(int id, MaterialUpdateDto materialDto)
        {
            ArgumentNullException.ThrowIfNull(materialDto);

            var existingMaterial = await _unitOfWork.Materials.GetByIdAsync(id);
            if (existingMaterial == null)
                return null;

            _mapper.Map(materialDto, existingMaterial);
            await _unitOfWork.SaveChangesAsync();

            return await GetMaterialByIdAsync(id);
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

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _unitOfWork.Materials
                .Query()
                .AnyAsync(m => m.Id == id);
        }

        public async Task UpdateMaterialStockAsync(int id, int newStock)
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material != null)
            {
                material.CurrentStock = newStock;
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MaterialListDto>> GetMaterialsByStockCategoryAsync(string stockCategoryCode)
        {
            var materials = await _unitOfWork.Materials
                .Query()
                .Include(m => m.MaterialType)
                .Where(m => m.StockCategoryCode == stockCategoryCode)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MaterialListDto>>(materials);
        }
    }
}
