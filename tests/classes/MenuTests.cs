using DCEUI.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCEUI.Tests;

[TestClass]
public class MenuTests
{
    private readonly DI di = new();

    private readonly Menu menu;

    public MenuTests()
    {
        menu = di.get_menu_instance();
    }

    [TestMethod]
    public void MenuTest()
    {
        Assert.IsNotNull(menu.menu_category_list);
        Assert.AreNotEqual(0, menu.menu_items_list);
    }

    [TestMethod]
    public void get_general_menuTest()
    {
        Assert.IsNotNull(menu.get_general_menu());
    }

    [TestMethod]
    public void get_container_menuTest()
    {
        Assert.IsNotNull(menu.get_container_menu());
    }

    [TestMethod]
    public void get_image_menuTest()
    {
        Assert.IsNotNull(menu.get_image_menu());
    }

    [TestMethod]
    public void get_volume_menuTest()
    {
        Assert.IsNotNull(menu.get_volume_menu());
    }

    [TestMethod]
    public void get_support_menuTest()
    {
        Assert.IsNotNull(menu.get_support_menu());
    }

    [TestMethod]
    public void get_user_selected_category_itemsTest()
    {
        var user_selected_menu_category = "General";
        Assert.IsNotNull(menu.get_user_selected_category_items(user_selected_menu_category));
    }

    [TestMethod]
    public void generate_user_selected_category_items_instructionsTest()
    {
        menu.generate_user_selected_category_items_instructions("Container", "SSH");
        Assert.IsTrue(menu.get_user_selected_command_selection_prompt().GetType() == typeof(string));
        Assert.IsTrue(
            menu.get_user_selected_command_instructions().GetType() == typeof(Dictionary<List<string>, List<string>>) &&
            menu.get_user_selected_command_instructions().Count > 0);
    }

    [TestMethod]
    public void get_user_selected_command_instructionsTest()
    {
        Assert.IsTrue(menu.get_user_selected_command_instructions().GetType() ==
                      typeof(Dictionary<List<string>, List<string>>));
    }

    [TestMethod]
    public void get_user_selected_command_selection_promptTest()
    {
        Assert.IsTrue(menu.get_user_selected_command_selection_prompt().GetType() == typeof(string));
    }

    [TestMethod]
    public void clear_dataTest()
    {
        menu.clear_data();

        Assert.IsTrue(
            menu.get_user_selected_command_instructions().GetType() == typeof(Dictionary<List<string>, List<string>>) &&
            menu.get_user_selected_command_instructions().Count == 0);
        Assert.IsTrue(menu.get_user_selected_command_selection_prompt().GetType() == typeof(string) &&
                      menu.get_user_selected_command_selection_prompt() == "");
    }
}