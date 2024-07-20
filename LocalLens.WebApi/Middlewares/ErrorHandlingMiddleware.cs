//using LocalLens.WebApi.Result;
//using System.Diagnostics;

//namespace LocalLens.WebApi.Middlewares;

//public class ErrorHandlingMiddleware
//{
//    private readonly RequestDelegate _next;

//    public ErrorHandlingMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private Task HandleExceptionAsync(HttpContext context, Exception exception)
//    {
//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

//        // Handle specific exceptions if needed
//        if (exception is FluentValidation.ValidationException validationException)
//        {
//            var fluentValidationError = new FluentValidationErrorResponse
//            {
//                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
//                Title = "One or more validation errors occurred.",
//                Status = StatusCodes.Status400BadRequest,
//                Errors = validationException.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage }),
//                TraceId = Activity.Current?.Id ?? context.TraceIdentifier
//            };

//            ResultT.Failure();

//            var customErrorResponse = new ResultT
//            {
//                Result = null,
//                IsSuccess = false,
//                Error = new ErrorDetail
//                {
//                    Title = "Validation Error",
//                    ErrorType = 1,
//                    OriginalError = fluentValidationError
//                }
//            };

//            var result = JsonConvert.SerializeObject(customErrorResponse);
//            return context.Response.WriteAsync(result);
//        }

//        // Handle other exceptions if needed
//        // For simplicity, return a generic error message
//        var errorResponse = new CustomErrorResponse
//        {
//            Result = null,
//            IsSuccess = false,
//            Error = new ErrorDetail
//            {
//                Title = "Internal Server Error",
//                ErrorType = 2,
//                OriginalError = null
//            }
//        };

//        var errorResult = JsonConvert.SerializeObject(errorResponse);
//        return context.Response.WriteAsync(errorResult);
//    }
//}
