using ErrSendApplication.Interfaces;
using MediatR;
using Serilog;

namespace ErrSendApplication.Behaviors
{
    /// <summary>
    /// Клас для поведінки логування через Serilog.
    /// </summary>
    /// <typeparam name="TRequest">Запит</typeparam>
    /// <typeparam name="TResponse">Відповідь</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        ICurrentService currentService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="currentUserService">Поточний сервіс</param>
        public LoggingBehavior(ICurrentService currentUserService)
        {
            this.currentService = currentUserService;
        }

        /// <summary>
        /// Метод обробки логування у файл.
        /// </summary>
        /// <param name="request">Запит</param>
        /// <param name="next">Делегат що переключає наступну відповідь.</param>
        /// <param name="cancellationToken">Токен відміни</param>
        /// <returns>Відповідь</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            Log.Information("Request: {Name} --- {@Request} ", requestName, request);

            var response = await next();

            return response;
        }
    }
}
