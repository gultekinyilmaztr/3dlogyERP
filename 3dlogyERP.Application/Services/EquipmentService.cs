using _3dlogyERP.Application.Dtos.EquipmentDtos;
using _3dlogyERP.Application.Interfaces;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _3dlogyERP.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EquipmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            ArgumentNullException.ThrowIfNull(mapper);

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EquipmentListDto> GetEquipmentByIdAsync(int id)
        {
            var equipment = await _unitOfWork.Equipment
                .Query()
                .Include(e => e.EquipmentType)
                .FirstOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<EquipmentListDto>(equipment);
        }

        public async Task<IEnumerable<EquipmentListDto>> GetAllEquipmentAsync()
        {
            var equipment = await _unitOfWork.Equipment
                .Query()
                .Include(e => e.EquipmentType)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EquipmentListDto>>(equipment);
        }

        public async Task<EquipmentListDto> CreateEquipmentAsync(EquipmentCreateDto equipmentDto)
        {
            ArgumentNullException.ThrowIfNull(equipmentDto);

            var equipment = _mapper.Map<Equipment>(equipmentDto);
            await _unitOfWork.Equipment.AddAsync(equipment);
            await _unitOfWork.SaveChangesAsync();

            return await GetEquipmentByIdAsync(equipment.Id);
        }

        public async Task<EquipmentListDto> UpdateEquipmentAsync(int id, EquipmentUpdateDto equipmentDto)
        {
            ArgumentNullException.ThrowIfNull(equipmentDto);

            var existingEquipment = await _unitOfWork.Equipment.GetByIdAsync(id);
            if (existingEquipment == null)
                return null;

            _mapper.Map(equipmentDto, existingEquipment);
            await _unitOfWork.SaveChangesAsync();

            return await GetEquipmentByIdAsync(id);
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

        public async Task<IEnumerable<EquipmentListDto>> GetEquipmentByTypeAsync(int equipmentTypeId)
        {
            var equipment = await _unitOfWork.Equipment
                .Query()
                .Include(e => e.EquipmentType)
                .Where(e => e.EquipmentTypeId == equipmentTypeId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EquipmentListDto>>(equipment);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _unitOfWork.Equipment
                .Query()
                .AnyAsync(e => e.Id == id);
        }
    }
}
