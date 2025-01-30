using _3dlogyERP.Application.Dtos.MaterialTypeDtos;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IMaterialTypeService
    {
        Task<MaterialTypeListDto> GetMaterialTypeByIdAsync(int id);
        Task<IEnumerable<MaterialTypeListDto>> GetAllMaterialTypesAsync();
        Task<MaterialTypeListDto> CreateMaterialTypeAsync(MaterialTypeCreateDto materialTypeDto);
        Task<MaterialTypeListDto> UpdateMaterialTypeAsync(int id, MaterialTypeUpdateDto materialTypeDto);
        Task<bool> DeleteMaterialTypeAsync(int id);
        Task<IEnumerable<MaterialTypeListDto>> GetMaterialTypesByCategoryAsync(int categoryId);
        Task<bool> ExistsByIdAsync(int id);
    }
}
