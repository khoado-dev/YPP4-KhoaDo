
namespace UnitTestForTrello.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkspaceTypeEnum Type { get; set; }
    }
}