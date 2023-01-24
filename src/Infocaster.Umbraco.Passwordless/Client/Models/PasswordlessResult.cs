using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Infocaster.Umbraco.Passwordless.Client.Models;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class PasswordlessResult
{
    private readonly Exception _exception;

    protected PasswordlessResult(bool success, Exception exception = null)
    {
        Success = success;
        _exception = exception;
    }

    public bool Success { get; }
    public Exception Exception => !Success ? _exception : throw new InvalidOperationException(string.Format("Cannot get {0} when {1} is {2}.", nameof(Exception), nameof(Success), true));
    public static PasswordlessResult CreateSuccess()
        => new PasswordlessResult(true);

    public static PasswordlessResult CreateFailure(Exception e)
        => new PasswordlessResult(false, e);

    public static PasswordlessResult<TValue> CreateSuccess<TValue>(TValue value)
        => new PasswordlessResult<TValue>(true, value);

    public static PasswordlessResult<TValue> CreateFailure<TValue>(Exception e)
        => new PasswordlessResult<TValue>(false, default, e);

    public static async Task<PasswordlessResult<TValue>> ExecuteSafelyAsync<TValue>(Func<Task<TValue>> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        try
        {
            return CreateSuccess(await func().ConfigureAwait(false));
        }
        catch (Exception e)
        {
            return CreateFailure<TValue>(e);
        }
    }

    public static async Task<PasswordlessResult> ExecuteSafelyAsync(Func<Task> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        try
        {
            await func().ConfigureAwait(false);
            return CreateSuccess();
        }
        catch (Exception e)
        {
            return CreateFailure(e);
        }
    }

    private string GetDebuggerDisplay()
    {
        return Success ? "Success" : "Failure";
    }
}

public class PasswordlessResult<TValue> : PasswordlessResult
{
    private readonly TValue _value;

    internal PasswordlessResult(bool success, TValue value, Exception exception = null)
        : base(success, exception)
    {
        if (success && value.Equals(default(TValue))) throw new ArgumentNullException(nameof(value));

        _value = value;
    }

    public TValue Value => Success ? _value : throw new InvalidOperationException(string.Format("Cannot get {0} when {1} is {2}.", nameof(Value), nameof(Success), false), Exception);

    public PasswordlessResult<TTarget> Map<TTarget>(Func<TValue, TTarget> map)
    {
        if (map is null)
        {
            throw new ArgumentNullException(nameof(map));
        }

        if (Success)
        {
            return CreateSuccess(map(Value));
        }

        return CreateFailure<TTarget>(Exception);
    }
}