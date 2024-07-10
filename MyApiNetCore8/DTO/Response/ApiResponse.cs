using System.Text.Json.Serialization;

namespace MyApiNetCore8.DTO.Response
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; } = 1000;

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("result")]
        public T Result { get; set; }

        public ApiResponse(int code, string message, T result)
        {
            Code = code;
            Message = message;
            Result = result;
        }
    }
}
