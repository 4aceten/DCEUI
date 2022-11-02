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

    public static bool Is_windows() => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

    public static bool Is_macos() => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

    public static bool Is_gnu_linux() => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);


    public OS()
    {
        try
        {
            this.program_pre_check();
            this.create_application_data_folder();
        }
        catch (Exception)
        {
            throw new Exception("Could not create application data folder.");
        }
    }

    public string get_application_data_folder()
    {
        return this.application_data_folder_path;
    }

    public Docker? get_Docker_instance()
    {
        return this.docker ?? null;
    }

    public void set_Docker_instance(Docker docker)
    {
        this.docker = docker;
    }

    public string get_os_cli_response()
    {
        return this.os_cli_response;
    }

    private void create_application_data_folder()
    {
        try
        {
            var result = Directory.CreateDirectory(application_data_folder_path);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void program_pre_check()
    {
        try
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
        catch (Exception ex)
        {
            throw;
        }
    }

    public bool is_supported_platform()
    {
        if (Is_windows())
        {
            application_data_folder_path = Directory.GetCurrentDirectory() + "\\data";
            return true;
        }
        else if (Is_macos())
        {
            application_data_folder_path = Directory.GetCurrentDirectory() + "/data";
            return true;
        }
        else if (Is_gnu_linux())
        {
            application_data_folder_path = Directory.GetCurrentDirectory() + "/data";
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool is_docker_installed()
    {
        string response = this.run_command("docker", "-v");
        if (response.Contains("error") || response.Contains("Command not found"))
        {
            return false;
        }

        return true;
    }

    public bool is_docker_currently_running_as_service()
    {
        var processes = Process.GetProcessesByName("Docker").ToArray();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (processes.Length > 0 && processes.GetValue(0).ToString().Contains("docker"))
        {
            return true;
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return false;
    }

    public string get_all_files_in_application_backup_folder(string file_extention = "")
    {
        string folderFiles = "";

        foreach (string file in Directory.EnumerateFiles(
            this.application_data_folder_path,
            "*",
            SearchOption.AllDirectories)
            )
        {
            if (file is not null && file.Contains(file_extention) && file.EndsWith(".tar"))
            {
                folderFiles += file.Substring(application_data_folder_path.Length + 1) + "\n";
            }
        }

        return folderFiles;
    }

    public string run_command(string command, string args)
    {
        var process = new Process()
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

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        this.os_cli_response = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (string.IsNullOrEmpty(error)) { return output; }
        else { return error; }
    }

    public void run_terminal(string command, string args)
    {
        string terminal_application = "";
        string terminal_arguments = "";

        if (OS.Is_windows())
        {
            terminal_application = "cmd.exe";
            terminal_arguments = @$"/C {command + " " + args}";
        }
        else
        {
            terminal_application = "/bin/bash";
            terminal_arguments = $@"-c ""{command + " " + args}""";
        }

        try
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = terminal_application,
                    Arguments = terminal_arguments,
                }
            };

            // Mac specific requirements.
            if (OS.Is_macos())
            {
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.UseShellExecute = true;
            }

            // TODO - Figure out how to make standard error work on MacOS and possibly Linux?
            if (!OS.Is_macos())
            {
                this.os_cli_response = process.StandardError.ReadToEnd() ?? "";
            }

            process.Start();
            process.WaitForExit();
            process.Close();
        }
        catch (Exception)
        {
            throw;
        }

    }

    public void delete_backup_file(string file_name)
    {
        try
        {
            foreach (string file in Directory.EnumerateFiles(
            this.application_data_folder_path,
            "*",
            SearchOption.AllDirectories)
            )
            {
                if (file is not null && file.EndsWith(file_name))
                {
                    File.Delete(file);
                }
            }
        }
        catch (IOException)
        {
            throw;
        }
    }

    public void open_browser(string url)
    {
        dynamic process;
        try
        {
            if (OS.Is_windows())
            {
                url = url.Replace("&", "^&");
                process = Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                process.Close();
            }
            else if (OS.Is_macos())
            {
                process = Process.Start("open", url);
                process.Close();
            }
            else if (OS.Is_gnu_linux())
            {
                process = Process.Start("open", url);
                process.Close();
            }
            else
            {
                throw new Exception("Looks like I'm not getting paid...");
            }
        }
        catch (Exception)
        {
            throw;
        }

        process.Close();
    }
}
