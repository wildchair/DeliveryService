using DeliveryService.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace DeliveryClient.Utils
{
    public static class Configurator
    {
        public static Config LoadXmlConfig()
        {
            var serializer = new XmlSerializer(typeof(Config));
            using FileStream file = new("config.xml", FileMode.Open);
            var conf = serializer.Deserialize(file) as Config;
            return conf;
        }
        public static void SaveXmlConfig(Config conf)
        {
            var serializer = new XmlSerializer(typeof(Config));
            using FileStream file = new("config.xml", FileMode.Create);
            serializer.Serialize(file, conf);
        }
        public class Config
        {
            public string ServerAddress;
            public int ServerPort;
            public Order[] Orders;
        }
    }
}
