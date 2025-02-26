using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Serilog;
using Utils;

namespace DAL;

public class DBContext
{
    private ILogger log = AquariumLogger.Logger;
    private ISettings Settings;

    public IMongoDatabase DataBase { get; set; }
    public GridFSBucket GridFSBucket { get; private set; }
    MongoClient Client;
    public bool IsConnected
    {
        get
        {
            return DataBase != null;
        }
    }

    public DBContext(ISettings settings)
    {
        this.Settings = settings;
        Task task = Connect();
        task.Wait();
    }

    public async Task Connect()
    {
        MongoClientSettings settings = new MongoClientSettings();
        settings.Server = new MongoServerAddress(Settings.MongoDBSettings.Server, Settings.MongoDBSettings.Port);
        settings.Credential = MongoCredential.CreateCredential("admin", Settings.MongoDBSettings.Username, Settings.MongoDBSettings.Password);
        Client = new MongoClient(settings);
        DataBase = Client.GetDatabase(Settings.MongoDBSettings.DatabaseName);

        if (DataBase != null)
        {
            log.Information("Connected to database {DatabaseName}", Settings.MongoDBSettings.DatabaseName);
        }
        else
        {
            log.Fatal("Could not connect to database {DatabaseName}", Settings.MongoDBSettings.DatabaseName);
        }
    }

    public async Task Disconnect()
    {

    }


}