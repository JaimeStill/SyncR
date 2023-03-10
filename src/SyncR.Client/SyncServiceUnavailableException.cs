namespace SyncR.Client;
public class SyncServiceUnavailableException : Exception
{
    public SyncServiceUnavailableException(string endpoint)
        : base($"Service unavailable at sync endpoint {endpoint}") { }
}