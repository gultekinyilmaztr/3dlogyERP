using _3dlogyERP.Application.Dtos.MaterialDtos;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialListDto> GetMaterialByIdAsync(int id);
        Task<IEnumerable<MaterialListDto>> GetAllMaterialsAsync();
        Task<MaterialListDto> CreateMaterialAsync(MaterialCreateDto materialDto);
        Task<MaterialListDto> UpdateMaterialAsync(int id, MaterialUpdateDto materialDto);
        Task<bool> DeleteMaterialAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task UpdateMaterialStockAsync(int id, int newStock);
        Task<IEnumerable<MaterialListDto>> GetMaterialsByStockCategoryAsync(int stockCategoryId);
    }
}
