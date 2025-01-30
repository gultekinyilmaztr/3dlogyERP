using System.ComponentModel.DataAnnotations;

namespace _3dlogyERP.WebUI.Dtos.CustomerDtos
{
    public class CustomerCreateDto
    {
        [Required(ErrorMessage = "Firma adı zorunludur")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "İletişim kişisi zorunludur")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Adres zorunludur")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vergi numarası zorunludur")]
        public string TaxNumber { get; set; }

        [Required(ErrorMessage = "Vergi dairesi zorunludur")]
        public string TaxOffice { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
