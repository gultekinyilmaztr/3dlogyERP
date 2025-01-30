namespace _3dlogyERP.Application.Dtos.ExpenseCategoryDtos
{
    // Güncelleme işlemi için kullanılacak DTO
    public class ExpenseCategoryUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}