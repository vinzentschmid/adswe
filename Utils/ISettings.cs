namespace Utils;

public interface ISettings
{
    public String LoggerSettings { get; set; }
    
    public MongoDBSettings MongoDBSettings { get; set; }
}