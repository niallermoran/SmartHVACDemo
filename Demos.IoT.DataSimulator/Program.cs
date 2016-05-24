using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using Demos.IoT.Models;

namespace Demos.IoT.Webjobs.DataSimulator
{
    class Program
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static Timer timer;


        static void Main(string[] args)
        {
            try
            {
                TimerCallback callback = ReportStatus;
                timer = new Timer( callback , null, 1000, 60 * 1000* 30); // 60*60*1000

                RunAsync().Wait();
            }
            catch (Exception ex)
            {
                cancellationTokenSource.Cancel();
                Trace.TraceError("Webjob terminating: {0}", ex.ToString());
            }
        }

        static void ReportStatus( object stateInfo )
        {
            Console.WriteLine("Total datapoints sent: " + Functions.DataPointsSent.ToString());
        }

        static async Task RunAsync()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                Trace.TraceInformation("Running");
                await Functions.SimulateData( );
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), cancellationTokenSource.Token);
                }
                catch (TaskCanceledException) { }
            }
        }

    }
}
