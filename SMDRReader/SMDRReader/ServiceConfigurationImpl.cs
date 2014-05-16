using System.Configuration;

namespace SMDRReader
{
    internal class ServiceConfigurationImpl : IServiceConfiguration
    {
        public ServiceConfigurationImpl()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Port"]))
            {
                Port = 1150;
            }
            else
            {
                Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            }
        }
        public int Port { get; set; }
    }
}
