namespace Ais.Receiver.Configuration
{
    public class MqConfig
    {
        public bool EnableMessageQueue { get; set; }
        public string Host { get; set; }
        public string StaticTopic { get; set; }
        public string DynamicTopic { get; set; }
    }
}
