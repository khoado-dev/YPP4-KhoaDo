using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public sealed class CollectionDTO
    {
        public int CollectionId { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int WorkspaceId { get; set; }
    }

}
