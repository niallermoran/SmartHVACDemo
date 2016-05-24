using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Demos.IoT.Models;
using System.Threading;

namespace Demos.IoT.Webjobs
{
    public class Functions
    {
    
        /// <summary>
        /// Sets up an event processor to automatically receive events from the event hub
        /// </summary>
        /// <returns></returns>
        public static async Task SetupEventProcessorIoTHub()
        {
            EventProcessorHost eventProcessorHost = new EventProcessorHost( Guid.NewGuid().ToString(),
                Strings.EventHubName.ToLowerInvariant(), 
                Strings.EventHubConsumerGroupName, 
                Strings.EventHubConnectionString,
                Strings.BLOBStorageConnectionstring );

            Console.WriteLine("Registering EventProcessor...");
            var options = new EventProcessorOptions();

            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            await eventProcessorHost.RegisterEventProcessorAsync<SmartHVACEventProcessor>(options);

        }

        public static async Task SetupEventProcessorDeviceMessages()
        {
            EventProcessorHost eventProcessorHost = new EventProcessorHost(Guid.NewGuid().ToString(),
                Strings.EventHubNameC2D.ToLowerInvariant(),
                Strings.EventHubConsumerGroupNameC2D,
                Strings.EventHubConnectionStringC2D,
                Strings.BLOBStorageConnectionstring);

            Console.WriteLine("Registering Cloud to Device EventProcessor...");
            var options = new EventProcessorOptions();

            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            await eventProcessorHost.RegisterEventProcessorAsync<SmartHVACDeviceEventProcessor>(options);

        }
    }
}
