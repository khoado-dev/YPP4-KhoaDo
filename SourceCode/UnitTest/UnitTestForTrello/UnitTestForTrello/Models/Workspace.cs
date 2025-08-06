using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public string? WorkspaceName { get; set; }
        public string? WorkspaceDescription { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public string? LogoUrl { get; set; }
        public List<Board> Boards { get; set; } = new List<Board>();
    }
}
