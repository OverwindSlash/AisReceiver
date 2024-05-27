using Ais.Models;
using Ais.Models.Abstractions;
using Ais.Receiver.Configuration;
using Ais.Receiver.Host.Console;
using Ais.Receiver.Receiver;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.Text.Json;

// Main function
AisConfig? aisConfig = GetAisConfiguration();
MqConfig? mqConfig = GetMqConfiguration();
StorageConfig? storageConfig = GetStorageConfiguration();

ReceiverHost? receiver = CreateReceiver(aisConfig);
IProducer<string, string> producer = CreateMqProducer(mqConfig);

StreamWriter staticWriter = CreateStaticLogWriter(storageConfig);
StreamWriter dynamicWriter = CreateDynamicLogWriter(storageConfig);
StreamWriter trackWriter = CreateTrackJsonWriter(storageConfig);

// Json options
var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
};

// Track options
int id = 0;
int interval = 1;
string target = storageConfig.TrackMmsi;
var tracks = new List<Location>();

receiver.Messages.Subscribe(async msg =>
{
    bool isStatic = false;
    string json = string.Empty;
    switch (msg.MessageType)
    {
        // For static info:
        case 5:
            isStatic = true;
            json = HandleType5Message(msg);
            break;
        case 24:
            isStatic = true;
            json = HandleType24Message(msg);
            break;

        // For dynamic info:
        case 1:
        case 2:
        case 3:
            json = HandleType1To3Message(msg);
            break;
        case 18:
            json = HandleType18Message(msg);
            break;
        case 19:
            json = HandleType19Message(msg);
            break;
        default:
            break;
    }

    if (mqConfig.EnableMessageQueue)
    {
        await PublishMessage(json, isStatic, producer);
    }
});

receiver.Sentences.Subscribe(async sentence =>
{
    Console.WriteLine(sentence);
});

await receiver.StartAsync(new CancellationTokenSource().Token);

if (producer != null)
{
    producer.Dispose();
}
staticWriter.Close();
staticWriter.Dispose();
dynamicWriter.Close();
dynamicWriter.Dispose();

string tracksJson = JsonSerializer.Serialize(tracks);
trackWriter.WriteLine(tracksJson);
trackWriter.Close();
trackWriter.Dispose();





// Helpers

AisConfig? GetAisConfiguration()
{
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("settings.json", true, true)
        .Build();

    return config.GetSection("Ais").Get<AisConfig>();
}

MqConfig? GetMqConfiguration()
{
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("settings.json", true, true)
        .Build();

    return config.GetSection("MessageQueue").Get<MqConfig>();
}

StorageConfig? GetStorageConfiguration()
{
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("settings.json", true, true)
        .Build();

    return config.GetSection("Storage").Get<StorageConfig>();
}

ReceiverHost? CreateReceiver(AisConfig? config)
{
    INmeaReceiver fileReceiver = (INmeaReceiver)new FileStreamNmeaReceiver(config.TestFile, config.TestFileLineInterval);

    if (!mqConfig.EnableMessageQueue && storageConfig.EnableCapture)
    {
        fileReceiver = (INmeaReceiver)new FileStreamNmeaReceiver(config.TestFile);
    }

    var receiver = config.TestMode
        ? fileReceiver
        : new NetworkStreamNmeaReceiver(
            config.Host, config.Port,
            config.RetryPeriodicity, config.RetryAttempts);

    return new ReceiverHost(receiver);
}

IProducer<string, string> CreateMqProducer(MqConfig? mqConfig1)
{
    if (!mqConfig1.EnableMessageQueue)
    {
        return null;
    }

    var config = new ProducerConfig
    {
        BootstrapServers = $"{mqConfig1.Host}:{mqConfig1.Port}", // Kafka broker地址
    };
    var producer1 = new ProducerBuilder<string, string>(config).Build();
    return producer1;
}

StreamWriter CreateStaticLogWriter(StorageConfig? config)
{
    if (File.Exists(config.StaticFile))
    {
        File.Delete(config.StaticFile);
    }

    return File.AppendText(config.StaticFile);
}

StreamWriter CreateDynamicLogWriter(StorageConfig? config)
{
    if (File.Exists(config.DynamicFile))
    {
        File.Delete(config.DynamicFile);
    }

    return File.AppendText(config.DynamicFile);
}

StreamWriter CreateTrackJsonWriter(StorageConfig? config)
{
    if (File.Exists(config.TrackFile))
    {
        File.Delete(config.TrackFile);
    }

    return File.AppendText(config.TrackFile);
}

string HandleType5Message(IAisMessage aisMessage)
{
    var type5Msg = (AisMessageType5)aisMessage;

    var data = new VesselStaticInfo()
    {
        id = Guid.NewGuid().ToString(),
        mmsi = type5Msg.Mmsi.ToString(),
        imo = type5Msg.ImoNumber.ToString(),
        callSign = type5Msg.CallSign,
        shipName = type5Msg.VesselName.CleanVesselName(),
        shipType = ((int)type5Msg.ShipType).ToString(),
        a = type5Msg.DimensionToBow.ToString(),
        b = type5Msg.DimensionToStern.ToString(),
        c = type5Msg.DimensionToPort.ToString(),
        d = type5Msg.DimensionToStarboard.ToString(),
        len = (type5Msg.DimensionToBow + type5Msg.DimensionToStern).ToString(),
        wid = (type5Msg.DimensionToPort + type5Msg.DimensionToStarboard).ToString(),
        high = string.Empty,
        draft = (type5Msg.Draught10thMetres / 10.0).ToString(),
        rot = string.Empty,
        eta = $"{type5Msg.EtaMonth}:{type5Msg.EtaDay}:{type5Msg.EtaHour}:{type5Msg.EtaMinute}",
        dest = type5Msg.Destination,

        msgId = Guid.NewGuid().ToString(),
        msg = type5Msg.OriginalMessage,
        from = type5Msg.Channel,

        sourceIp = "192.168.1.40",
        BSName = "红花岭"
    };

    if (storageConfig.EnableCapture)
    {
        string content = $"5,{data.mmsi},{data.imo},{data.callSign},{data.shipName},{data.shipType},{data.len},{data.wid},{data.high},{data.draft},{data.time}";
        staticWriter.WriteLine(content);
    }

    return JsonSerializer.Serialize(data, options);
}

string HandleType24Message(IAisMessage aisMessage)
{
    if (!(aisMessage is AisMessageType24Part1))
    {
        return string.Empty;
    }

    var type24Msg = (AisMessageType24Part1)aisMessage;

    var data = new VesselStaticInfo()
    {
        id = Guid.NewGuid().ToString(),
        mmsi = type24Msg.Mmsi.ToString(),
        callSign = type24Msg.CallSign,
        shipType = ((int)type24Msg.ShipType).ToString(),
        a = type24Msg.DimensionToBow.ToString(),
        b = type24Msg.DimensionToStern.ToString(),
        c = type24Msg.DimensionToPort.ToString(),
        d = type24Msg.DimensionToStarboard.ToString(),
        len = (type24Msg.DimensionToBow + type24Msg.DimensionToStern).ToString(),
        wid = (type24Msg.DimensionToPort + type24Msg.DimensionToStarboard).ToString(),

        msgId = Guid.NewGuid().ToString(),
        msg = type24Msg.OriginalMessage,
        from = type24Msg.Channel,

        sourceIp = "192.168.1.40",
        BSName = "红花岭"
    };

    if (storageConfig.EnableCapture)
    {
        string content = $"24,{data.mmsi},{data.imo},{data.callSign},{data.shipName},{data.shipType},{data.len},{data.wid},{data.high},{data.draft},{data.time}";
        staticWriter.WriteLine(content);
    }

    return JsonSerializer.Serialize(data, options);
}

string HandleType1To3Message(IAisMessage aisMessage)
{
    var type1to3Msg = (AisMessageType1Through3)aisMessage;

    var data = new VesselDynamicInfo()
    {
        id = Guid.NewGuid().ToString(),
        mmsi = type1to3Msg.Mmsi.ToString(),
        lon = type1to3Msg.Position?.Longitude.ToString(),
        lat = type1to3Msg.Position?.Latitude.ToString(),
        sailStatus = ((int)type1to3Msg.NavigationStatus).ToString(),
        hdg = type1to3Msg.TrueHeadingDegrees.ToString(),
        cog = type1to3Msg.CourseOverGroundDegrees.ToString(),
        speed = type1to3Msg.SpeedOverGround.ToString(),
        rot = type1to3Msg.RateOfTurn.ToString(),

        msgId = Guid.NewGuid().ToString(),
        msg = type1to3Msg.OriginalMessage,
        from = type1to3Msg.Channel,

        sourceIp = "192.168.1.40",
        BSName = "红花岭"
    };

    if (storageConfig.EnableCapture)
    {
        string content = $"1To3,{data.mmsi},{data.lon},{data.lat},{data.sailStatus},{data.hdg},{data.cog},{data.speed},{data.rot},{data.time}";
        dynamicWriter.WriteLine(content);

        RecordTrack(data, id++, type1to3Msg.Position?.Longitude, type1to3Msg.Position?.Latitude, tracks);
    }

    return JsonSerializer.Serialize(data, options);
}

void RecordTrack(VesselDynamicInfo dynamicInfo, int index, double? longitude, double? latitude, List<Location> locations)
{
    if (dynamicInfo.mmsi == target)
    {
        if (index++ % interval == 0)
        {
            var location = new Location()
            {
                Lon = (double)longitude,
                Lat = (double)latitude
            };
            locations.Add(location);
        }
    }
}

string HandleType18Message(IAisMessage aisMessage)
{
    var type18Msg = (AisMessageType18)aisMessage;

    var data = new VesselDynamicInfo()
    {
        id = Guid.NewGuid().ToString(),
        mmsi = type18Msg.Mmsi.ToString(),
        lon = type18Msg.Position.Longitude.ToString(),
        lat = type18Msg.Position.Latitude.ToString(),
        hdg = type18Msg.TrueHeadingDegrees.ToString(),
        cog = type18Msg.CourseOverGroundDegrees.ToString(),
        speed = type18Msg.SpeedOverGround.ToString(),

        msgId = Guid.NewGuid().ToString(),
        msg = type18Msg.OriginalMessage,
        from = type18Msg.Channel,

        sourceIp = "192.168.1.40",
        BSName = "红花岭"
    };

    if (storageConfig.EnableCapture)
    {
        string content = $"18,{data.mmsi},{data.lon},{data.lat},{data.sailStatus},{data.hdg},{data.cog},{data.speed},{data.rot},{data.time}";
        dynamicWriter.WriteLine(content);

        RecordTrack(data, id++, type18Msg.Position?.Longitude, type18Msg.Position?.Latitude, tracks);
    }

    return JsonSerializer.Serialize(data, options);
}

string HandleType19Message(IAisMessage aisMessage)
{
    var type19Msg = (AisMessageType19)aisMessage;

    var data = new VesselDynamicInfo()
    {
        id = Guid.NewGuid().ToString(),
        mmsi = type19Msg.Mmsi.ToString(),
        lon = type19Msg.Position.Longitude.ToString(),
        lat = type19Msg.Position.Latitude.ToString(),
        hdg = type19Msg.TrueHeadingDegrees.ToString(),
        cog = type19Msg.CourseOverGroundDegrees.ToString(),
        speed = type19Msg.SpeedOverGround.ToString(),

        msgId = Guid.NewGuid().ToString(),
        msg = type19Msg.OriginalMessage,
        from = type19Msg.Channel,

        sourceIp = "192.168.1.40",
        BSName = "红花岭"
    };

    if (storageConfig.EnableCapture)
    {
        string content = $"19,{data.mmsi},{data.lon},{data.lat},{data.sailStatus},{data.hdg},{data.cog},{data.speed},{data.rot},{data.time}";
        dynamicWriter.WriteLine(content);

        RecordTrack(data, id++, type19Msg.Position?.Longitude, type19Msg.Position?.Latitude, tracks);
    }

    return JsonSerializer.Serialize(data, options);
}

async Task PublishMessage(string msg, bool isStatic, IProducer<string, string> mqProducer)
{
    if (!string.IsNullOrEmpty(msg))
    {
        if (isStatic)
        {
            var result = await mqProducer.ProduceAsync(
                mqConfig.StaticTopic, new Message<string, string>
            {
                Key = "deviceAisStaticMsg",
                Value = msg
            });
        }
        else
        {
            var result = await mqProducer.ProduceAsync(
                mqConfig.DynamicTopic, new Message<string, string>
            {
                Key = "deviceAisDymamicTopic",
                Value = msg
            });
        }
    }
}

