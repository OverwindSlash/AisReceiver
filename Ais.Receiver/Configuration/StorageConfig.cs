namespace Ais.Receiver.Configuration
{
    public class StorageConfig
    {
        public bool EnableCapture { get; set; }
        public string StaticFile { get; set; }
        public string DynamicFile { get; set; }
        public string TrackMmsi { get; set; }
        public string TrackFile { get; set; }
    }
}
