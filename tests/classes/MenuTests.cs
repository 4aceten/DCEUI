using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCEUI.classes;

namespace DCEUI.Tests
{
    [TestClass()]
    public class MenuTests
    {
        DI di = new DI();

        Menu menu;

        public MenuTests()
        {
            this.menu = di.get_menu_instance();
        }

        [TestMethod()]
        public void MenuTest()
        {
            Assert.IsNotNull(this.menu.menu_category_list);
            Assert.AreNotEqual(0, this.menu.menu_items_list);
        }

        [TestMethod()]
        public void get_general_menuTest()
        {
            Assert.IsNotNull(this.menu.get_general_menu());
        }

        [TestMethod()]
        public void get_container_menuTest()
        {
            Assert.IsNotNull(this.menu.get_container_menu());
        }

        [TestMethod()]
        public void get_image_menuTest()
        {
            Assert.IsNotNull(this.menu.get_image_menu());
        }

        [TestMethod()]
        public void get_volume_menuTest()
        {
            Assert.IsNotNull(this.menu.get_volume_menu());
        }

        [TestMethod()]
        public void get_support_menuTest()
        {
            Assert.IsNotNull(this.menu.get_support_menu());
        }

        [TestMethod()]
        public void get_user_selected_category_itemsTest()
        {
            string user_selected_menu_category = "General";
            Assert.IsNotNull(this.menu.get_user_selected_category_items(user_selected_menu_category));
        }

        [TestMethod()]
        public void generate_user_selected_category_items_instructionsTest()
        {
            this.menu.generate_user_selected_category_items_instructions("Container", "SSH");
            Assert.IsTrue((this.menu.get_user_selected_command_selection_prompt().GetType() == typeof(string)));
            Assert.IsTrue((this.menu.get_user_selected_command_instructions().GetType() == typeof(Dictionary<List<string>, List<string>>) && this.menu.get_user_selected_command_instructions().Count > 0));
        }

        [TestMethod()]
        public void get_user_selected_command_instructionsTest()
        {
            Assert.IsTrue((this.menu.get_user_selected_command_instructions().GetType() == typeof(Dictionary<List<string>, List<string>>)));
        }

        [TestMethod()]
        public void get_user_selected_command_selection_promptTest()
        {
            Assert.IsTrue((this.menu.get_user_selected_command_selection_prompt().GetType() == typeof(string)));
        }

        [TestMethod()]
        public void clear_dataTest()
        {
            this.menu.clear_data();

            Assert.IsTrue((this.menu.get_user_selected_command_instructions().GetType() == typeof(Dictionary<List<string>, List<string>>) && this.menu.get_user_selected_command_instructions().Count == 0));
            Assert.IsTrue((this.menu.get_user_selected_command_selection_prompt().GetType() == typeof(string) && this.menu.get_user_selected_command_selection_prompt() == ""));
        }
    }
}