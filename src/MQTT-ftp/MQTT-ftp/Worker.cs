using MQTT_ftp.Domain;
using MQTT_ftp.Services;
using MR.MQTT.Domain;
using MR.MQTT.Service;

namespace MQTT_ftp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly FtpFolderService _folderService;
        private readonly MQTTService _mqtt;
        private List<Device> _devices;


        public Worker(ILogger<Worker> logger, IConfiguration config, FtpFolderService folderService, MQTTService mqtt)
        {
            _logger = logger;
            _config = config;
            _folderService = folderService;
            _mqtt = mqtt;
            _devices = new();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var _delay = _config.GetValue<int>("Service:Delay", 30000);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RunWorker();
                    await Task.Delay(_delay, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Eliminando registros de devices...");
            foreach (var device in _devices)
            {
                await _mqtt.UnRegister(device);
            }
        }

        private async Task RunWorker()
        {
            var folders = _folderService.GetFolders();
            foreach (var folder in folders)
            {
                var ultimaData = _folderService.GetNewestFileDateTime(folder);

                var device = GetDevice("cliente_ftp_backup_" + folder);

                await _mqtt.Register(device);
                await _mqtt.SendState(device, ultimaData);
            }
        }

        private Device GetDevice(string folder)
        {
            var device = _devices.FirstOrDefault(x => x.Name == folder);
            if (device == null)
            {
                device = new Device(folder);
                _devices.Add(device);
            }
            return device;

        }
    }
}