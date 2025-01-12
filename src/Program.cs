using DCEUI.classes;

var di = new DI();

try
{
    di.get_commandlinehandler_instance().execute_program();
}
catch (Exception ex)
{
    di.get_errorhandler_instance().render_program_error(ex);
    Environment.Exit(0);
}