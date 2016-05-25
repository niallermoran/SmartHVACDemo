using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Demos.IoT.Models;
using Microsoft.WindowsAzure;

namespace Demos.IoT.Webjobs.DataSimulator
{
    public class Functions
    {
        public static long DataPointsSent;

        public static async Task SimulateData( )
        {
            // registry manager to access registered devices
            RegistryManager registryManager = null;

            try
            {
                // create an IoT Hub Registry manager to get access to the device
                registryManager = RegistryManager.CreateFromConnectionString(Strings.IoTHubConnectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't create Event Hub registry manager: " + ex.Message);
            }

            try
            {
                var devices = DeviceFactory.Instance.Devices;

                // for each device simulate some data
                foreach (var deviceModel in devices)
                {
                    // get the device as we need it's unique security credentials
                    Device device = null;
                    try
                    {
                        device = await registryManager.GetDeviceAsync(deviceModel.DeviceId);
                    }
                    catch( Exception ex)
                    {
                        Console.WriteLine("Error with GetDeviceAsync: " + ex.GetText(true));
                    }

                    if (device != null && deviceModel.Generate)
                    {

                        // create the client object so we can start to send data
                        var deviceClient = DeviceClient.Create(Strings.IotHubHostname,
                            new DeviceAuthenticationWithRegistrySymmetricKey(device.Id, device.Authentication.SymmetricKey.PrimaryKey),
                                 Microsoft.Azure.Devices.Client.TransportType.Amqp_Tcp_Only);

                        await deviceClient.OpenAsync();

                        // we will randomly generate data around these average values
                        double avgInternalTemp = 20; // celcius 

                        Random rand = new Random();

                        // get the sensor readings for 4 stands
                        double internaltemp = avgInternalTemp + (rand.NextDouble() * 4) - 1; // actual temp values from thermostat, could be anything just under 20 to about 23 
                        double numberofPeople = 40 + (rand.NextDouble() * 4) - 1; // actual temp values from thermostat, could be anything just under 20 to about 23 

                        // make this random to simulate somebody just pressing boost
                        int isHeatingOn = (int)Math.Round( rand.NextDouble() );

                        // capture the time that the reading is taken
                        DateTime time = DateTime.Now.ToUniversalTime();

                        // create an object to represent the data
                        TemperatureReading telemetryDataPoint = new TemperatureReading
                        {
                            DeviceId = device.Id,
                            DeviceName = deviceModel.DeviceName,
                            FloorArea = deviceModel.FloorArea,
                            ExternalTemp = internaltemp - 15,
                            Internaltemp = internaltemp,
                            IsHeatingOn = isHeatingOn,
                            NumberofPeople = (int) numberofPeople,
                            Time = time
                        };

                        // create the message to send
                        var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(telemetryDataPoint)));

                        // send to IoT Hub
                        try
                        {
                            await Helpers.OperationWithBasicRetryAsync<object>(async () =>
                            {
                                await deviceClient.SendEventAsync(message);
                                DataPointsSent++;
#if DEBUG
                                Console.WriteLine("Sending data to hub {0} > temp: {1},  is heating on: {2}, # People: {3}", device.Id + " " + time.ToString("HH:mm:ss"), internaltemp.ToString(), isHeatingOn.ToString(),  numberofPeople.ToString());
#endif
                                return null;

                            }, 3);

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine("Error: " + ex.GetText(true));
                        }
                        finally
                        {
                            try
                            {
                                await deviceClient.CloseAsync();
                            }
                            catch { }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Couldn't send data to cloud: " + ex.GetText(true));
            }
        }
    }
}
