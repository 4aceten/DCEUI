using System.Diagnostics;
using System.Runtime.InteropServices;
using DCEUI.interfaces;

namespace DCEUI.classes;

public class OS : IOS
{
    public string application_name = "DCrane";
    private string application_data_folder_path = "";
    private string os_cli_response = "";
    private Docker? docker = null;

    public static bool Is_windows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    public static bool Is_macos() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    public static bool Is_gnu_linux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

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

    public string get_application_data_folder() => this.application_data_folder_path;

    public Docker? get_Docker_instance() => this.docker;

    public void set_Docker_instance(Docker docker) => this.docker = docker;

    public string get_os_cli_response() => this.os_cli_response;

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
            throw new Exception($"Failed to create application data folder at '{application_data_folder_path}': {ex.Message}");
        }
    }


    public void program_pre_check()
    {
        if (!is_supported_platform())
        {
            throw new SystemException("The application does not support your Operating System.");
        }

        if (!is_docker_installed())
        {
            throw new SystemException("Docker CLI needs to be installed.");
        }

        if (!is_docker_currently_running_as_service())
        {
            throw new SystemException("Docker CLI needs to be running as a service.");
        }
    }

    public bool is_supported_platform()
    {
        if (Is_windows() || Is_macos() || Is_gnu_linux())
        {
            return true;
        }
        return false;
    }

    public bool is_docker_installed()
    {
        string response = this.run_command("docker", "-v");
        return !string.IsNullOrEmpty(response) && !response.ToLower().Contains("error");
    }

    public bool is_docker_currently_running_as_service()
    {
        try
        {
            // Attempt to run a simple Docker CLI command
            var result = run_command("docker", "info");;

            // If the command succeeds, Docker is running
            return !string.IsNullOrEmpty(result) && !result.ToLower().Contains("error");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Docker service check failed: {ex.Message}");
            return false;
        }
    }

    public string get_all_files_in_application_backup_folder(string file_extention = "")
    {
        if (string.IsNullOrEmpty(application_data_folder_path))
        {
            throw new Exception("Application data folder path is not set.");
        }

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
                    CreateNoWindow = true,
                }
            };

            process.Start();

            // Read output and error streams synchronously
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            // Optional timeout handling
            if (!process.WaitForExit(10000)) // Wait up to 10 seconds
            {
                throw new Exception("The process did not finish within the timeout period.");
            }

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
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Is_windows() ? "cmd.exe" : "/bin/bash",
                    Arguments = Is_windows() ? $"/C \"{command} {args}\"" : $"-c \"{command} {args}\"",
                    RedirectStandardOutput = Is_windows(),
                    RedirectStandardError = Is_windows(),
                    UseShellExecute = !Is_windows(),
                    CreateNoWindow = Is_windows()
                }
            };
            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to run terminal command: {ex.Message}");
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
            {
                File.Delete(file);
            }
        }
        catch (IOException ex)
        {
            throw new Exception($"Failed to delete backup file: {ex.Message}");
        }
    }

    public void open_browser(string url)
    {
        try
        {
            if (Is_windows())
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });
            }
            else if (Is_macos())
            {
                Process.Start("open", url);
            }
            else if (Is_gnu_linux())
            {
                Process.Start("xdg-open", url);
            }
            else
            {
                throw new Exception("Unsupported operating system for opening browser.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to open browser: {ex.Message}");
        }
    }
}
