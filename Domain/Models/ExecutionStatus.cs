namespace Domain.Models
{
    public class ExecutionStatus
    {
        /// <summary>
        /// Статус код серв.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Статус може бути "OK" або "ER"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Список помилок.
        /// </summary>
        public List<string> Errors { get; set; }

        public ExecutionStatus()
        {
            ErrorCode = 200;
            Status = "OK";
            Errors = new List<string>();

        }
    }
}
