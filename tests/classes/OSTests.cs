using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCEUI.classes;

namespace DCEUI.Tests
{
    [TestClass()]
    public class OSTests
    {
        static DI di = new DI();
        OS os;

        public OSTests()
        {
            this.os = di.get_os_instance();
        }

        [TestMethod()]
        public void program_pre_checkTest()
        {
            try
            {
                this.os.program_pre_check();
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void run_commandTest()
        {
            string command_response = this.os.run_command("docker", "-v");
            Assert.IsTrue(command_response != "" && !command_response.Contains("error") && !command_response.Contains("Error"));
        }

        [TestMethod()]
        public void run_terminalTest()
        {
            try
            {
                this.os.run_terminal("echo", "hello test!");
                Assert.IsTrue(true);
            } catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void open_browserTest()
        {
            try
            {
                this.os.open_browser("https://google.com");
                Assert.IsTrue(true);
            } catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void setDockerInstanceTest()
        {
            Assert.IsNotNull(this.os.get_Docker_instance());    
        }

        [TestMethod()]
        public void get_application_data_folder_pathTest()
        {
            Assert.IsTrue((this.os.get_application_data_folder().GetType() == typeof(string)));
        }

        [TestMethod()]
        public void get_all_files_in_application_backup_folderTest()
        {
            Assert.IsTrue((this.os.get_all_files_in_application_backup_folder().GetType() == typeof(string)));
        }

        [TestMethod()]
        public void is_supported_platformTest()
        {
            Assert.IsTrue(this.os.is_supported_platform());
        }

        [TestMethod()]
        public void is_docker_installedTest()
        {
            Assert.IsTrue(this.os.is_docker_installed());
        }

        [TestMethod()]
        public void is_docker_currently_running_as_serviceTest()
        {
            Assert.IsTrue(this.os.is_docker_currently_running_as_service());
        }
    }
}