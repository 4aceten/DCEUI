using DCEUI.classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCEUI.Tests;

[TestClass]
public class DockerTests
{
    private static readonly DI di = new();

    private readonly Docker docker;

    private string container_id = "";
    private string image_id = "";
    private string volume_id = "";

    public DockerTests()
    {
        docker = di.get_docker_instance();
    }

    private void create_container_for_test()
    {
        var output = docker.get_os_instance()
            .run_command(docker.get_cli_command(), "run -d echokrist/dceui-test-container:1.0.0");
        container_id = output.Trim();
    }

    private void pull_image_for_test()
    {
        docker.get_os_instance().run_command(docker.get_cli_command(), "pull echokrist/dceui-test-container:1.0.0");
        var output = docker.get_os_instance()
            .run_command(docker.get_cli_command(), "images -q echokrist/dceui-test-container:1.0.0");
        image_id = output.Trim();
    }

    [TestMethod]
    public void DockerTest()
    {
        Assert.IsNotNull(docker.get_os_instance());
    }

    [TestMethod]
    public void get_all_docker_containersTest()
    {
        create_container_for_test();
        docker.get_all_docker_containers();
        docker.delete_container(container_id);
        Assert.IsTrue(docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) &&
                      docker.get_data_menu_instruction_response().Count >= 0);
    }

    [TestMethod]
    public void get_all_docker_imagesTest()
    {
        pull_image_for_test();
        docker.get_all_docker_images();
        docker.delete_image(image_id);
        Assert.IsTrue(docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) &&
                      docker.get_data_menu_instruction_response().Count >= 0);
    }

    [TestMethod]
    public void get_all_docker_volumesTest()
    {
        create_container_for_test();
        docker.get_all_docker_volumes();
        docker.delete_container(container_id);
        Assert.IsTrue(docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) &&
                      docker.get_data_menu_instruction_response().Count >= 0);
    }

    [TestMethod]
    public void delete_containerTest()
    {
        create_container_for_test();
        docker.delete_container(container_id);
        Assert.IsTrue(!docker.get_cli_response().Contains("Error"));
    }

    [TestMethod]
    public void delete_imageTest()
    {
        create_container_for_test();
        docker.stop_container(container_id);
        image_id = docker.get_os_instance().run_command(
            docker.get_cli_command(),
            @$"container inspect --format='{{{{.Image}}}}' {container_id}"
        ).Trim();
        volume_id = docker.get_os_instance().run_command(
            docker.get_cli_command(),
            @$"container inspect --format=""{{{{range .Mounts}}}}{{{{.Name}}}}{{{{end}}}}"" {container_id}"
        ).Trim();
        docker.delete_container(container_id);
        docker.delete_image(image_id);
        docker.delete_volume(volume_id);
        Assert.IsTrue(!docker.get_cli_response().Contains("Error"));
    }

    [TestMethod]
    public void delete_volumeTest()
    {
        create_container_for_test();
        docker.stop_container(container_id);

        volume_id = docker.get_os_instance().run_command(
            docker.get_cli_command(),
            @$"container inspect --format=""{{{{range .Mounts}}}}{{{{.Name}}}}{{{{end}}}}"" {container_id}"
        ).Trim();
        docker.delete_container(container_id);
        docker.delete_volume(volume_id);
        Assert.IsTrue(!docker.get_cli_response().Contains("Error"));
    }

    [TestMethod]
    public void get_backup_file_listTest()
    {
        docker.get_backup_file_list_images();
        Assert.IsTrue(!docker.get_cli_response().Contains("Error"));
    }

    [TestMethod]
    public void clear_dataTest()
    {
        docker.clear_data();

        Assert.IsTrue(docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) &&
                      docker.get_data_menu_instruction_response().Count == 0);

        Assert.AreEqual("", docker.get_cli_response());
    }
}