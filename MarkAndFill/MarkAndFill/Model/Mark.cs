namespace MarkAndFill.Model;

public class Mark
{
    public Mark(string tagName, string tagValue)
    {
        TagName = tagName;
        TagValue = tagValue;
    }

    public Mark(string tagName)
    {
        TagName = tagName;
    }

    public string TagName { get; set; }
    public string? TagValue { get; set; }
}