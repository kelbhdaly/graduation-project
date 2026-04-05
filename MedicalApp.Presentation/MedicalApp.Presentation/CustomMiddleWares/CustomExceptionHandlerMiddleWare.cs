namespace MedicalApp.Presentation.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problem = CreateProblemDetails(exception, httpContext);

            httpContext.Response.StatusCode = problem.Status ?? 500;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;

        }

        private ProblemDetails CreateProblemDetails(Exception exception, HttpContext context)
        {
            return exception switch
            {
                InvalidFormatException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Format",
                    Detail = exception.Message,
                },

                UnauthorizedException => new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = exception.Message,
                },

                PostNotFoundException => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Post Not Found",
                    Detail = exception.Message,
                },

                InvalidEmailException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Email",
                    Detail = exception.Message,
                },

                InvalidCreateException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Create Operation",
                    Detail = exception.Message,
                },

                InvalidPasswordException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Password",
                    Detail = exception.Message,
                },

                PostAlreadyInFavoritesException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Post Already in Favorites",
                    Detail = exception.Message,
                },

                NotFoundFavoritePostException => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Favorite Post Not Found",
                    Detail = exception.Message,
                },

                InvalidResetPasswordException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Operation",
                    Detail = exception.Message,
                },

                InvalidRestPasswordException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Reset Password Operation",
                    Detail = exception.Message,
                },

                BadRequestException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message,
                },

                NotFoundUserException => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "User Not Found",
                    Detail = exception.Message,
                },  
            };

        }
    }
}
