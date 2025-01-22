using System;

namespace _3dlogyERP.Core.Entities
{
    public class CustomerDocument
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        
        // Navigation property
        public virtual Customer Customer { get; set; }
    }
}
