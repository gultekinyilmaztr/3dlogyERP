namespace _3dlogyERP.Core.Constants
{
    public static class ErrorMessages
    {
        // Genel Hatalar
        public const string UnexpectedError = "Beklenmeyen bir hata oluştu.";
        public const string ValidationError = "Doğrulama hatası oluştu.";
        public const string UnauthorizedAccess = "Bu işlem için yetkiniz bulunmamaktadır.";
        public const string InvalidCredentials = "Geçersiz kullanıcı adı veya şifre.";

        // Kullanıcı İşlemleri
        public const string UserNotFound = "Kullanıcı bulunamadı.";
        public const string EmailAlreadyExists = "Bu e-posta adresi zaten kayıtlı.";
        public const string InvalidPassword = "Geçersiz şifre.";
        public const string PasswordsDoNotMatch = "Şifreler eşleşmiyor.";

        // Sipariş İşlemleri
        public const string OrderNotFound = "Sipariş bulunamadı.";
        public const string InvalidOrderStatus = "Geçersiz sipariş durumu.";
        public const string OrderCannotBeModified = "Bu sipariş artık değiştirilemez.";

        // Malzeme İşlemleri
        public const string MaterialNotFound = "Malzeme bulunamadı.";
        public const string InsufficientStock = "Yetersiz stok miktarı.";
        public const string MaterialInUse = "Bu malzeme aktif siparişlerde kullanıldığı için silinemez.";

        // Müşteri İşlemleri
        public const string CustomerNotFound = "Müşteri bulunamadı.";
        public const string CustomerHasActiveOrders = "Müşterinin aktif siparişleri olduğu için silinemez.";

        // Gider İşlemleri
        public const string ExpenseNotFound = "Gider kaydı bulunamadı.";
        public const string ExpenseCategoryNotFound = "Gider kategorisi bulunamadı.";
        public const string CategoryHasExpenses = "Bu kategoriye ait giderler olduğu için silinemez.";

        // Form ve Veri Doğrulama
        public const string RequiredField = "{0} alanı zorunludur.";
        public const string InvalidEmail = "Geçersiz e-posta adresi.";
        public const string InvalidPhoneNumber = "Geçersiz telefon numarası.";
        public const string InvalidDate = "Geçersiz tarih.";
        public const string InvalidAmount = "Geçersiz tutar.";
        public const string InvalidQuantity = "Geçersiz miktar.";
    }
}
