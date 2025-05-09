using System.Text;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ErrSendPersistensTelegram.Services;

public class TelegramBotService
    {
        private readonly ITelegramBotClient botClient;
        private readonly string chatId;
        private readonly ILogger<TelegramBotService> logger;
        private readonly bool enabled;

        public TelegramBotService(IConfiguration configuration, ILogger<TelegramBotService> logger)
        {
            this.logger = logger;
            
            enabled = configuration.GetValue<bool>("Telegram:Enabled");
            if (!enabled)
            {
                this.logger.LogWarning("Telegram відправка вимкнена в конфігурації");
                return;
            }
            
            var botToken = configuration["Telegram:BotToken"];
            chatId = configuration["Telegram:ChatId"];
            
            if (string.IsNullOrEmpty(botToken) || string.IsNullOrEmpty(chatId))
            {
                this.logger.LogError("Токен бота або ID чату не вказані в конфігурації");
                enabled = false;
                return;
            }
            
            botClient = new TelegramBotClient(botToken);
        }

        public async Task<bool> SendErrorMessageAsync(ErrorMessage error)
        {
            if (!enabled)
            {
                logger.LogWarning("Спроба відправити повідомлення при вимкненому сервісі Telegram");
                return false;
            }
            
            try
            {
                if (error == null)
                {
                    logger.LogWarning("Спроба відправити null-помилку");
                    return false;
                }

                var message = FormatErrorMessage(error);
                
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message,
                    parseMode: ParseMode.Html
                );

                logger.LogInformation($"Повідомлення про помилку успішно відправлено: {error.Message}");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Помилка відправки повідомлення в Telegram: {ex.Message}");
                return false;
            }
        }

        private string FormatErrorMessage(ErrorMessage error)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine($"<b>⚠️ ПОМИЛКА</b>");
            sb.AppendLine();
            sb.AppendLine($"<b>📱 Додаток:</b> {HtmlEncode(error.Application)}");
            
            if (!string.IsNullOrEmpty(error.Version))
                sb.AppendLine($"<b>🔄 Версія:</b> {HtmlEncode(error.Version)}");
                
            if (!string.IsNullOrEmpty(error.Environment))
                sb.AppendLine($"<b>🖥 Середовище:</b> {HtmlEncode(error.Environment)}");
                
            sb.AppendLine($"<b>⏱ Час:</b> {error.Timestamp:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();
            sb.AppendLine($"<b>❌ Повідомлення:</b> {HtmlEncode(error.Message)}");
            
            if (!string.IsNullOrEmpty(error.StackTrace))
            {
                sb.AppendLine();
                sb.AppendLine($"<b>📊 Стек виклику:</b>");
                sb.AppendLine($"<pre>{HtmlEncode(TruncateStackTrace(error.StackTrace))}</pre>");
            }
            
            if (!string.IsNullOrEmpty(error.AdditionalInfo))
            {
                sb.AppendLine();
                sb.AppendLine($"<b>📋 Додаткова інформація:</b>");
                sb.AppendLine($"{HtmlEncode(error.AdditionalInfo)}");
            }

            return sb.ToString();
        }

        private string TruncateStackTrace(string stackTrace)
        {
            // Обмежуємо довжину стеку до 800 символів для Telegram
            if (string.IsNullOrEmpty(stackTrace))
                return string.Empty;
                
            const int maxLength = 800;
            
            if (stackTrace.Length <= maxLength)
                return stackTrace;
                
            return stackTrace.Substring(0, maxLength) + "...";
        }
        
        private string HtmlEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
                
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }
    }