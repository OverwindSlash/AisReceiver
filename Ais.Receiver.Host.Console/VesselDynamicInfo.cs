namespace Ais.Receiver.Host.Console
{
    internal class VesselDynamicInfo
    {
        public string code => "aisDymamic";

        public string id { get; set; }
        public string mmsi { get; set; }
        public string lon { get; set; }
        public string lat { get; set; }
        public string sailStatus { get; set; }
        public string hdg { get; set; }
        public string cog { get; set; }
        public string speed { get; set; }
        public string rot { get; set; }
        public string time { get; set; }

        public int enc { get; set; }
        public string msgId { get; set; }
        public string msg { get; set; }
        public string from { get; set; }

        public string sourceIp { get; set; }
        public string BSName { get; set; }

        public VesselDynamicInfo()
        {
            time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }
    }
}
