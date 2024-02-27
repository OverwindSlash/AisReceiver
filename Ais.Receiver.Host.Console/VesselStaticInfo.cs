namespace Ais.Receiver.Host.Console
{
    internal class VesselStaticInfo
    {
        public string code => "aisStatic";

        public string id { get; set; }
        public string mmsi { get; set; }
        public string imo { get; set; }
        public string callSign { get; set; }
        public string shipName { get; set; }
        public int shipType { get; set; }
        public string a { get; set; }
        public string b { get; set; }
        public string c { get; set; }
        public string d { get; set; }
        public string len { get; set; }
        public string wid { get; set; }
        public string high { get; set; }
        public string draft { get; set; }
        public string rot { get; set; }
        public string eta { get; set; }
        public string dest { get; set; }
        public string time { get; set; }

        public int enc { get; set; }
        public string msgId { get; set; }
        public string msg { get; set; }
        public string from { get; set; }

        public VesselStaticInfo()
        {
            time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }
    }
}
