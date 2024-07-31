namespace Zybach.Models.DataTransferObjects
{
    public class ErrorMessage
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public ErrorMessage(){}

        public ErrorMessage(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}