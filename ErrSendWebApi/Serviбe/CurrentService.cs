using ErrSendApplication.Interfaces;

namespace ErrSendWebApi.Serviсe
{
    /// <summary>
    /// Клас для ініціалізвції теперішнього сервіса потрібний для логування.
    /// </summary>
    public class CurrentService : ICurrentService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Конструктор ініціалізує httpContextAccessor
        /// </summary>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        public CurrentService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
    }
}
