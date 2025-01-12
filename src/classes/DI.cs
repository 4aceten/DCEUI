using classes;
using DCEUI.interfaces;

namespace DCEUI.classes;

public class DI : IDI
{
    private readonly CommandLineHandler commandLineHandler;
    private readonly Docker docker;
    private readonly ErrorHandler errorHandler;
    private readonly Menu menu;
    private readonly OS os;

    public DI()
    {
        os = Os();
        docker = Docker();
        os.set_Docker_instance(docker);
        menu = Menu();
        errorHandler = ErrorHandler();
        commandLineHandler = CommandLineHandler();
    }

    public OS Os()
    {
        var Os = new OS();
        return Os;
    }

    public Docker Docker()
    {
        var docker = new Docker(os);
        return docker;
    }

    public Menu Menu()
    {
        var menu = new Menu();
        return menu;
    }

    public ErrorHandler ErrorHandler()
    {
        var errorHandler = new ErrorHandler();
        return errorHandler;
    }

    public CommandLineHandler CommandLineHandler()
    {
        var commandLineHandler = new CommandLineHandler(os, menu, docker, errorHandler);
        return commandLineHandler;
    }


    public OS get_os_instance()
    {
        return os;
    }

    public Docker get_docker_instance()
    {
        return docker;
    }

    public Menu get_menu_instance()
    {
        return menu;
    }

    public ErrorHandler get_errorhandler_instance()
    {
        return errorHandler;
    }

    public CommandLineHandler get_commandlinehandler_instance()
    {
        return commandLineHandler;
    }
}