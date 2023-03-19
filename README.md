# MQTT-ftp
Serviço do windows que avisa ao Home Assistant através do MQTT, o último backup do cliente.
 
Cada cliente é criado como uma entidade de domínio sensor dentro do Home Assistant.

## Configuração
appsettings.json:

```json
{
  "Folders": {
    "FTP": "<pasta de usuários do FTP>",
  },
  "ExcludeFolders": [
    "<array com todos os usuários que devem ser ignorados na pasta de ftp>"
  ]
  "Service": {
    "Delay": "<tempo de espera entre as chamadas (default 30000)>"
  }
  "MQTT": {
    "Server": "<IP do servidor MQTT>",
    "Username": "<Usuario do MQTT>",
    "Password": "<Senha do MQTT>"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```