using DCEUI.classes;

namespace DCEUI.interfaces;

public interface IOS
{
    string get_all_files_in_application_backup_folder(string file_extention);
    string get_application_data_folder();
    Docker? get_Docker_instance();
    bool is_docker_currently_running_as_service();
    bool is_docker_installed();
    bool is_supported_platform();
    void open_browser(string url);
    void program_pre_check();
    string run_command(string command, string args);
    void run_terminal(string command, string args);
    void set_Docker_instance(Docker docker);
}