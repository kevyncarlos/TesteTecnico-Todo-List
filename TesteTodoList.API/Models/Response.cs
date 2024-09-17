using System.Collections;

namespace TesteTodoList.API.Models;

public class Response<T>
{
    public string Message { get; set; }
    public bool Success { get; set; }
    public T? Data { get; set; }

    public static Response<T> GenerateWithError(string message)
    {
        return new Response<T> { Data = default, Success = false, Message = message };
    }

    public static Response<T> GenerateWithSuccess(string message, T data)
    {
        return new Response<T> { Data = data, Success = true, Message = message };
    }
}
