using System.Text;
using Consul;

namespace Utils;

public class ConsulClient
{
    public async Task<String> GetKey(String key)
    {
        String url = "http://127.0.0.1:8500";
        ConsulClientConfiguration config = new ConsulClientConfiguration
        {
            Address = new Uri(url),
            WaitTime = TimeSpan.FromSeconds(3)
        };

        Consul.ConsulClient client = new Consul.ConsulClient(config);
        
        var pair = await client.KV.Get(key);

        if (pair.Response == null) return null;
        
        String value = Encoding.UTF8.GetString(pair.Response.Value, 0, pair.Response.Value.Length);
        
        return value;

    }
}