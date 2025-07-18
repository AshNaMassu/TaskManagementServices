namespace Persistence.Configurations;

public class PostgresDatabaseOptions
{
    public string Schema { get; set; } = "public";
    public string Host { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public int MaxPoolSize { get; set; } = 0;
    public int MinPoolSize { get; set; } = 0;
    public int KeepAlive { get; set; } = 300;
    public int CommandTimeout { get; set; } = 300;
    public int ConnectionIdLifetime { get; set; } = 300;
    public bool Pooling { get; set; } = false;
    public bool TcpKeepAlive { get; set; } = true;
}