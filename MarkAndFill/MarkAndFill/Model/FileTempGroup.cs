namespace MarkAndFill.Model;

public class FileTempGroup
{
    public FileTempGroup(string groupName)
    {
        GroupName = groupName;
    }

    private FileTempGroup()
    {
    }

    public static FileTempGroup BaseGroup => new() { GroupName = "Без группы" };

    public string GroupName { get; set; } = string.Empty;
}