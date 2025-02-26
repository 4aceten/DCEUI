﻿using DCEUI.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCEUI.Tests;

[TestClass]
public class Menu_CategoryTests
{
    private readonly Menu_Category menu_Category = new();

    [TestMethod]
    public void Menu_CategoryTest()
    {
        menu_Category.id = 1;
        menu_Category.name = "name";
        menu_Category.items = new List<Menu_Item>
        {
            new()
            {
                id = 01, name = "Delete All(containers, images, volumes)",
                docker_instructions = new List<string> { "get_all_docker_containers" },
                commandlinehandler_instructions = new List<string> { "delete_all_data_from_docker" }
            }
        };

        Assert.IsNotNull(menu_Category.id);
        Assert.IsNotNull(menu_Category.name);
        Assert.IsNotNull(menu_Category.items);
    }
}