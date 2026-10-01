namespace Voucherly.Sdk.Exceptions;

public class VoucherlyException : Exception
{
    public VoucherlyException(string message)
        : base(message)
    {
    }

    public VoucherlyException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// No HTTP response was received: DNS failure, refused connection, TLS error or timeout.
/// </summary>
public class ConnectionException(string message, Exception? innerException) : VoucherlyException(message, innerException);
