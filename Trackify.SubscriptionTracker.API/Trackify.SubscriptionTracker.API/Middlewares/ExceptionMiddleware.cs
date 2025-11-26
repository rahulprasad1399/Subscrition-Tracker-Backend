namespace Trackify.SubscriptionTracker.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                var response = new { Message = "Validation Failed", Errors = errors };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new { Message = "Internal Server Error", Details = ex.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
