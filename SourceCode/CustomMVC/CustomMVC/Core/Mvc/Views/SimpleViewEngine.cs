using System.Text.RegularExpressions;

namespace CustomMVC.Core.Mvc.Views
{
    public sealed class SimpleViewEngine : IViewEngine
    {
        private readonly string _basePath;

        public SimpleViewEngine()
        {
            _basePath = Path.Combine(AppContext.BaseDirectory, "App", "Views");
        }

        public string Render(string viewName, object? model)
        {
            var parts = viewName.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var normalized = Path.Combine(parts); // "Users\Profile"
            var filePath = Path.Combine(_basePath, normalized + ".html");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"View not found: {filePath}");

            var template = File.ReadAllText(filePath); //read html template file

            if (model == null) return template;

            // Replace {{Property}} by value
            var props = model.GetType().GetProperties(); //get all properties of model
            foreach (var p in props) //for each property
            {
                var val = p.GetValue(model)?.ToString() ?? "";
                template = Regex.Replace(template, @"{{\s*" + p.Name + @"\s*}}", val); //pass value into template
            }
            return template;
        }
    }
}
