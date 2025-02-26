namespace Utils;

public class AquariumSettings : ISettings
{
    public string LoggerSettings { get; set; }
    public MongoDBSettings MongoDBSettings { get; set; }
}