using CustomMVC.Core.Mvc.Views;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

public sealed class SimpleViewEngine : IViewEngine
{
    private readonly string _basePath;

    public SimpleViewEngine(string? basePath = null)
    {
        _basePath = Path.Combine(AppContext.BaseDirectory, "App", "Views");
    }

    public string Render(string viewName, object? model)
    {
        // Build the file path like "Users/Index.html"
        var parts = viewName.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        var filePath = Path.Combine(_basePath, Path.Combine(parts) + ".html");
        if (!File.Exists(filePath)) throw new FileNotFoundException($"View not found: {filePath}");

        var template = File.ReadAllText(filePath);

        // 1) Handle @foreach blocks first (so body can still contain tokens)
        template = RenderForeach(template, model);

        // 2) Replace {{ Property }} tokens using top-level model properties (simple mustache-style)
        if (model != null)
        {
            foreach (var p in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var val = p.GetValue(model)?.ToString() ?? "";
                var pattern = @"{{\s*" + Regex.Escape(p.Name) + @"\s*}}";
                template = Regex.Replace(template, pattern, val);
            }
        }

        return template;
    }

    // --- Minimal @foreach support ---
    // Supports: @foreach (var u in Model.Users) { ... }
    // Inside the block you can use: @u.Property and @index (1-based)
    private static string RenderForeach(string input, object? model)
    {
        if (model == null) return input;

        // Regex breakdown:
        // @foreach (var <var> in Model.<src>) { <body> }
        var rx = new Regex(
            @"@foreach\s*\(\s*var\s+(?<var>\w+)\s+in\s+Model\.(?<src>\w+)\s*\)\s*\{(?<body>[\s\S]*?)\}",
            RegexOptions.Singleline | RegexOptions.CultureInvariant);

        return rx.Replace(input, m =>
        {
            var varName = m.Groups["var"].Value; // e.g., "u"
            var srcName = m.Groups["src"].Value; // e.g., "Users"
            var body = m.Groups["body"].Value;

            // Get Model.<src> as an IEnumerable
            var srcObj = GetPropObject(model, srcName);
            if (srcObj is not IEnumerable seq || srcObj is string)
                return string.Empty; // Not a collection → render nothing

            var sb = new System.Text.StringBuilder();
            var idx = 0;

            foreach (var item in seq)
            {
                // Replace @<var>.Property (one level: @u.Name, @u.Email, etc.)
                var rendered = Regex.Replace(
                    body,
                    $@"@{Regex.Escape(varName)}\.(\w+)",
                    mm =>
                    {
                        var prop = mm.Groups[1].Value;
                        return GetPropValue(item, prop) ?? string.Empty;
                    },
                    RegexOptions.CultureInvariant
                );

                // Replace @index with 1-based index
                rendered = Regex.Replace(rendered, @"@index\b", (idx + 1).ToString(), RegexOptions.CultureInvariant);

                sb.Append(rendered);
                idx++;
            }

            return sb.ToString();
        });
    }

    // Get a public instance property value as object
    private static object? GetPropObject(object obj, string name)
        => obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
           ?.GetValue(obj);

    // Get a public instance property value as string
    private static string? GetPropValue(object obj, string name)
        => obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
           ?.GetValue(obj)?.ToString();
}
