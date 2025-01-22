using System;

namespace _3dlogyERP.Core.Entities
{
    public class OrderDocument
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        
        // Navigation property
        public virtual Order Order { get; set; }
    }
}
