using classes;
using DCEUI.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;

namespace DCEUI.Tests;

[TestClass]
public class CommandLineHandlerTests
{
    private readonly CommandLineHandler commandLineHandler;
    private readonly DI di = new();

    public CommandLineHandlerTests()
    {
        commandLineHandler = di.get_commandlinehandler_instance();
    }

    [TestMethod]
    public void CommandLineHandlerTest()
    {
        Assert.IsNotNull(di.get_os_instance());

        Assert.IsNotNull(di.get_menu_instance());

        Assert.IsNotNull(di.get_docker_instance());

        Assert.IsNotNull(di.get_errorhandler_instance());
    }

    [TestMethod]
    public void render_header_uiTest()
    {
        AnsiConsole.Record();

        commandLineHandler.render_header_ui();

        var cli_text_output = AnsiConsole.ExportText();

        Assert.AreNotEqual("", cli_text_output);
    }

    [TestMethod]
    public void report_issueTest()
    {
        Assert.IsTrue(commandLineHandler.report_issue());
    }

    [TestMethod]
    public void buy_me_a_coffeeTest()
    {
        Assert.IsTrue(commandLineHandler.buy_me_a_coffee());
    }
}