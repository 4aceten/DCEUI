using DCEUI.classes;

namespace DCEUI.interfaces;

public interface IMenu
{
    List<Menu_Category> menu_category_list { get; set; }
    List<Menu_Item> menu_items_list { get; set; }

    void clear_data();

    void generate_user_selected_category_items_instructions(string user_seluser_selected_menu_category,
        string user_selected_category_item);

    List<string> get_container_menu();
    string get_exit_menu_item();
    List<string> get_general_menu();
    List<string> get_image_menu();
    List<string> get_support_menu();
    List<string>? get_user_selected_category_items(string user_selected_menu_category);
    Dictionary<List<string>, List<string>> get_user_selected_command_instructions();
    string? get_user_selected_command_selection_prompt();
    List<string> get_volume_menu();
}