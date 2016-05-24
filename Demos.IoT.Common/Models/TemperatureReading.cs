using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.IoT.Models
{
    public class TemperatureReading
    {
        private string _uniqueId;

        public string UniqueId {
            get {
                if (string.IsNullOrEmpty(_uniqueId))
                    _uniqueId = Guid.NewGuid().ToString();
                return _uniqueId;
            } }

        public TemperatureReading(string deviceId, DateTime time)
        {
            this.DeviceId = deviceId;
            this.Time = time;
            this._uniqueId = Guid.NewGuid().ToString();
        }

        public TemperatureReading() { }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public double Internaltemp { get; set; }

        public double ExternalTemp { get; set; }

        public int FloorArea { get; set; }

        public int NumberofPeople { get; set; }

        /// <summary>
        /// 1 for on and 0 for off, using integer so we can aggregate later
        /// </summary>
        public int IsHeatingOn { get; set; }

        public DateTime Time { get; set; }

        public string TimeLabel
        {
            get
            {
                return Time.ToString("dd MMM yyyy HH:mm:ss");
            }
        }

        public string TimeLabelShort
        {
            get
            {
                return Time.ToString("HH:mm:ss");
            }
        }
    }

    public class DeviceTemperatureReadings
    {
        public DeviceModel Device { get; set; }

        public List<TemperatureReading> TemperatureReadings { get; set; }
    }
}
