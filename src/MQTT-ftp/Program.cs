using MQTT_ftp;
using MQTT_ftp.Services;
using MR.Log;
using MR.MQTT.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .MRConfigureLogService()
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddSingleton<MQTTService>();
        services.AddSingleton<FtpFolderService>();
        services.AddHostedService<Worker>();
    })
    .Build();

MRLog.ConfigureLogMain();

try
{
    await host.RunAsync();
}
finally
{
    MRLog.CloseAndFlush();
}
