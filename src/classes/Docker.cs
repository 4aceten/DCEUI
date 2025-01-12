namespace DCEUI.classes;

public class Docker
{
    private readonly string cli_command = "docker";

    private readonly Dictionary<string, string> data_menu_instruction_response = new();

    private readonly OS? os;

    private string? cli_response = "";

    public Docker(OS Os)
    {
        os = Os;
    }

    public string get_cli_command()
    {
        return cli_command;
    }

    public string? get_cli_response()
    {
        return cli_response;
    }

    public OS? get_os_instance()
    {
        return os ?? null;
    }

    public Dictionary<string, string> get_data_menu_instruction_response()
    {
        return data_menu_instruction_response;
    }

    public void clear_data()
    {
        data_menu_instruction_response.Clear();
        cli_response = "";
    }

    private void data_response_parser(string response)
    {
        cli_response = response;

        foreach (var input in response.Split("\n"))
            if (input != "" && input != "\n")
            {
                if (input.Contains(','))
                {
                    var info = input.Split(',');
                    if (!data_menu_instruction_response.ContainsKey(info[0]))
                        data_menu_instruction_response.Add(info[0], info[1]);
                }
                else
                {
                    if (!data_menu_instruction_response.ContainsKey(input))
                        data_menu_instruction_response.Add(input, input);
                }
            }
    }

    public void ssh_container(string id)
    {
        // Start the container (assuming start_container handles starting the container if it's not running)
        start_container(id);

        // Execute the /bin/bash command in the container
        os.run_terminal(cli_command, $@"exec -it {id}");

        // Retrieve the CLI response
        var cliResponse = os.get_os_cli_response();

        // Check if the response contains an error
        if (!string.IsNullOrEmpty(cliResponse) && (cliResponse.Contains("Error") || cliResponse.Contains("error")))
            cli_response = cliResponse; // Store the error response
    }


    public void get_all_docker_containers()
    {
        data_response_parser(os.run_command(cli_command, @"container ls -a --format ""{{.ID}},{{.Names}}"""));
        if (data_menu_instruction_response.Count == 0) cli_response = "Error: Could not find any containers.";
    }

    public void get_all_docker_containers_status_stopped()
    {
        data_response_parser(os.run_command(cli_command,
            @"container ls -a --filter ""status=exited"" --format ""{{.ID}},{{.Names}}"""));
    }

    public void get_all_docker_containers_status_running()
    {
        data_response_parser(os.run_command(cli_command,
            @"container ls -a --filter ""status=running"" --format ""{{.ID}},{{.Names}}"""));
    }

    public void get_all_docker_containers_with_columns()
    {
        data_response_parser(os.run_command(cli_command, @"container ls -a"));
        if (data_menu_instruction_response.Count == 1) cli_response = "Error: Could not find any containers.";
    }

    public void get_all_docker_images()
    {
        data_response_parser(os.run_command(cli_command, @"image ls --format ""{{.ID}},{{.Repository}}"""));

        if (data_menu_instruction_response.Count == 0) cli_response = "Error: Could not find any images.";
    }

    public void get_all_docker_volumes()
    {
        data_response_parser(os.run_command(cli_command, @"volume ls --format ""{{.Name}}"""));
        if (data_menu_instruction_response.Count == 0) cli_response = "Error: Could not find any volumes.";
    }

    public void delete_all_data()
    {
        get_all_docker_containers();

        if (data_menu_instruction_response.Count > 0)
            foreach (var container in data_menu_instruction_response)
                cli_response = os.run_command(cli_command, $"kill container {container.Key}");

        cli_response = os.run_command(cli_command, @"system prune -af --volumes");
    }

    public void inspect(string name)
    {
        cli_response = os.run_command(cli_command, @$"inspect {name}");
    }

    public void start_container(string id)
    {
        cli_response = os.run_command(cli_command, @$"container start {id}");
    }

    public void stop_container(string id)
    {
        cli_response = os.run_command(cli_command, @$"container stop {id}");
    }

    public void restart_container(string id)
    {
        cli_response = os.run_command(cli_command, @$"container restart {id}");
    }

    public void delete_container(string id)
    {
        cli_response = os.run_command(cli_command, @$"container stop {id}");
        cli_response = os.run_command(cli_command, @$"container rm {id}");
    }

    public void delete_image(string id)
    {
        cli_response = os.run_command(cli_command, @$"rmi {id} -f");
    }

    public void delete_volume(string id)
    {
        cli_response = os.run_command(cli_command, @$"volume rm {id} -f");
    }

    public void get_backup_file_list_containers()
    {
        data_response_parser(os.get_all_files_in_application_backup_folder("container"));
    }

    public void get_backup_file_list_images()
    {
        data_response_parser(os.get_all_files_in_application_backup_folder("image"));
    }

    public void commit_container(string name)
    {
        cli_response = os.run_command(cli_command, $"commit {name} {name.ToLower()}-backup");
    }

    public void save_container(string name)
    {
        cli_response = os.run_command(cli_command,
            $@"save {name.ToLower()}-backup -o {os.get_application_data_folder()}/{name.ToLower()}-backup-container.tar");
    }

    public void load_backup_container(string name)
    {
        cli_response = os.run_command(cli_command, $@"load -i {os.get_application_data_folder()}/{name.ToLower()} -q");
    }

    public void run_image(string id)
    {
        cli_response = os.run_command(cli_command, $"run -t -d {id.Replace(".tar", "")}");
    }

    public void save_image(string file_name)
    {
        cli_response = os.run_command(cli_command,
            $"image save -o {os.get_application_data_folder()}/{file_name.ToLower()}-backup-image.tar {file_name}");
    }

    public void restore_image(string file_name)
    {
        cli_response = os.run_command(cli_command, $"image load -i {os.get_application_data_folder()}/{file_name}");
    }

    public void save_volume(string name)
    {
        cli_response = os.run_command(cli_command,
            $@"docker run --rm --volumes-from dbstore -v {Directory.GetCurrentDirectory()}:/backup ubuntu tar cvf /backup/backup.tar /dbdata");
    }

    public void restore_volume(string name)
    {
        cli_response = os.run_command(cli_command,
            "docker run --rm --volumes-from dbstore2 -v $(pwd):/backup ubuntu bash -c $(cd / dbdata && tar xvf / backup / backup.tar--strip 1)");
    }
}