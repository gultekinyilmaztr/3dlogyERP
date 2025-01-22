namespace _3dlogyERP.Core.Entities
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }                     // Kategori Adı (Hammadde, Sarf, Makine vb.)
        public string Description { get; set; }              // Açıklama
        public bool IsActive { get; set; } = true;           // Aktif/Pasif Durumu
        public int? ParentCategoryId { get; set; }           // Üst Kategori ID (Alt kategoriler için)
        public ExpenseCategory ParentCategory { get; set; }  // Üst Kategori
        public ICollection<ExpenseCategory> SubCategories { get; set; } // Alt Kategoriler
        public ICollection<Expense> Expenses { get; set; }    // Bu kategoriye ait harcamalar

        public ExpenseCategory()
        {
            SubCategories = new List<ExpenseCategory>();
            Expenses = new List<Expense>();
        }
    }
}
