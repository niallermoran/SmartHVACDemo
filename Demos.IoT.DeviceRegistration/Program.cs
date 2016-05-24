using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Demos.IoT.Models;

namespace Demos.IoT.DeviceRegistration
{
    class Program
    {
        // properties required to connect with the IoT hub and regiter a device
         private static RegistryManager registryManager;

        static void Main(string[] args)
        {
            // get the registry manager
            registryManager = RegistryManager.CreateFromConnectionString( Strings.IoTHubConnectionString );

            // get devices from Azure
            var devices = DeviceFactory.Instance.Devices;

            // for each device register with the IoT Hub
            foreach( var device in devices )
            {
                RegisterDeviceAsync(device.DeviceId).Wait();
            }

        }


        /// <summary>
        /// This method registers a device with the IoT hub. If the device is already registered then the method will return the device
        /// </summary>
        /// <returns></returns>
        private async static Task RegisterDeviceAsync(string deviceId)
        {
            var device = await registryManager.GetDeviceAsync(deviceId);
            if (device == null)
            {
                device = new Microsoft.Azure.Devices.Device(deviceId);
                device = await registryManager.AddDeviceAsync(device);

                Console.WriteLine("Device registered: " + device.Authentication.SymmetricKey.PrimaryKey);
            }
            else
                Console.WriteLine("Device " + deviceId + " exists");
        }
    }
}
