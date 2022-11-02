using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCEUI.classes;

namespace DCEUI.Tests
{
    [TestClass()]
    public class DockerTests
    {
        static DI di = new DI();

        Docker docker;

        string container_id = "";
        string image_id = "";
        string volume_id = "";

        public DockerTests()
        {
            this.docker = di.get_docker_instance();
        }

        private void create_container_for_test()
        {
            this.container_id = this.docker.get_os_instance().run_command(this.docker.get_cli_command(), "run -d redis").Split("\n")[0];
        }

        private void pull_image_for_test()
        {
            this.docker.get_os_instance().run_command(this.docker.get_cli_command(), "pull redis");
            this.image_id = this.docker.get_os_instance().run_command(this.docker.get_cli_command(), "images -q redis").Split("\n")[0];
        }

        [TestMethod()]
        public void DockerTest()
        {
            Assert.IsNotNull(this.docker.get_os_instance());
        }

        [TestMethod()]
        public void get_all_docker_containersTest()
        {
            this.create_container_for_test();
            this.docker.get_all_docker_containers();
            this.docker.delete_container(this.container_id);
            Assert.IsTrue((this.docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) && this.docker.get_data_menu_instruction_response().Count >= 0));
        }

        [TestMethod()]
        public void get_all_docker_imagesTest()
        {
            this.pull_image_for_test();
            this.docker.get_all_docker_images();
            this.docker.delete_image(this.image_id);
            Assert.IsTrue((this.docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) && this.docker.get_data_menu_instruction_response().Count >= 0));
        }

        [TestMethod()]
        public void get_all_docker_volumesTest()
        {
            this.create_container_for_test();
            this.docker.get_all_docker_volumes();
            this.docker.delete_container(this.container_id);
            Assert.IsTrue((this.docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) && this.docker.get_data_menu_instruction_response().Count >= 0));
        }

        [TestMethod()]
        public void delete_containerTest()
        {
            this.create_container_for_test();
            this.docker.delete_container(this.container_id);
            Assert.IsTrue(!this.docker.get_cli_response().Contains("Error") && !this.docker.get_cli_response().Contains("Error"));
        }

        //[TestMethod()]
        //public void delete_imageTest()
        //{
        //    this.pull_image_for_test();
        //    this.docker.delete_image(this.image_id);
        //    Assert.IsTrue(!this.docker.cli_response.Contains("Error") && !this.docker.cli_response.Contains("Error"));
        //}

        [TestMethod()]
        public void delete_volumeTest()
        {
            try
            {
                this.create_container_for_test();
                this.docker.stop_container(this.container_id);
                this.volume_id = this.docker.get_os_instance().run_command(this.docker.get_cli_command(), @$"inspect--format = {{.Id}} {this.container_id}");
                this.docker.delete_volume(this.volume_id);
                this.docker.delete_container(this.container_id);
                Assert.IsTrue(!this.docker.get_cli_response().Contains("Error") && !this.docker.get_cli_response().Contains("Error"));
            } catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }


        }

        [TestMethod()]
        public void get_backup_file_listTest()
        {
            this.docker.get_backup_file_list_images();
            Assert.IsTrue(!this.docker.get_cli_response().Contains("Error") && !this.docker.get_cli_response().Contains("Error"));
        }

        [TestMethod()]
        public void clear_dataTest()
        {
            this.docker.clear_data();

            Assert.IsTrue((this.docker.get_data_menu_instruction_response().GetType() == typeof(Dictionary<string, string>) && this.docker.get_data_menu_instruction_response().Count == 0));

            Assert.AreEqual("", this.docker.get_cli_response());
        }
    }
}