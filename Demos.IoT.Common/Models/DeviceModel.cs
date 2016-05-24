using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Demos.IoT.Models
{
    public class DeviceModel
    {
        /// <summary>
        /// The device id used to identify a device
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// The name of the device
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Indicates if we swhould generate data for this device
        /// </summary>
        public bool Generate { get; set; }

        /// <summary>
        /// The floor area of the factory
        /// </summary>
        public int FloorArea { get; set; }

        public string DeviceLocation { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }

    public class DeviceFactory
    {
        private List<DeviceModel> devices;

        /// <summary>
        /// This is a singleton so hide the constructor
        /// </summary>
        private DeviceFactory()
        {
            devices = new List<DeviceModel>();

            try
            {
                // get the azure blob devices.json from smarthvacstorage/referencedata
                string cn = string.Format(Strings.BLOBStorageConnectionstring);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cn);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container.
                CloudBlobContainer container = blobClient.GetContainerReference("referencedata");

                // download json
                System.IO.MemoryStream s = new System.IO.MemoryStream();
                container.GetBlobReference("devices.json").DownloadToStream(s);
                string json = Encoding.UTF8.GetString(s.ToArray());


                // desrialise the json
                devices.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceModel>>(json));
            }
            catch{ }
           
        }

        public List<DeviceModel> Devices
        {
            get
            {
                return devices;
            }
        }

        private static DeviceFactory instance;

        public static DeviceFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeviceFactory();
                }
                return instance;
            }
        }

    }
}
