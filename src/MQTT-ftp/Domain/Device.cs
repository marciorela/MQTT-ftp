using MR.MQTT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_ftp.Domain
{
    public class Device : DeviceBase
    {
        public Device(string name) : base(name)
        {
            Topic = "homeassistant/sensor/ftpclientes/";
            Device_class = "timestamp";

            Unique_id = name.ToLower().Replace(" ", "_");

            Topic += Unique_id;

            Name = name;
            State_topic = $"{Topic}/state";
        }
    }
}
