namespace ErrSendWebApi.Middleware.Culture
{
    /// <summary>
    /// Клас розширення для пвдключення культури в конвеєр програми.
    /// </summary>
    public static class CultureExtensions
    {
        /// <summary>
        /// Метод розширення для пвдключення культури в конвеєр програми.
        /// </summary>
        /// <param name="builder">Об'єкт конвеєра IApplicationBuilder</param>
        /// <returns>Підключення в контейнер CultureMiddleware</returns>
        public static IApplicationBuilder UseCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureMiddleware>();
        }
    }
}
