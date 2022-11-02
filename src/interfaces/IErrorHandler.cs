namespace DCEUI.interfaces
{
    public interface IErrorHandler
    {
        void render_error(Exception ex);
        void render_program_error(Exception ex);
    }
}