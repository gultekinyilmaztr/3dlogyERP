namespace _3dlogyERP.Application.Dtos.ExpenseCategoryDtos
{
    // Detaylı görüntüleme için kullanılacak DTO
    public class ExpenseCategoryDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public ICollection<ExpenseCategoryListDto> SubCategories { get; set; }
        public decimal TotalExpenseAmount { get; set; }
    }
}