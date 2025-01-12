using DCEUI.classes;

namespace DCEUI.interfaces;

public interface IMenu_Category
{
    int id { get; set; }
    List<Menu_Item> items { get; set; }
    string name { get; set; }
}