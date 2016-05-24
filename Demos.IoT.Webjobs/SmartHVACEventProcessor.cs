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

namespace Demos.IoT.Webjobs
{
    /// <summary>
    /// This is the main event processor that reads events from the IoT hub and sends the data
    /// to a redis cache for real time reporting
    /// </summary>
    public class SmartHVACEventProcessor : IEventProcessor
    {
        IDatabase cache = null;
        long DataPointsSent = 0;
        static Timer timer;

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
            try
            {
                // get the redis cache object set up
                var redisConnection = ConnectionMultiplexer.Connect(Strings.RedisConnectionString);
                cache = redisConnection.GetDatabase();

                // setup timer to report status every hour
                TimerCallback callback = ReportStatus;
                timer = new Timer(callback, null, 1000, 60 * 30 * 1000);

                return Task.FromResult<object>(null);
            }
            catch( Exception ex)
            {
                Console.WriteLine("Error connecting to redis: " + ex.Message);
            }

            return Task.FromResult<object>(null);
        }

        void ReportStatus(object stateInfo)
        {
            Console.WriteLine("Total datapoints added to cache: " + DataPointsSent.ToString());
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

                    // add this json to the head of a liost for this device
                    await cache.ListLeftPushAsync(model.DeviceId, data);

#if DEBUG
                    Console.WriteLine("{0} > temp: {1},  is heating on: {2}, # People: {3}", model.DeviceId + " " + model.Time.ToString("HH:mm"), 
                       model.Internaltemp.ToString(), model.IsHeatingOn.ToString(), model.NumberofPeople.ToString());
#endif

                    this.DataPointsSent++;

                }

                await context.CheckpointAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing events: " + ex.Message);
            }
        }
    }
}
