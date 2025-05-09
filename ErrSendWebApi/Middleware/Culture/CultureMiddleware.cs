using System.Globalization;

namespace ErrSendWebApi.Middleware.Culture
{
    /// <summary>
    /// Middleware для примусового встановлення культури програми.
    /// </summary>
    public class CultureMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="next">Делегат запиту</param>
        public CultureMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Виконання делегата встановлення культури.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Перехід на наступний запрос.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("uk-UA");
                CultureInfo.CurrentUICulture = new CultureInfo("uk-UA");
            }
            catch (CultureNotFoundException) { }
            await next.Invoke(context);
        }
    }
}
