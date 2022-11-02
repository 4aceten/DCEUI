using classes;
using DCEUI.classes;

namespace DCEUI.interfaces
{
    public interface IDI
    {
        CommandLineHandler CommandLineHandler();
        Docker Docker();
        ErrorHandler ErrorHandler();
        CommandLineHandler get_commandlinehandler_instance();
        Docker get_docker_instance();
        ErrorHandler get_errorhandler_instance();
        Menu get_menu_instance();
        OS get_os_instance();
        Menu Menu();
        OS Os();
    }
}