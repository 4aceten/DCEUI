using DCEUI.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCEUI.Tests;

[TestClass]
public class OSTests
{
    private static readonly DI di = new();
    private readonly OS os;

    public OSTests()
    {
        os = di.get_os_instance();
    }

    [TestMethod]
    public void program_pre_checkTest()
    {
        try
        {
            os.program_pre_check();
            Assert.IsTrue(true);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [TestMethod]
    public void run_commandTest()
    {
        var command_response = os.run_command("docker", "-v");
        Assert.IsTrue(command_response != "" && !command_response.Contains("error") &&
                      !command_response.Contains("Error"));
    }

    [TestMethod]
    public void run_terminalTest()
    {
        try
        {
            os.run_terminal("echo", "hello test!");
            Assert.IsTrue(true);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [TestMethod]
    public void open_browserTest()
    {
        try
        {
            os.open_browser("https://google.com");
            Assert.IsTrue(true);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [TestMethod]
    public void setDockerInstanceTest()
    {
        Assert.IsNotNull(os.get_Docker_instance());
    }

    [TestMethod]
    public void get_application_data_folder_pathTest()
    {
        Assert.IsTrue(os.get_application_data_folder().GetType() == typeof(string));
    }

    [TestMethod]
    public void get_all_files_in_application_backup_folderTest()
    {
        Assert.IsTrue(os.get_all_files_in_application_backup_folder().GetType() == typeof(string));
    }

    [TestMethod]
    public void is_supported_platformTest()
    {
        Assert.IsTrue(os.is_supported_platform());
    }

    [TestMethod]
    public void is_docker_installedTest()
    {
        Assert.IsTrue(os.is_docker_installed());
    }

    [TestMethod]
    public void is_docker_currently_running_as_serviceTest()
    {
        Assert.IsTrue(os.is_docker_currently_running_as_service());
    }
}