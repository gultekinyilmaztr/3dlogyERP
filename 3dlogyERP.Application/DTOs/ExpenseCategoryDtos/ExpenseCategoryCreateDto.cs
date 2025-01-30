namespace _3dlogyERP.Application.Dtos.ExpenseCategoryDtos
{
    // Oluşturma işlemi için kullanılacak DTO
    public class ExpenseCategoryCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? ParentCategoryId { get; set; }
    }
}