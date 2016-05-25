using System.Text;
using System;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using System.Threading;
using StackExchange.Redis;
using System.Diagnostics;
using Newtonsoft.Json;
using Demos.IoT.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Demos.IoT.Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            ClearDownRedisCache();
            DeleteTable("Streaming");
            DeleteTable("Hopping");
            DeleteTable("Tumbling");
            DeleteTable("Sliding");
            DeleteTable("ComfortLevels");

            DeleteTable("WADMetricsPT1MP10DV2S20160415");
            DeleteTable("WADMetricsPT1MP10DV2S20160425");
            DeleteTable("WADMetricsPT1MP10DV2S20160505");
            DeleteTable("WADMetricsPT1MP10DV2S20160515");


            DeleteTable("WADMetricsPT30MP10DV2S20160415");
            DeleteTable("WADMetricsPT30MP10DV2S20160425");
            DeleteTable("WADMetricsPT30MP10DV2S20160505");
            DeleteTable("WADMetricsPT30MP10DV2S20160515");

            DeleteTable("WADMetricsPT5MP10DV2S20160415");
            DeleteTable("WADMetricsPT5MP10DV2S20160425");
            DeleteTable("WADMetricsPT5MP10DV2S20160505");
            DeleteTable("WADMetricsPT5MP10DV2S20160515");
        }

        static void ClearDownRedisCache()
        {
            try
            {
                var redisConnection = ConnectionMultiplexer.Connect(Strings.RedisConnectionString);
                var redisCache = redisConnection.GetDatabase();

                // get the redis cache object set up
                var devices = DeviceFactory.Instance.Devices;
                foreach (var device in devices)
                {
                    // for device 5 clear down all values
                    redisCache.KeyDelete(device.DeviceId);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to redis: " + ex.Message);
            }
        }


        /// <summary>
        /// Deletes the tables for stream analytics outputs.
        /// These will be recreated once SA is tarted
        /// </summary>
        static void DeleteTable( string name )
        {
            try
            {
                string cn = string.Format(Strings.BLOBStorageConnectionstring);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cn);

                var client = storageAccount.CreateCloudTableClient();

                client.GetTableReference(name).Delete();
            }

            catch
            {

            }
        }
    }
}
