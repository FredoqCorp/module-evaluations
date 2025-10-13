using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// Global exception handler that transforms exceptions into RFC 7807 ProblemDetails responses.
/// </summary>
internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    /// <summary>
    /// Initializes the exception handler with logging and environment information.
    /// </summary>
    /// <param name="logger">Logger for recording exception details.</param>
    /// <param name="environment">Host environment to determine development mode.</param>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Handles exceptions by converting them to ProblemDetails responses.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the current request.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the exception was handled, false otherwise.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}",
            traceId);

        var problemDetails = CreateProblemDetails(exception, httpContext, traceId);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetails(Exception exception, HttpContext context, string traceId)
    {
        var (status, title, detail) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = _environment.IsDevelopment() ? detail : GetSafeDetail(status),
            Instance = context.Request.Path,
            Extensions =
            {
                ["traceId"] = traceId
            }
        };

        // In development, include additional diagnostic information
        if (_environment.IsDevelopment())
        {
            problemDetails.Extensions["exceptionType"] = exception.GetType().Name;

            if (exception.StackTrace is not null)
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace.Split('\n');
            }

            if (exception.InnerException is not null)
            {
                problemDetails.Extensions["innerException"] = new
                {
                    type = exception.InnerException.GetType().Name,
                    message = exception.InnerException.Message
                };
            }
        }

        return problemDetails;
    }

    private static (int Status, string Title, string Detail) MapException(Exception exception)
    {
        return exception switch
        {
            // PostgreSQL exceptions
            PostgresException pgEx => MapPostgresException(pgEx),

            // Npgsql connection exceptions
            NpgsqlException npgsqlEx => (
                StatusCodes.Status503ServiceUnavailable,
                "Database Connection Error",
                $"Unable to connect to the database: {npgsqlEx.Message}"
            ),

            // Operation cancelled (client disconnected or timeout)
            OperationCanceledException => (
                StatusCodes.Status499ClientClosedRequest,
                "Request Cancelled",
                "The request was cancelled"
            ),

            // Timeout exceptions
            TimeoutException => (
                StatusCodes.Status504GatewayTimeout,
                "Request Timeout",
                "The request took too long to process"
            ),

            // Argument exceptions (validation errors)
            ArgumentException argEx => (
                StatusCodes.Status400BadRequest,
                "Invalid Argument",
                argEx.Message
            ),

            // Default: Internal Server Error
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                exception.Message
            )
        };
    }

    private static (int Status, string Title, string Detail) MapPostgresException(PostgresException pgEx)
    {
        // PostgreSQL error codes: https://www.postgresql.org/docs/current/errcodes-appendix.html
        return pgEx.SqlState switch
        {
            // Class 23 - Integrity Constraint Violation
            "23000" or "23001" or "23502" or "23503" or "23505" or "23514" => (
                StatusCodes.Status409Conflict,
                "Database Constraint Violation",
                GetConstraintViolationMessage(pgEx)
            ),

            // Class 42 - Syntax Error or Access Rule Violation
            "42P01" => ( // undefined_table
                StatusCodes.Status500InternalServerError,
                "Database Schema Error",
                $"Required database table does not exist: {pgEx.TableName ?? "unknown"}"
            ),
            "42701" => ( // duplicate_column
                StatusCodes.Status500InternalServerError,
                "Database Schema Error",
                "Duplicate column in database schema"
            ),
            "42703" => ( // undefined_column
                StatusCodes.Status500InternalServerError,
                "Database Schema Error",
                $"Required database column does not exist: {pgEx.ColumnName ?? "unknown"}"
            ),

            // Class 53 - Insufficient Resources
            "53000" or "53100" or "53200" or "53300" or "53400" => (
                StatusCodes.Status503ServiceUnavailable,
                "Database Resource Unavailable",
                "Database server is experiencing resource constraints"
            ),

            // Class 57 - Operator Intervention
            "57000" or "57014" or "57P01" or "57P02" or "57P03" => (
                StatusCodes.Status503ServiceUnavailable,
                "Database Unavailable",
                "Database operation was interrupted"
            ),

            // Class 58 - System Error
            "58000" or "58030" or "58P01" or "58P02" => (
                StatusCodes.Status500InternalServerError,
                "Database System Error",
                "Database system encountered an error"
            ),

            // Default PostgreSQL error
            _ => (
                StatusCodes.Status500InternalServerError,
                "Database Error",
                $"Database operation failed: {pgEx.MessageText}"
            )
        };
    }

    private static string GetConstraintViolationMessage(PostgresException pgEx)
    {
        return pgEx.SqlState switch
        {
            "23505" => $"A record with this {pgEx.ConstraintName ?? "value"} already exists",
            "23503" => $"Referenced record does not exist: {pgEx.ConstraintName ?? "foreign key violation"}",
            "23502" => $"Required field cannot be null: {pgEx.ColumnName ?? "unknown"}",
            _ => pgEx.MessageText
        };
    }

    private static string GetSafeDetail(int statusCode)
    {
        return statusCode switch
        {
            400 => "The request contains invalid data",
            401 => "Authentication is required",
            403 => "You don't have permission to access this resource",
            404 => "The requested resource was not found",
            409 => "The request conflicts with existing data",
            422 => "The request cannot be processed",
            429 => "Too many requests",
            500 => "An internal server error occurred",
            502 => "Bad gateway",
            503 => "The service is temporarily unavailable",
            504 => "The request timed out",
            _ => "An error occurred while processing your request"
        };
    }
}
