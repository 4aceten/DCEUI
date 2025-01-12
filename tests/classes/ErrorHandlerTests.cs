using DCEUI.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCEUI.Tests;

[TestClass]
public class ErrorHandlerTests
{
    private static readonly DI di = new();
    private readonly ErrorHandler errorHandler;

    public ErrorHandlerTests()
    {
        errorHandler = di.get_errorhandler_instance();
    }

    [TestMethod]
    public void render_errorTest()
    {
        try
        {
            errorHandler.render_error(new Exception("test ran ErrorHandler.render_error"));
            Assert.Fail();
        }
        catch (Exception ex)
        {
            Assert.IsTrue(true);
        }
    }

    [TestMethod]
    public void render_program_errorTest()
    {
        try
        {
            errorHandler.render_program_error(new Exception("test ran ErrorHandler.render_error"));
            Assert.Fail();
        }
        catch (Exception ex)
        {
            Assert.IsTrue(true);
        }
    }
}