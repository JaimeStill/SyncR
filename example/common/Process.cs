namespace Common;
public class Process
{
    public string Name { get; set; } = string.Empty;

    public List<ProcessTask> Tasks { get; set; } = new();
}