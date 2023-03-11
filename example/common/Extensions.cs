namespace Common;
public static class Extensions
{
    public static string ToActionString(this Intent intent) => intent switch
    {
        Intent.Acquire => "Acquisition",
        Intent.Approve => "Approval",
        Intent.Destroy => "Destruction",
        Intent.Transfer => "Transfer",
        _ => throw new ArgumentOutOfRangeException(
            nameof(intent),
            intent,
            "An unexpected intent was provided"
        )
    };

    public static Package GeneratePackage(this Intent intent) => intent switch
    {
        Intent.Approve => Packages.Approval(),
        Intent.Acquire => Packages.Acquisition(),
        Intent.Transfer => Packages.Transfer(),
        Intent.Destroy => Packages.Destruction(),
        _ => throw new ArgumentOutOfRangeException(
            nameof(intent),
            intent,
            "An unexpected intent was provided and no associated package could be found"
        )
    };

    public static Process GenerateProcess(this Package package) => package.Intent switch
    {
        Intent.Approve => Processes.Approval(),
        Intent.Acquire => Processes.Acquisition(),
        Intent.Transfer => Processes.Transfer(),
        Intent.Destroy => Processes.Destruction(),
        _ => throw new ArgumentOutOfRangeException(
            nameof(package.Intent),
            package.Intent,
            "An unexpected intent was provided and no associated process could be found"
        )
    };
}