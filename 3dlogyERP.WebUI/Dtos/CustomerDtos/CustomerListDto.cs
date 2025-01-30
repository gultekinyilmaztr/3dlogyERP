namespace _3dlogyERP.WebUI.Dtos.CustomerDtos
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int OrderCount { get; set; }
    }
}
