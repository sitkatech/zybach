namespace Zybach.API.Models
{
    public class ApiResult<T>
    {
        public string Status { get; set; }

        public T Result { get; set; }
    }
}