using _3dlogyERP.Application.Dtos.EquipmentDtos;

namespace _3dlogyERP.Application.Interfaces
{
    public interface IEquipmentService
    {
        Task<EquipmentListDto> GetEquipmentByIdAsync(int id);
        Task<IEnumerable<EquipmentListDto>> GetAllEquipmentAsync();
        Task<EquipmentListDto> CreateEquipmentAsync(EquipmentCreateDto equipmentDto);
        Task<EquipmentListDto> UpdateEquipmentAsync(int id, EquipmentUpdateDto equipmentDto);
        Task<bool> DeleteEquipmentAsync(int id);
        Task<IEnumerable<EquipmentListDto>> GetEquipmentByTypeAsync(int equipmentTypeId);
        Task<bool> ExistsByIdAsync(int id);
    }
}
