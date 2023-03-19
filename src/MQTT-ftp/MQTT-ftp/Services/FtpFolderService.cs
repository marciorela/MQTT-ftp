using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_ftp.Services
{
    public class FtpFolderService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FtpFolderService> _logger;
        private readonly List<string> _exclude;
        private readonly string _ftpFolder;

        public FtpFolderService(IConfiguration config, ILogger<FtpFolderService> logger)
        {
            _config = config;
            _logger = logger;
            _exclude = _config.GetSection("ExcludeFolders").Get<List<string>>();
            _ftpFolder = _config.GetSection("Folders:FTP").Get<string>();

            _logger.LogInformation("Configuração do ftp em {ftp}", _ftpFolder);
        }

        public List<string> GetFolders()
        {
            var resultFolders = new List<string>();

            var folders = Directory.EnumerateDirectories(_ftpFolder).ToList();
            foreach (var folderPath in folders)
            {
                var folderName = Path.GetFileName(folderPath);
                if (folderName[0] != '.' && !_exclude.Any(y => y == folderName))
                {
                    resultFolders.Add(folderName);
                }
            }

            return resultFolders;
        }

        public DateTime GetNewestFileDateTime(string folder)
        {
            var path = Path.Combine(_ftpFolder, folder);

            var directory = new DirectoryInfo(path);
            var files = directory.EnumerateFiles("*.zip", new EnumerationOptions()
            {
                RecurseSubdirectories = true
            }).OrderByDescending(x => x.Name);

            if (files.Any())
            {
                return DateTime.ParseExact(Path.GetFileNameWithoutExtension(files.First().Name).AsSpan(0, 14), "yyyyMMddHHmmss", CultureInfo.CreateSpecificCulture("pt-BR"));
            }
            else 
            {
                return DateTime.MinValue;
            }
        }
    }
}
