using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloSyncOptimizer.Application.Common.Models;

namespace VeloSyncOptimizer.Application.Common.Helpers;

public static class ResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failure<T>(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static ApiResponse<object> Error(string message, List<string>? errors = null)
    {
        return new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>(),
            Data = null
        };
    }
}
