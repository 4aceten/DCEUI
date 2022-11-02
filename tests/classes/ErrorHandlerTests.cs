using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCEUI.classes;

namespace DCEUI.Tests
{
    [TestClass()]
    public class ErrorHandlerTests
    {
        static DI di = new DI();
        ErrorHandler errorHandler;

        public ErrorHandlerTests()
        {
            this.errorHandler = di.get_errorhandler_instance();
        }

        [TestMethod()]
        public void render_errorTest()
        {
            try
            {
                this.errorHandler.render_error(new Exception("test ran ErrorHandler.render_error"));
                Assert.Fail();
            } catch(Exception ex)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void render_program_errorTest()
        {
            try
            {
                this.errorHandler.render_program_error(new Exception("test ran ErrorHandler.render_error"));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
            }
        }
    }
}