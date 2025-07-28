
namespace UnitTestForTrello.Models
{
    public class Workspace
    {
        public string Title { get; set; }
        public Workspace(string title)
        {
            if (String.IsNullOrEmpty(title)) throw new ArgumentNullException("title");
            Title = title;
        }
    }
}