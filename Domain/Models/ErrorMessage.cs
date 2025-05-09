namespace Domain.Models;

/// <summary>
/// Модель для передачі інформації про помилку
/// </summary>
public class ErrorMessage
{
    /// <summary>
    /// Назва додатку, в якому виникла помилка
    /// </summary>
    public string Application { get; set; }
        
    /// <summary>
    /// Версія додатку
    /// </summary>
    public string Version { get; set; }
        
    /// <summary>
    /// Середовище виконання (Development, Staging, Production)
    /// </summary>
    public string Environment { get; set; }
        
    /// <summary>
    /// Повідомлення про помилку
    /// </summary>
    public string Message { get; set; }
        
    /// <summary>
    /// Стек виклику
    /// </summary>
    public string StackTrace { get; set; }
        
    /// <summary>
    /// Додаткова інформація
    /// </summary>
    public string AdditionalInfo { get; set; }
        
    /// <summary>
    /// Час виникнення помилки
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}