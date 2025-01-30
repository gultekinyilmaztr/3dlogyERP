namespace _3dlogyERP.Application.Dtos.ExpenseCategoryDtos
{

    public class ExpenseCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public int ExpenseCount { get; set; }
        public int SubCategoryCount { get; set; }
    }
}