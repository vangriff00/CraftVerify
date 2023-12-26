using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestProject
{
    [TestClass]
    public class ConfigServiceShould
    {
        [TestMethod]
        public void LoadAllConfigsFromFile()
        {
            // Arrange
            var fqfn = "c:\\TestProject\\config.local.txt";
            var delimiter = ":=";

            // Act
            var configs = ConfigService.GetConfigs<AppConfig>(fqfn, delimiter);


            // Assert
            Assert.IsNotNull(configs);
            Assert.IsTrue(configs.ConnectionString == "helllooo");
            Assert.IsTrue(configs.Pi == 3.14159);
            Assert.IsTrue(configs.MaxRetryCount == 100);

        }
    }
