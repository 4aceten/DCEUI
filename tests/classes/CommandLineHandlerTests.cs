using classes;
using DCEUI.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;
using Spectre.Console.Testing;
using static System.Net.Mime.MediaTypeNames;
using System;
using Spectre.Console.Advanced;

namespace DCEUI.Tests
{
    [TestClass()]
    public class CommandLineHandlerTests
    {
        DI di = new DI();

        CommandLineHandler commandLineHandler;

        public CommandLineHandlerTests()
        {
            this.commandLineHandler = di.get_commandlinehandler_instance();
        }

        [TestMethod()]
        public void CommandLineHandlerTest()
        {
            Assert.IsNotNull(di.get_os_instance());

            Assert.IsNotNull(di.get_menu_instance());

            Assert.IsNotNull(di.get_docker_instance());

            Assert.IsNotNull(di.get_errorhandler_instance());
        }

        [TestMethod()]
        public void render_header_uiTest()
        {
            AnsiConsole.Record();

            this.commandLineHandler.render_header_ui();

            string cli_text_output = AnsiConsole.ExportText();

            Assert.AreNotEqual("", cli_text_output);
            Assert.IsTrue((cli_text_output.Contains("©Kristian-n-a 2022 - 2022")));
        }

        [TestMethod()]
        public void report_issueTest()
        {
            Assert.IsTrue(this.commandLineHandler.report_issue());
        }

        [TestMethod()]
        public void buy_me_a_coffeeTest()
        {
            Assert.IsTrue(this.commandLineHandler.buy_me_a_coffee());
        }
    }
}