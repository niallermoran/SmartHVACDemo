using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Demos.IoT.Models;
using System.Threading;
using System.Diagnostics;

namespace Demos.IoT.Webjobs
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {

            try
            {
                // clears down the cache once started
                RedisCacheHelper helper = new RedisCacheHelper();
                helper.ClearDownRedisCache();

                Functions.SetupEventProcessorIoTHub().Wait();
                Functions.SetupEventProcessorDeviceMessages().Wait();

                RunAsync().Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                Console.WriteLine("Webjob terminating: {0}", ex.ToString());
            }

        }

        static async Task RunAsync()
        {
            Console.WriteLine("Starting event processor");

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
       
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationTokenSource.Token);
                }
                catch (TaskCanceledException) { }
            }

            Console.WriteLine("Ending event processor");

        }


    }
}
