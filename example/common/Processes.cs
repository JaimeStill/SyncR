namespace Common;
public static class Processes
{
    public static Process Approval() =>
        new()
        {
            Name = "Approval Processor",
            Tasks = GenerateTasks()
        };

    public static Process Acquisition() =>
        new()
        {
            Name = "Acquisition Processor",
            Tasks = GenerateTasks()
        };

    public static Process Transfer() =>
        new()
        {
            Name = "Transfer Processor",
            Tasks = GenerateTasks()
        };

    public static Process Destruction() =>
        new()
        {
            Name = "Destruction Processor",
            Tasks = GenerateTasks()
        };

    static List<ProcessTask> GenerateTasks() => new()
    {
        new()
        {
            Name = "Security Review",
            Section = "Cybersecurity",
            Step = 1,
            Duration = 800
        },
        new()
        {
            Name = "Legal Review",
            Section = "Legal",
            Step = 2,
            Duration = 1100
        },
        new()
        {
            Name = "Informal Review",
            Section = "Review Board",
            Step = 3,
            Duration = 500
        },
        new()
        {
            Name = "Formal Review",
            Section = "Senior Leadership",
            Step = 4,
            Duration = 1800
        },
        new()
        {
            Name = "Final Approval",
            Section = "Chief Executives",
            Step = 5,
            Duration = 1000
        }
    };
}