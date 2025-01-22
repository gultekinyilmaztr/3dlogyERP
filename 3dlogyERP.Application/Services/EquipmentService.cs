using System.Collections.Generic;
using System.Threading.Tasks;
using _3dlogyERP.Core.Entities;
using _3dlogyERP.Core.Interfaces;

namespace _3dlogyERP.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EquipmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Equipment> GetEquipmentByIdAsync(int id)
        {
            return await _unitOfWork.Equipment.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
        {
            return await _unitOfWork.Equipment.GetAllAsync();
        }

        public async Task<Equipment> CreateEquipmentAsync(Equipment equipment)
        {
            await _unitOfWork.Equipment.AddAsync(equipment);
            await _unitOfWork.SaveChangesAsync();
            return equipment;
        }

        public async Task<Equipment> UpdateEquipmentAsync(Equipment equipment)
        {
            var existingEquipment = await _unitOfWork.Equipment.GetByIdAsync(equipment.Id);
            if (existingEquipment == null)
                return null;

            existingEquipment.Name = equipment.Name;
            existingEquipment.Model = equipment.Model;
            existingEquipment.SerialNumber = equipment.SerialNumber;
            existingEquipment.EquipmentTypeId = equipment.EquipmentTypeId;
            existingEquipment.PurchaseDate = equipment.PurchaseDate;
            existingEquipment.PurchasePrice = equipment.PurchasePrice;
            existingEquipment.HourlyRate = equipment.HourlyRate;
            existingEquipment.MaintenanceCostPerHour = equipment.MaintenanceCostPerHour;
            existingEquipment.ElectricityConsumptionPerHour = equipment.ElectricityConsumptionPerHour;
            existingEquipment.IsActive = equipment.IsActive;
            existingEquipment.LastMaintenanceDate = equipment.LastMaintenanceDate;
            existingEquipment.NextMaintenanceDate = equipment.NextMaintenanceDate;

            _unitOfWork.Equipment.Update(existingEquipment);
            await _unitOfWork.SaveChangesAsync();

            return existingEquipment;
        }

        public async Task<bool> DeleteEquipmentAsync(int id)
        {
            var equipment = await _unitOfWork.Equipment.GetByIdAsync(id);
            if (equipment == null)
                return false;

            _unitOfWork.Equipment.Remove(equipment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Equipment>> GetEquipmentByTypeAsync(int equipmentTypeId)
        {
            return await _unitOfWork.Equipment.FindAsync(e => e.EquipmentTypeId == equipmentTypeId);
        }
    }
}
