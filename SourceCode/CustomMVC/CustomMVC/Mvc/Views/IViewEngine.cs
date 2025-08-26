namespace CustomMVC.Mvc.Views
{
    public interface IViewEngine
    {
        string Render(string viewName, object? model);
    }
}
