using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloSyncOptimizer.Application.Common.Models;

namespace VeloSyncOptimizer.Application.Common.Helpers;

public static class ResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failure<T>(string message, List<string>? errors = null, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static ApiResponse<object> Error(string message, List<string>? errors = null, int statusCode = 500)
    {
        return new ApiResponse<object>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new List<string>(),
            Data = null
        };
    }
}
