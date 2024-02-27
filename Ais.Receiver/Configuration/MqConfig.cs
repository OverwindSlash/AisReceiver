namespace Ais.Receiver.Configuration
{
    public class MqConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string StaticTopic { get; set; }
        public string DynamicTopic { get; set; }
    }
}
