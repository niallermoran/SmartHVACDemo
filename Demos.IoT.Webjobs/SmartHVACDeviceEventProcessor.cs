using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using System.Threading;
using StackExchange.Redis;
using System.Diagnostics;
using Newtonsoft.Json;
using Demos.IoT.Models;
using Microsoft.Azure.Devices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Demos.IoT.Webjobs
{
    /// <summary>
    /// This event processor is used to read events from an event hub
    /// which is used to send messages to devices
    /// </summary>
    public class SmartHVACDeviceEventProcessor : IEventProcessor
    {
        IDatabase cache = null;

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason);
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        public Task OpenAsync(PartitionContext context)
        {
            // get the redis cache object set up
            var redisConnection = ConnectionMultiplexer.Connect(Strings.RedisConnectionString);
            cache = redisConnection.GetDatabase();


            return Task.FromResult<object>(null);
        }

        public string LastMessageOffset { get; private set; }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            try
            {
                foreach (EventData eventData in messages)
                {
                    this.LastMessageOffset = eventData.Offset;

                    string data = Encoding.UTF8.GetString(eventData.GetBytes());

                    var model = JsonConvert.DeserializeObject<TemperatureReading>(data);

                    Newtonsoft.Json.Linq.JObject json = JObject.Parse(data);
                    string action = json.GetValue("action").ToString();

                    // send this data to the appropriate evice
                    await SendMessageToDevice(model.DeviceId, action);

                    // add data to cache for display on UI
                    // add this json to the head of a liost for this device
                    await cache.ListLeftPushAsync( "DeviceCommands", data );

                    Console.WriteLine("Processed cloud to device event for Device: " + model.DeviceId + ", Action: " + action);

                }

                await context.CheckpointAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing events: " + ex.Message);
            }
        }

        /// <summary>
        /// Sends the data to a device registered in the IoT Hub
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SendMessageToDevice(string deviceId, string data)
        {
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString( Strings.IoTHubConnectionString);
            var commandMessage = new Message(Encoding.ASCII.GetBytes( data ));
            await serviceClient.SendAsync( deviceId, commandMessage);
        }
    }
}
