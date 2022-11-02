using classes;
using DCEUI.interfaces;

namespace DCEUI.classes
{
    public class DI : IDI
    {
        private OS os;
        private Docker docker;
        private Menu menu;
        private ErrorHandler errorHandler;
        private CommandLineHandler commandLineHandler;

        public DI()
        {
            this.os = Os();
            this.docker = Docker();
            this.os.set_Docker_instance(this.docker);
            this.menu = Menu();
            this.errorHandler = ErrorHandler();
            this.commandLineHandler = CommandLineHandler();
        }

        public OS Os()
        {
            OS Os = new OS();
            return Os;
        }

        public Docker Docker()
        {
            Docker docker = new Docker(os);
            return docker;
        }

        public Menu Menu()
        {
            Menu menu = new Menu();
            return menu;
        }

        public ErrorHandler ErrorHandler()
        {
            ErrorHandler errorHandler = new ErrorHandler();
            return errorHandler;
        }

        public CommandLineHandler CommandLineHandler()
        {
            CommandLineHandler commandLineHandler = new CommandLineHandler(os, menu, docker, errorHandler);
            return commandLineHandler;
        }


        public OS get_os_instance()
        {
            return this.os;
        }

        public Docker get_docker_instance()
        {
            return this.docker;
        }

        public Menu get_menu_instance()
        {
            return this.menu;
        }

        public ErrorHandler get_errorhandler_instance()
        {
            return this.errorHandler;
        }

        public CommandLineHandler get_commandlinehandler_instance()
        {
            return this.commandLineHandler;
        }
    }
}
