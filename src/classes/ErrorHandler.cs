using DCEUI.interfaces;

namespace DCEUI.classes
{
    public class ErrorHandler : IErrorHandler
    {
        private int error_message_sleep_time = 8000;

        private ExceptionSettings exception_settings = new ExceptionSettings
        {
            Format = ExceptionFormats.Default,
            Style = new ExceptionStyle
            {
                Exception = new Style().Foreground(Color.Grey),
                Message = new Style().Foreground(Color.White),
                NonEmphasized = new Style().Foreground(Color.Cornsilk1),
                Parenthesis = new Style().Foreground(Color.Cornsilk1),
                Method = new Style().Foreground(Color.Red),
                ParameterName = new Style().Foreground(Color.Cornsilk1),
                ParameterType = new Style().Foreground(Color.Red),
                Path = new Style().Foreground(Color.Red),
                LineNumber = new Style().Foreground(Color.Cornsilk1),
            }
        };

        public void render_error(Exception ex)
        {
            AnsiConsole.WriteException(ex, exception_settings);

            Thread.Sleep(error_message_sleep_time);
        }

        public void render_program_error(Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message);
            AnsiConsole.WriteLine("Exiting program...");


            Thread.Sleep(error_message_sleep_time);
        }
    }
}
