using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public class CardAttachmentDTO
    {
        public int AttachmentId { get; set; }
        public string? AttachmentType { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public string Size { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsCover { get; set; }
        public int CardId { get; set; }
    }

}
