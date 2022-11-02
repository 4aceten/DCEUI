using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCEUI.classes;

namespace DCEUI.Tests
{
    [TestClass()]
    public class DITests
    {
        DI di = new DI();

        public DITests()
        {
            Assert.IsNotNull(di.get_os_instance());
            Assert.IsNotNull(di.get_menu_instance());
            Assert.IsNotNull(di.get_docker_instance());
            Assert.IsNotNull(di.get_errorhandler_instance());
            Assert.IsNotNull(di.get_commandlinehandler_instance());
        }

        [TestMethod()]
        public void DITest()
        {
            Assert.IsNotNull(di);
        }

        [TestMethod()]
        public void OsTest()
        {
            Assert.IsNotNull(di.get_os_instance());
        }

        [TestMethod()]
        public void MenuTest()
        {
            Assert.IsNotNull(di.get_menu_instance());
        }

        [TestMethod()]
        public void DockerTest()
        {
            Assert.IsNotNull(di.get_docker_instance());
        }

        [TestMethod()]
        public void CommandLineHandlerTest()
        {
            Assert.IsNotNull(di.get_os_instance());

            Assert.IsNotNull(di.get_menu_instance());

            Assert.IsNotNull(di.get_docker_instance());

            Assert.IsNotNull(di.get_errorhandler_instance());

            Assert.IsNotNull(di.get_commandlinehandler_instance());
        }

        [TestMethod()]
        public void ErrorHandlerTest()
        {
            Assert.IsNotNull(di.get_errorhandler_instance());
        }
    }
}