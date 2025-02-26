using Newtonsoft.Json;

namespace Utils;

public class ConsulSettingsHandler(ISettings settings) : ISettingsHandler
{
    private ISettings Settings = settings;

    public async Task Load()
    {
        ConsulClient client = new ConsulClient();

        String loggerSettings = await client.GetKey("AquariumManagement/Logger");
        
        Settings.LoggerSettings = loggerSettings;
        
        string mongostring = await client.GetKey("AquariumManagement/Database");

        MongoDBSettings mongoDbSettings = JsonConvert.DeserializeObject<MongoDBSettings>(mongostring);
        
        Settings.MongoDBSettings = mongoDbSettings;
    }
}