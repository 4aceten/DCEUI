using DCEUI.interfaces;

namespace DCEUI.classes;

public class Menu_Category : IMenu_Category
{
    public int id { get; set; }
    public string name { get; set; }

    public List<Menu_Item> items { get; set; }
}

public class Menu_Item : IMenu_Item
{
    public int id { get; set; }
    public string name { get; set; }
    public List<string>? docker_instructions { get; set; }
    public string? selection_prompt { get; set; }
    public List<string> commandlinehandler_instructions { get; set; }
}

public class Menu : IMenu
{
    private readonly List<string> menu_back = new();

    private readonly List<string> menu_container = new();

    private readonly List<string> menu_exit = new();

    private readonly List<string> menu_general = new();

    private readonly List<string> menu_image = new();

    private readonly List<string> menu_support = new();

    private readonly List<string> menu_volume = new();

    private readonly Dictionary<List<string>, List<string>> user_selected_command_instructions = new();

    public int menu_items_count;

    private string user_selected_command_selection_prompt = "";

    public Menu()
    {
        menu_category_list = new List<Menu_Category>();

        var menu_id = 0;
        var item_id = 0;

        menu_category_list.Add(
            new Menu_Category
            {
                id = menu_id++,
                name = "General",
                items = new List<Menu_Item>
                {
                    new()
                    {
                        id = item_id++, name = "Delete All(containers, images, volumes)",
                        commandlinehandler_instructions = new List<string> { "delete_all_data_from_docker" }
                    },
                    new() { id = item_id++, name = "Back" }
                }
            });

        menu_category_list.Add(
            new Menu_Category
            {
                id = menu_id++,
                name = "Container",
                items = new List<Menu_Item>
                {
                    new()
                    {
                        id = item_id++, name = "SSH",
                        docker_instructions = new List<string> { "get_all_docker_containers" },
                        selection_prompt = "generate_single_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "ssh_container" }
                    },
                    new()
                    {
                        id = item_id++, name = "List all containers",
                        docker_instructions = new List<string> { "get_all_docker_containers_with_columns" },
                        commandlinehandler_instructions = new List<string> { "render_docker_output_data" }
                    },

                    new()
                    {
                        id = item_id++, name = "Inspect",
                        docker_instructions = new List<string> { "get_all_docker_containers" },
                        selection_prompt = "generate_single_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "inspect" }
                    },
                    new()
                    {
                        id = item_id++, name = "Start",
                        docker_instructions = new List<string> { "get_all_docker_containers_status_stopped" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Stop",
                        docker_instructions = new List<string> { "get_all_docker_containers_status_running" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Restart",
                        docker_instructions = new List<string> { "get_all_docker_containers_status_running" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },

                    new()
                    {
                        id = item_id++, name = "Backup",
                        docker_instructions = new List<string> { "get_all_docker_containers" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Restore",
                        docker_instructions = new List<string> { "get_backup_file_list_containers" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Delete backup",
                        docker_instructions = new List<string> { "get_backup_file_list_containers" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Delete",
                        docker_instructions = new List<string> { "get_all_docker_containers" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new() { id = item_id++, name = "Back" }
                }
            });
        menu_category_list.Add(
            new Menu_Category
            {
                id = menu_id++,
                name = "Image",
                items = new List<Menu_Item>
                {
                    new()
                    {
                        id = item_id++, name = "List all images",
                        docker_instructions = new List<string> { "get_all_docker_images" }, selection_prompt = "",
                        commandlinehandler_instructions = new List<string> { "render_docker_output_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Inspect",
                        docker_instructions = new List<string> { "get_all_docker_images" },
                        selection_prompt = "generate_single_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "inspect_images" }
                    },

                    new()
                    {
                        id = item_id++, name = "Backup",
                        docker_instructions = new List<string> { "get_all_docker_images" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Restore",
                        docker_instructions = new List<string> { "get_backup_file_list_images" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Delete backup",
                        docker_instructions = new List<string> { "get_backup_file_list_images" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Delete",
                        docker_instructions = new List<string> { "get_all_docker_images" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new() { id = item_id++, name = "Back" }
                }
            });

        menu_category_list.Add(
            new Menu_Category
            {
                id = menu_id++,
                name = "Volume",
                items = new List<Menu_Item>
                {
                    new()
                    {
                        id = item_id++, name = "List all volumes",
                        docker_instructions = new List<string> { "get_all_docker_volumes" }, selection_prompt = "",
                        commandlinehandler_instructions = new List<string> { "render_docker_output_data" }
                    },
                    new()
                    {
                        id = item_id++, name = "Inspect",
                        docker_instructions = new List<string> { "get_all_docker_volumes" },
                        selection_prompt = "generate_single_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "inspect" }
                    },

                    new()
                    {
                        id = item_id++, name = "Delete",
                        docker_instructions = new List<string> { "get_all_docker_volumes" },
                        selection_prompt = "generate_multi_selection_prompt",
                        commandlinehandler_instructions = new List<string> { "process_multi_user_selected_docker_data" }
                    },
                    new() { id = item_id++, name = "Back" }
                }
            });

        menu_category_list.Add(
            new Menu_Category
            {
                id = menu_id++,
                name = "Support",
                items = new List<Menu_Item>
                {
                    new()
                    {
                        id = item_id++, name = "Report Issues",
                        commandlinehandler_instructions = new List<string> { "report_issue" }
                    },
                    new()
                    {
                        id = item_id++, name = "Buy me a coffee",
                        commandlinehandler_instructions = new List<string> { "buy_me_a_coffee" }
                    },
                    new() { id = item_id++, name = "Back" }
                }
            });

        menu_category_list.Add(
            new Menu_Category
            {
                id = menu_id++,
                name = "Back"
            }
        );

        menu_category_list.Add(
            new Menu_Category
            {
                id = menu_id++,
                name = "Exit"
            }
        );


        generate_menu_list();
    }

    public List<Menu_Category> menu_category_list { get; set; }
    public List<Menu_Item> menu_items_list { get; set; }

    public List<string> get_general_menu()
    {
        return menu_general;
    }

    public List<string> get_container_menu()
    {
        return menu_container;
    }

    public List<string> get_image_menu()
    {
        return menu_image;
    }

    public List<string> get_volume_menu()
    {
        return menu_volume;
    }

    public List<string> get_support_menu()
    {
        return menu_support;
    }

    public string get_exit_menu_item()
    {
        return menu_exit[0];
    }

    public Dictionary<List<string>, List<string>> get_user_selected_command_instructions()
    {
        return user_selected_command_instructions;
    }

    public string? get_user_selected_command_selection_prompt()
    {
        return user_selected_command_selection_prompt;
    }

    public List<string>? get_user_selected_category_items(string user_selected_menu_category)
    {
        foreach (var menu_category in menu_category_list)
            if (menu_category.name == user_selected_menu_category)
                switch (menu_category.id)
                {
                    case 0:
                        return get_general_menu();
                        break;

                    case 1:
                        return get_container_menu();
                        break;

                    case 2:
                        return get_image_menu();
                        break;

                    case 3:
                        return get_volume_menu();
                        break;

                    case 4:
                        return get_support_menu();
                        break;
                    case 5:
                    case 6:
                        return null;
                        break;
                }

        return null;
    }

    public void generate_user_selected_category_items_instructions(string user_seluser_selected_menu_category,
        string user_selected_category_item)
    {
        foreach (var menu_category in menu_category_list)
            if (menu_category.name == user_seluser_selected_menu_category)
                foreach (var menu_items in menu_category.items)
                    if (menu_items.name == user_selected_category_item)
                    {
                        user_selected_command_selection_prompt = menu_items.selection_prompt ?? "";

                        user_selected_command_instructions.TryAdd(menu_items.docker_instructions ?? new List<string>(),
                            menu_items.commandlinehandler_instructions ?? new List<string>());
                    }
    }

    public void clear_data()
    {
        user_selected_command_instructions.Clear();
        user_selected_command_selection_prompt = "";
    }

    public string get_back_menu_item()
    {
        return menu_back[0];
    }

    private void generate_menu_list()
    {
        foreach (var menu in menu_category_list)
            switch (menu.id)
            {
                case 0:
                    foreach (var menu_item in menu.items)
                    {
                        menu_general.Add(menu_item.name);
                        menu_items_count++;
                    }

                    break;

                case 1:
                    foreach (var menu_item in menu.items)
                    {
                        menu_container.Add(menu_item.name);
                        menu_items_count++;
                    }

                    break;

                case 2:
                    foreach (var menu_item in menu.items)
                    {
                        menu_image.Add(menu_item.name);
                        menu_items_count++;
                    }

                    break;

                case 3:
                    foreach (var menu_item in menu.items)
                    {
                        menu_volume.Add(menu_item.name);
                        menu_items_count++;
                    }

                    break;

                case 4:
                    foreach (var menu_item in menu.items)
                    {
                        menu_support.Add(menu_item.name);
                        menu_items_count++;
                    }

                    break;

                case 5:
                    menu_back.Add(menu_category_list[5].name);
                    menu_items_count++;
                    break;

                case 6:
                    menu_exit.Add(menu_category_list[6].name);
                    menu_items_count++;
                    break;
            }
    }
}