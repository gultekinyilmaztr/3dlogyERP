using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Application.Services
{
    public interface IMaterialService
    {
        Task<Material> GetMaterialByIdAsync(int id);
        Task<IEnumerable<Material>> GetAllMaterialsAsync();
        Task<Material> CreateMaterialAsync(Material material);
        Task<Material> UpdateMaterialAsync(Material material);
        Task<bool> DeleteMaterialAsync(int id);
        Task<IEnumerable<Material>> GetMaterialsByTypeAsync(int materialTypeId);
        Task<bool> UpdateMaterialStockAsync(int id, int quantity);
    }
}
