namespace DCEUI.interfaces;

public interface IMenu_Item
{
    List<string> commandlinehandler_instructions { get; set; }
    List<string>? docker_instructions { get; set; }
    int id { get; set; }
    string name { get; set; }
    string? selection_prompt { get; set; }
}