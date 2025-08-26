using CustomMVC.Core.Http;

namespace CustomMVC.Mvc.Results
{
    public sealed class FileResult : IActionResult
    {
        private readonly byte[] _bytes;
        private readonly string _contentType;
        private readonly string? _downloadFileName;

        public FileResult(byte[] bytes, string contentType, string? downloadFileName = null)
        {
            _bytes = bytes;
            _contentType = contentType;
            _downloadFileName = downloadFileName;
        }

        public async Task ExecuteAsync(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;

            if (!string.IsNullOrWhiteSpace(_downloadFileName))
                ctx.Response.Headers["Content-Disposition"] = $"attachment; filename=\"{_downloadFileName}\"";

            await ctx.Response.WriteAsync(_bytes, _contentType);
        }

    }
}
