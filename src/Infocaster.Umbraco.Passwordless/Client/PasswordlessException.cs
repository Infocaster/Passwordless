using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Infocaster.Umbraco.Passwordless.Client;

/// <summary>
/// An exception that is thrown when the response from Passwordless does not indicate success
/// </summary>
public class PasswordlessException : Exception
{
    /// <summary>
    /// An object of any type with more information about the actual error
    /// </summary>
    public object? ErrorData { get; }

    /// <summary>
    /// Create an instance of this exception
    /// </summary>
    /// <param name="errorData">An object of any type with more information about the actual error</param>
    public PasswordlessException(object? errorData)
    {
        ErrorData = errorData;
    }

    /// <inheritdoc cref="PasswordlessException(object)"/>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="errorData">An object of any type with more information about the actual error</param>
    public PasswordlessException(string message, object? errorData)
        : base(message)
    {
        ErrorData = errorData;
    }

    /// <inheritdoc />
    public PasswordlessException(string message, Exception innerException)
        : base(message, innerException)
    { }

    /// <inheritdoc />
    protected PasswordlessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Join("\n", new[] { ErrorData?.ToString(), base.ToString() }.Where(x => x != null));
    }
}