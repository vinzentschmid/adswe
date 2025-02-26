using System.Net;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Utils;

public class AquariumLogger(ISettings settings)
{
    private ISettings Settings = settings;
    private Boolean IsInitialized = false;
    private static ILogger _Logger;

    public static ILogger Logger
    {
        get { return _Logger;  }
    }

    public ILogger ContextLog<T>() where T : class
    {
        if(_Logger == null)
        {
            Initialize();
        }

        ILogger contextLogger = _Logger.ForContext("Host", Dns.GetHostName()).ForContext<T>();
        return contextLogger;
    }
    
    public async Task Initialize()
    {
        if (!IsInitialized)
        {
            var config = new ConfigurationBuilder()
                .AddJsonStream(
                    new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Settings.LoggerSettings)))
                .Build();
        
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            
            _Logger = Log.Logger;
            _Logger.Information("Logger initialized");
            
            IsInitialized = true;
        }
        
    }
}