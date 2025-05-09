using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ErrSendApplication.Proceses;

public class SendErrorMessage
{
    public class Command : IRequest<ExecutionStatus>
    {
        public ErrorMessage ErrorMessage { get; set; }
    }
        
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ErrorMessage).NotNull();
            RuleFor(x => x.ErrorMessage.Application).NotEmpty();
            RuleFor(x => x.ErrorMessage.Message).NotEmpty();
        }
    }
        
    public class Handler : IRequestHandler<Command, ExecutionStatus>
    {
        private readonly TelegramBotService _telegramBotService;
        private readonly ILogger<Handler> _logger;
            
        public Handler(TelegramBotService telegramBotService, ILogger<Handler> logger)
        {
            _telegramBotService = telegramBotService;
            _logger = logger;
        }
            
        public async Task<ExecutionStatus> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = new ExecutionStatus();
                
            var success = await _telegramBotService.SendErrorMessageAsync(request.ErrorMessage);
                
            if (!success)
            {
                result.Status = "ER";
                result.Errors.Add("Не вдалося відправити повідомлення про помилку в Telegram");
                result.ErrorCode = 500;
                _logger.LogError("Не вдалося відправити повідомлення про помилку в Telegram");
            }
                
            return result;
        }
    }
}