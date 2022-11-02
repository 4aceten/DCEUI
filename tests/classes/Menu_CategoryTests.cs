using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCEUI.classes;

namespace DCEUI.Tests
{
    [TestClass()]
    public class Menu_CategoryTests
    {
        Menu_Category menu_Category = new Menu_Category();

        [TestMethod()]
        public void Menu_CategoryTest()
        {
            menu_Category.id = 1;
            menu_Category.name = "name";
            menu_Category.items = new List<Menu_Item>() { new Menu_Item() { id = 01, name = "Delete All(containers, images, volumes)", docker_instructions = new List<string> { "get_all_docker_containers" }, commandlinehandler_instructions = new List<string> { "delete_all_data_from_docker" } } };

            Assert.IsNotNull(menu_Category.id);
            Assert.IsNotNull(menu_Category.name);
            Assert.IsNotNull(menu_Category.items);
        }
    }
}