namespace JapaneseLearnSystem.Extension
{
    // 檔名: HttpRequestExtensions.cs
    using Microsoft.AspNetCore.Http;

    namespace JapaneseLearnSystem.Extensions
    {
        public static class HttpRequestExtensions
        {
            public static bool IsAjaxRequest(this HttpRequest request)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
        }
    }

}
