using _3dlogyERP.Application.Dtos.CustomerDtos;
using _3dlogyERP.Application.Dtos.EquipmentDtos;
using _3dlogyERP.Application.Dtos.ExpenseDtos;
using _3dlogyERP.Application.Dtos.MaterialDtos;
using _3dlogyERP.Application.Dtos.MaterialTypeDtos;
using _3dlogyERP.Application.Dtos.OrderDtos;
using _3dlogyERP.Application.Dtos.UserDtos;
using _3dlogyERP.Core.Entities;
using AutoMapper;
using _3dlogyERP.Core.Enums;


namespace _3dlogyERP.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer mappings
            CreateMap<Customer, CustomerListDto>()
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders.Count));
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();
            CreateMap<Customer, CustomerUpdateDto>();

            // Equipment mappings
            CreateMap<Equipment, EquipmentListDto>()
                .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Name));
            CreateMap<EquipmentCreateDto, Equipment>();
            CreateMap<EquipmentUpdateDto, Equipment>();
            CreateMap<Equipment, EquipmentUpdateDto>();

            // Expense mappings
            CreateMap<Expense, ExpenseListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<ExpenseCreateDto, Expense>();
            CreateMap<ExpenseUpdateDto, Expense>();
            CreateMap<Expense, ExpenseUpdateDto>();

            // Material mappings
            CreateMap<Material, MaterialListDto>()
            .ForMember(dest => dest.StockCategory, opt => opt.MapFrom(src => src.StockCategory.GetDescription()));

            CreateMap<MaterialCreateDto, Material>()
                .ForMember(dest => dest.StockCategory, opt => opt.MapFrom(src => src.StockCategory));

            CreateMap<MaterialUpdateDto, Material>()
                .ForMember(dest => dest.StockCategory, opt => opt.MapFrom(src => src.StockCategory));

            CreateMap<Material, MaterialUpdateDto>()
                .ForMember(dest => dest.StockCategory, opt => opt.MapFrom(src => src.StockCategory));

            // MaterialType mappings
            CreateMap<MaterialType, MaterialTypeListDto>();
            CreateMap<MaterialTypeCreateDto, MaterialType>();
            CreateMap<MaterialTypeUpdateDto, MaterialType>();
            CreateMap<MaterialType, MaterialTypeUpdateDto>();

            // Order mappings
            CreateMap<Order, OrderListDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CompanyName))
                .ForMember(dest => dest.ServiceCount, opt => opt.MapFrom(src => src.Services.Count));
            CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderUpdateDto, Order>();
            CreateMap<Order, OrderUpdateDto>();

            // User mappings
            CreateMap<User, UserListDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
        }
    }
}
