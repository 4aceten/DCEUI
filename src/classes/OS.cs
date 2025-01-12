using System.Diagnostics;
using System.Runtime.InteropServices;
using DCEUI.interfaces;

namespace DCEUI.classes;

public class OS : IOS
{
    private string application_data_folder_path = "";
    public string application_name = "DCrane";
    private Docker? docker;
    private string os_cli_response = "";

    public OS()
    {
        try
        {
            program_pre_check();
            create_application_data_folder();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public string get_application_data_folder()
    {
        return application_data_folder_path;
    }

    public Docker? get_Docker_instance()
    {
        return docker;
    }

    public void set_Docker_instance(Docker docker)
    {
        this.docker = docker;
    }


    public void program_pre_check()
    {
        if (!is_supported_platform())
            throw new SystemException("The application does not support your Operating System.");

        if (!is_docker_installed()) throw new SystemException("Docker CLI needs to be installed.");

        if (!is_docker_currently_running_as_service())
            throw new SystemException("Docker CLI needs to be running as a service.");
    }

    public bool is_supported_platform()
    {
        if (Is_windows() || Is_macos() || Is_gnu_linux()) return true;

        return false;
    }

    public bool is_docker_installed()
    {
        var response = run_command("docker", "-v");
        return !string.IsNullOrEmpty(response) && !response.ToLower().Contains("error");
    }

    public bool is_docker_currently_running_as_service()
    {
        var result = run_command("docker", "info");
        ;
        // If the command succeeds, Docker is running
        return !string.IsNullOrEmpty(result) && !result.ToLower().Contains("error");
    }

    public string get_all_files_in_application_backup_folder(string file_extention = "")
    {
        if (string.IsNullOrEmpty(application_data_folder_path))
            throw new Exception("Application data folder path is not set.");

        return string.Join("\n", Directory.EnumerateFiles(
            application_data_folder_path,
            "*" + file_extention + "*.tar",
            SearchOption.AllDirectories
        ));
    }

    public string run_command(string command, string args)
    {
        // Debugging
        // Console.WriteLine($"Running command with args: {command} {args}");
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Read output and error streams synchronously
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            // Optional timeout handling
            if (!process.WaitForExit(10000)) // Wait up to 10 seconds
                throw new Exception("The process did not finish within the timeout period.");

            os_cli_response = error; // Save error response for later access

            // Return output or error
            return string.IsNullOrEmpty(error) ? output : error;
        }
        catch (Exception ex)
        {
            throw new Exception($"Command execution failed: {ex.Message}");
        }
    }


    public void run_terminal(string command, string args)
    {
        try
        {
            // Debugging
            // Console.WriteLine($"Running command with args: {command} {args}");

            var fileName = "";
            var arguments = "";

            if (Is_windows())
            {
                fileName = "powershell.exe"; // Use PowerShell as the default shell
                arguments = $"-Command \"{command} {args}\""; // Adjust for PowerShell syntax
            }
            else
            {
                fileName = "/bin/sh"; // Default to Bash for Linux/Unix containers
                arguments = $"-c \"{command} {args} {fileName}\"";
            }

            Console.WriteLine($"Running {fileName} {arguments}");

            // Initialize the process
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false // Use process APIs instead of shell
                }
            };

            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void open_browser(string url)
    {
        try
        {
            if (Is_windows())
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}")
                    { CreateNoWindow = true });
            else if (Is_macos())
                Process.Start("open", url);
            else if (Is_gnu_linux())
                Process.Start("xdg-open", url);
            else
                throw new Exception("Unsupported operating system for opening browser.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to open browser: {ex.Message}");
        }
    }

    public static bool Is_windows()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    public static bool Is_macos()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }

    public static bool Is_gnu_linux()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

    public string get_os_cli_response()
    {
        return os_cli_response;
    }

    private void create_application_data_folder()
    {
        try
        {
            application_data_folder_path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                application_name
            );

            Directory.CreateDirectory(application_data_folder_path);
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Failed to create application data folder at '{application_data_folder_path}': {ex.Message}");
        }
    }


    public void delete_backup_file(string file_name)
    {
        try
        {
            foreach (var file in Directory.EnumerateFiles(
                         application_data_folder_path,
                         "*" + file_name + "*",
                         SearchOption.AllDirectories
                     ))
                File.Delete(file);
        }
        catch (IOException ex)
        {
            throw new Exception($"Failed to delete backup file: {ex.Message}");
        }
    }
}