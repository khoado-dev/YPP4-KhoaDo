using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.DTOs
{
    public sealed class BoardWithCollectionDTO
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; } = string.Empty;
        public string BoardBackgroundImage { get; set; } = string.Empty;

        public int CollectionId { get; set; }
        public string CollectionName { get; set; } = string.Empty;

        public int WorkspaceId { get; set; }
    }

}
