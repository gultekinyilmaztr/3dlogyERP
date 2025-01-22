using System.Collections.Generic;
using System.Threading.Tasks;
using _3dlogyERP.Core.Entities;

namespace _3dlogyERP.Application.Services
{
    public interface IEquipmentService
    {
        Task<Equipment> GetEquipmentByIdAsync(int id);
        Task<IEnumerable<Equipment>> GetAllEquipmentAsync();
        Task<Equipment> CreateEquipmentAsync(Equipment equipment);
        Task<Equipment> UpdateEquipmentAsync(Equipment equipment);
        Task<bool> DeleteEquipmentAsync(int id);
        Task<IEnumerable<Equipment>> GetEquipmentByTypeAsync(int equipmentTypeId);
    }
}
