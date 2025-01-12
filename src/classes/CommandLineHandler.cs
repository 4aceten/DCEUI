using System.Reflection;
using DCEUI.classes;

namespace classes;

/**
 * * Class: CommandLineHandler
 * *
 * * Description: Handles the entire life cycle of the console application.
 * *
 * *
 */
public class CommandLineHandler
{
    private const int gui_minimum_required_pagesize = 50;
    private readonly Docker? Docker;
    private readonly ErrorHandler errorHandler;
    private readonly Menu menu;
    private readonly OS os;
    private readonly Dictionary<string, string> user_selected_menu_docker_data_items = new();
    private MethodInfo? mi;
    private string user_selected_category_item = "";

    private string user_selected_menu_category = "";

    public CommandLineHandler(OS os, Menu menu, Docker docker, ErrorHandler errorHandler)
    {
        this.os = os;
        this.menu = menu;
        Docker = docker;
        this.errorHandler = errorHandler;
    }

    /**
     * * Function: execute_program
     * *
     * * Description: Executes all the functions that renders the program UI.
     * *
     * *
     */
    public void execute_program()
    {
        render_header_ui();

        render_main_gui();

        execute_instructions(menu.get_user_selected_command_instructions());
    }


    /**
     * Function: render_header_ui
     *
     * Description: Renders the header UI - the top part of the console application
     * consisting of application name, author and copyright.
     */
    public void render_header_ui()
    {
        AnsiConsole.Write(
            new FigletText("DCEUI")
                .LeftJustified()
                .Color(Color.Red)
        );

        AnsiConsole.WriteLine("©echokrist 2022 - " + DateTime.Now.ToString("yyyy"));
    }


    /**
     * * Function: render_main_gui
     * *
     * * Description: Renders the main menu UI - the rest of the UI of the console application
     * *              consisting of the selection menu with command actions such as General, container etc...
     * *
     * *
     */
    public void render_main_gui()
    {
        render_category_ui();
        render_selected_category_sub_menu();
    }


    /**
     * * Function: render_category_ui
     * *
     * * Description: Renders the menu consisting of a selection prompt for categories,
     * *              Example: General, Container, Image etc...
     * *
     * *
     */
    public void render_category_ui()
    {
        var rule = new Rule();
        rule.Justify(Justify.Left);
        rule.Style = Style.Parse(Color.Red.ToString());
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine("");
        var selectionprompt_pagesize = menu.menu_category_list.Count() >= 3
            ? menu.menu_category_list.Count() * gui_minimum_required_pagesize
            : gui_minimum_required_pagesize;

        user_selected_menu_category = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select command category:")
                .AddChoices(
                    menu.menu_category_list.ElementAt(0).name,
                    menu.menu_category_list.ElementAt(1).name,
                    menu.menu_category_list.ElementAt(2).name,
                    menu.menu_category_list.ElementAt(3).name,
                    menu.menu_category_list.ElementAt(4).name,
                    menu.menu_category_list.ElementAt(6).name
                )
                .PageSize(selectionprompt_pagesize)
        );

        if (user_selected_menu_category == menu.get_exit_menu_item()) exit_program();
    }

    /**
     * Function: render_back_ui_button
     *
     * Description: Renders a back button selection prompt that will redirect
     * back to the main menu once selected.
     */
    public void render_back_ui_button()
    {
        var back_selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("")
                .AddChoices(
                    menu.get_back_menu_item()
                )
        );

        redirect_back_to_main_menu_ui();
    }

    /**
     * * Function: render_selected_category_sub_menu
     * *
     * * Description: Renders the sub menu based on main menu selection choice,
     * *              Example: Start, Stop, Delete etc...
     * *
     * *
     */
    public void render_selected_category_sub_menu()
    {
        List<string> user_selected_category_items = menu.get_user_selected_category_items(user_selected_menu_category);

        var selectionprompt_pagesize = user_selected_category_items.Count() >= 3
            ? user_selected_category_items.Count() * gui_minimum_required_pagesize
            : gui_minimum_required_pagesize;

        user_selected_category_item = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select command to execute:")
                .AddChoices(user_selected_category_items.ToArray())
                .PageSize(selectionprompt_pagesize)
        );


        menu.generate_user_selected_category_items_instructions(user_selected_menu_category,
            user_selected_category_item);

        if (menu.get_user_selected_command_instructions() == null)
            error_structure(new Exception(
                $@"Error {user_selected_menu_category + " " + user_selected_category_item} failed. Redirecting to main menu..."));
    }


    /**
     * * Function: generate_prompt
     * *
     * * Description: Generate and renders the different SpectreConsole prompts based on
     * *              Menu_Item class selection_prompt property.
     * *
     * *
     * *
     */
    public void generate_prompt()
    {
        dynamic? selection_prompt = null;

        if (menu.get_user_selected_command_selection_prompt() == "generate_multi_selection_prompt")
            selection_prompt = generate_multiselection_prompt();
        else if (menu.get_user_selected_command_selection_prompt() == "generate_single_selection_prompt")
            selection_prompt = generate_single_selection_prompt();
        else
            error_structure(null);

        Dictionary<string, string> selectionList = new();

        foreach (var selectionData in Docker.get_data_menu_instruction_response())
        {
            selectionList.Add(selectionData.Key, selectionData.Value);
            selection_prompt.AddChoice(selectionData.Value);
        }

        if (selectionList.Count == 0) error_structure(null);

        // Show the prompt
        var userSelectedData = AnsiConsole.Prompt(selection_prompt);


        if (userSelectedData == null) error_structure(null);

        var break_first_loop = false;
        foreach (var selection in userSelectedData)
        {
            if (break_first_loop) break;

            foreach (var selectionListItems in selectionList)
                if (menu.get_user_selected_command_selection_prompt() == "generate_multi_selection_prompt")
                {
                    if (selectionListItems.Value == selection &&
                        !user_selected_menu_docker_data_items.ContainsKey(selectionListItems.Key))
                        user_selected_menu_docker_data_items.Add(selectionListItems.Key, selectionListItems.Value);
                }
                else if (menu.get_user_selected_command_selection_prompt() == "generate_single_selection_prompt")
                {
                    user_selected_menu_docker_data_items.Add(selectionListItems.Key, selectionListItems.Value);
                    break_first_loop = true;
                    break;
                }
        }

        if (user_selected_menu_docker_data_items == null) error_structure(null);
    }


    /**
     * * Function: render_docker_output_data
     * *
     * * Description: Renders the clean output of Docker CLI commands.
     * *
     * *
     * *
     */
    public void render_docker_output_data()
    {
        foreach (var data in Docker.get_data_menu_instruction_response()) AnsiConsole.WriteLine(data.Value);

        render_back_ui_button();
    }


    /**
     * * Function: render_docker_inspect_data
     * *
     * * Description: Renders the clean output of Docker CLI inspect commands.
     * *
     * *
     * *
     */
    public void render_docker_inspect_data()
    {
        AnsiConsole.WriteLine(Docker.get_cli_response());

        render_back_ui_button();
    }


    /**
     * * Function: generate_single_selection_prompt
     * *
     * * Description: Generates single selection prompt based on user selected main menu selection.
     * *
     * *
     * *
     */
    private SelectionPrompt<string> generate_single_selection_prompt()
    {
        var selectionPrompt = new SelectionPrompt<string>();
        selectionPrompt.Title($"Select {user_selected_menu_category}:");
        selectionPrompt.MoreChoicesText("[grey](Move up and down to reveal more)[/]");

        return selectionPrompt;
    }


    /**
     * * Function: generate_multiselection_prompt
     * *
     * * Description: Generates multi selection prompt based on user selected main menu selection.
     * *
     * *
     * *
     */
    private MultiSelectionPrompt<string> generate_multiselection_prompt()
    {
        var multiSelectionPrompt = new MultiSelectionPrompt<string>();

        multiSelectionPrompt.Title($"Select {user_selected_menu_category}:");
        multiSelectionPrompt.Required();
        multiSelectionPrompt.MoreChoicesText("[grey](Move up and down to reveal more)[/]");
        multiSelectionPrompt.InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle, " +
            "[green]<enter>[/] to accept)[/]"
        );

        return multiSelectionPrompt;
    }


    /**
     * * Function: execute_instructions
     * *
     * * Description: Calls the helper function by passing the methods set in the the selected menu item properties called docker_instructions and
     * *              commandlinehandler_instructions while handling the different outputs/results.
     * *
     * *
     * *
     */
    public void execute_instructions(
        Dictionary<List<string>, List<string>> user_selected_docker_commandlinehandler_command_instructions)
    {
        var executionCompleted = false;

        foreach (var instruction_lists in user_selected_docker_commandlinehandler_command_instructions)
        {
            if (user_selected_docker_commandlinehandler_command_instructions.Count != 0 &&
                user_selected_docker_commandlinehandler_command_instructions.ContainsKey(instruction_lists.Key))
                foreach (var instruction_set in instruction_lists.Key)
                    try
                    {
                        execute_command_function(instruction_set, Docker);
                        if (Docker.get_data_menu_instruction_response().Count == 0 ||
                            Docker.get_cli_response().Contains("Error"))
                            error_structure(new Exception(
                                $@"Data missing in order to execute {user_selected_menu_category} {user_selected_category_item}... Redirecting to main menu."));
                    }
                    catch (Exception ex)
                    {
                        error_structure(null);
                    }

            if (user_selected_docker_commandlinehandler_command_instructions.Values != null &&
                user_selected_docker_commandlinehandler_command_instructions.ContainsValue(instruction_lists.Value))
                foreach (var instruction_set in instruction_lists.Value)
                    try
                    {
                        if (menu.get_user_selected_command_selection_prompt() != "")
                            execute_command_function("generate_prompt", this);
                        execute_command_function(instruction_set, this);
                        executionCompleted = true;
                    }
                    catch (Exception ex)
                    {
                        error_structure(null);
                    }
        }

        AnsiConsole.WriteLine("");

        if (executionCompleted)
            AnsiConsole.WriteLine(user_selected_menu_category + "" + user_selected_category_item +
                                  " execution completed. Redirecting to main menu...");

        redirect_back_to_main_menu_ui();
    }


    /**
     * * Function: execute_command_function
     * *
     * * Description: Calls the passed function string with correct object instance of classes.
     * *              Example: Docker class get_all_docker_containers would be function="get_all_docker_containers"
     * *              and objectInstance = this.Docker. Which is the dependency injected instance of the Docker class.
     * *
     * *
     * *
     */
    private void execute_command_function(string function, object? objectInstance = null)
    {
        //Get the method information using the method info class
        objectInstance = objectInstance ?? this;

        var thisType = objectInstance.GetType();
        mi = thisType.GetMethod(function);

        if (mi == null) error_structure(null);


        mi.Invoke(objectInstance, null);

        if (Docker.get_cli_response() == null) throw new Exception(@$"Error: {function} could not execute.");
    }


    /*
     * Function: ssh_container
     *
     * Description: Renders the UI for SSH session and calls the terminal session instance to do so.
     *
     *
     **/
    public void ssh_container()
    {
        AnsiConsole.WriteLine(@$"SSH into container {user_selected_menu_docker_data_items.ElementAt(0).Value}");
        AnsiConsole.WriteLine("Type exit and press enter to exit the SSH session.");

        try
        {
            Docker.ssh_container(user_selected_menu_docker_data_items.ElementAt(0).Key);
        }
        catch (Exception ex)
        {
            error_structure(ex);
        }
    }


    /**
     * * Function: inspect
     * *
     * * Description: Renders the Docker inspect UI with the data.
     * *
     * *
     * *
     */
    public void inspect()
    {
        Docker.inspect(user_selected_menu_docker_data_items.ElementAt(0).Value);
        render_docker_inspect_data();
    }

    /**
   * * Function: inspect
   * *
   * * Description: Renders the Docker inspect UI with the data for images.
   * *
   * *
   * *
   */
    public void inspect_images()
    {
        Docker.inspect(user_selected_menu_docker_data_items.ElementAt(0).Key);
        render_docker_inspect_data();
    }

    /**
     * * Function: delete_all_data_from_docker
     * *
     * * Description: Renders the Docker inspect UI with the data.
     * *
     * *
     * *
     */
    public void delete_all_data_from_docker()
    {
        var confirmation = AnsiConsole.Confirm("Are you sure you want to delete ALL DOCKER DATA?!");
        if (!confirmation) re_render_application();

        Docker.delete_all_data();
    }


    /**
     * * Function: process_multi_user_selected_docker_data
     * *
     * * Description: Handles all the sub menu selected actions.
     * *              Example: Start, Stop etc...
     * *
     * *
     * *
     */
    public void process_multi_user_selected_docker_data()
    {
        try
        {
            AnsiConsole.Status()
                .Start(user_selected_category_item + " " + user_selected_menu_category, ctx =>
                {
                    foreach (var user_selected_docker_data in user_selected_menu_docker_data_items)
                    {
                        if (user_selected_category_item == "Start")
                            switch (user_selected_menu_category)
                            {
                                case "Container":

                                    Docker.start_container(user_selected_docker_data.Key);

                                    break;

                                case "Image":
                                    Docker.run_image(user_selected_docker_data.Key);
                                    break;

                                default:
                                    error_structure(null);
                                    break;
                            }

                        if (user_selected_category_item == "Stop")
                            switch (user_selected_menu_category)
                            {
                                case "Container":

                                    Docker.stop_container(user_selected_docker_data.Key);

                                    break;

                                default:
                                    error_structure(null);
                                    break;
                            }

                        if (user_selected_category_item == "Restart")
                            switch (user_selected_menu_category)
                            {
                                case "Container":

                                    Docker.restart_container(user_selected_docker_data.Key);

                                    break;

                                default:
                                    error_structure(null);
                                    break;
                            }

                        if (user_selected_category_item == "Backup")
                            switch (user_selected_menu_category)
                            {
                                case "Container":

                                    Docker.commit_container(user_selected_docker_data.Value);
                                    Docker.save_container(user_selected_docker_data.Value);

                                    break;

                                case "Image":
                                    Docker.save_image(user_selected_docker_data.Value);
                                    break;

                                default:
                                    error_structure(null);
                                    break;
                            }
                        else if (user_selected_category_item == "Restore")
                            switch (user_selected_menu_category)
                            {
                                case "Container":
                                    Docker.load_backup_container(user_selected_docker_data.Key);
                                    Docker.run_image(user_selected_docker_data.Key);

                                    break;

                                case "Image":
                                    Docker.restore_image(user_selected_docker_data.Value);
                                    break;

                                default:
                                    error_structure(null);
                                    break;
                            }

                        else if (user_selected_category_item == "Delete backup")
                            switch (user_selected_menu_category)
                            {
                                case "Container":
                                    os.delete_backup_file(user_selected_docker_data.Key);
                                    break;

                                case "Image":
                                    os.delete_backup_file(user_selected_docker_data.Key);

                                    break;

                                case "Volume":
                                    os.delete_backup_file(user_selected_docker_data.Key);

                                    break;

                                default:
                                    error_structure(null);
                                    break;
                            }

                        else if (user_selected_category_item == "Delete")
                            switch (user_selected_menu_category)
                            {
                                case "Container":
                                    Docker.delete_container(user_selected_docker_data.Key);
                                    break;

                                case "Image":
                                    Docker.delete_image(user_selected_docker_data.Key);

                                    break;

                                case "Volume":
                                    Docker.delete_volume(user_selected_docker_data.Key);

                                    break;

                                default:
                                    error_structure(null);
                                    break;
                            }
                    }
                });
        }
        catch (Exception ex)
        {
            error_structure(ex);
        }
    }


    /**
     * * Function: report_issue
     * *
     * * Description: Calls the OS open browsers function with the URL for
     * *              the Github Repo Issues tab.
     * *
     * *
     * *
     */
    public bool report_issue()
    {
        os.open_browser("https://github.com/kristian-n-a/DCEUI/issues");
        return true;
    }


    /**
     * * Function: buy_me_a_coffee
     * *
     * * Description: Calls the OS open browsers function with the URL for
     * *              the authors ko-fi page where one can donate in order to support this project and
     * *              future endeavours.
     * *
     * *
     */
    public bool buy_me_a_coffee()
    {
        os.open_browser("https://ko-fi.com/echokrist");
        return true;
    }


    /**
     * * Function: clean_data_for_re_render
     * *
     * * Description: Resets the different data properties in order to re-render the UI.
     * *
     * *
     */
    public void clean_data_for_re_render()
    {
        // CommandLineHandler
        user_selected_menu_category = "";
        user_selected_category_item = "";
        user_selected_menu_docker_data_items.Clear();

        // Docker
        Docker.clear_data();

        // Menu
        menu.clear_data();

        // Clear GUI
        AnsiConsole.Clear();
    }


    /**
     * * Function: error_structure
     * *
     * * Description: Handles the error method calls.
     * *
     * *
     */
    private void error_structure(Exception ex)
    {
        if (ex == null)
        {
            AnsiConsole.WriteLine("");
            errorHandler.render_error(new Exception(
                $@"Error {user_selected_menu_category + " " + user_selected_category_item} failed. Redirecting to main menu..."));
        }
        else
        {
            errorHandler.render_error(ex);
        }

        re_render_application();
    }


    /**
     * * Function: re_render_application
     * *
     * * Description: Calls the necesarry methods in order to re-render the application.
     * *
     * *
     */
    private void re_render_application()
    {
        clean_data_for_re_render();

        execute_program();
    }


    /**
     * * Function: redirect_back_to_main_menu_ui
     * *
     * * Description: Renders the redirecting text prompt and calls the re-rendering of the application.
     * *
     * *
     */
    private void redirect_back_to_main_menu_ui()
    {
        re_render_application();
    }


    /**
     * * Function: exit_program
     * *
     * * Description: Renders the exit text prompt and kills the application process.
     * *
     * *
     */
    private void exit_program()
    {
        AnsiConsole.WriteLine("Exited program.");
        Environment.Exit(0);
    }
}