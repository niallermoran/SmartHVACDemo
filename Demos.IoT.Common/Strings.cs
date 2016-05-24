using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;

namespace Demos.IoT
{
    public class Strings
    {
        /// Below connection strings and keys are hard coded for simplicity
        /// please remember to change this for any production system, e.g. Use CloudConfigurationManager
        /// as per commented settings below

        /// <summary>
        /// Iot Hub properties from portal
        /// </summary>
        public static string IotHubHostname = "[iot hub name].azure-devices.net"; // under properties in IoT Hub
        public static string IotHubSharedKeyName = "[shared key name]"; // under shared access keys in IoT Hub
        public static string IotHubSharedKeyValue = "[shared key value]"; // under shared access keys in IoT Hub
        public static string IoTHubConnectionString = string.Format("HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}", IotHubHostname, IotHubSharedKeyName, IotHubSharedKeyValue); // used for sending data to IoT hub


        /// <summary>
        /// Event hub details from messaging properties of iot hub, used for reading data from event hub
        /// </summary>
        public static string EventHubName = "[iot hub name]";
        private static string EventHubSBConnectionString = "[service bus compatible string from messaging section of IoT hub settings]";
        public static string EventHubConnectionString = string.Format("Endpoint={0};SharedAccessKeyName={1};SharedAccessKey={2}", EventHubSBConnectionString, IotHubSharedKeyName, IotHubSharedKeyValue);
        public static string EventHubConsumerGroupName = "[consumer group]"; // use a consumer group for each event processor, in our case there's just one


        /// <summary>
        /// Storage account used telemtry and reference data
        /// </summary>
        public static string BLOBStorageAccountName = "[]";
        public static string BLOBStorageAccountKey = "[]";
        public static string BLOBStorageConnectionstring = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", BLOBStorageAccountName, BLOBStorageAccountKey);

        /// <summary>
        /// Event hub details for messages to be sent to devices from the Cloud
        /// </summary>
        public static string EventHubNameC2D = "[event hub for device messages]";
        public static string EventHubConnectionStringC2D = ""; // "Endpoint=sb://name.servicebus.windows.net/;SharedAccessKeyName=[];SharedAccessKey=[]"
        public static string EventHubConsumerGroupNameC2D = "";// use a consumer group for each event processor, in our case there's just one

        /// <summary>
        /// Redis cache connection string
        /// </summary>
        public static string RedisConnectionString = "[]";
            
        ///// <summary>
        ///// Iot Hub properties from portal
        ///// </summary>
        //public static string IotHubHostname = CloudConfigurationManager.GetSetting("IotHubHostname"); // under properties in IoT Hub
        //public static string IotHubSharedKeyName = CloudConfigurationManager.GetSetting("IotHubSharedKeyName"); // under shared access keys in IoT Hub
        //public static string IotHubSharedKeyValue = CloudConfigurationManager.GetSetting("IotHubSharedKeyValue"); // under shared access keys in IoT Hub
        //public static string IoTHubConnectionString = string.Format("HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}", IotHubHostname, IotHubSharedKeyName, IotHubSharedKeyValue); // used for sending data to IoT hub

        ///// <summary>
        ///// Storage account used telemtry and reference data
        ///// </summary>
        //public static string BLOBStorageAccountName = CloudConfigurationManager.GetSetting("BLOBStorageAccountName");
        //public static string BLOBStorageAccountKey = CloudConfigurationManager.GetSetting("BLOBStorageAccountKey");
        //public static string BLOBStorageConnectionstring = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", BLOBStorageAccountName, BLOBStorageAccountKey);


        ///// <summary>
        ///// Event hub details from messaging properties of iot hub, used for reading data from event hub
        ///// </summary>
        //public static string EventHubName = CloudConfigurationManager.GetSetting("EventHubName");
        //private static string EventHubSBConnectionString = CloudConfigurationManager.GetSetting("EventHubSBConnectionString");
        //public static string EventHubConnectionString = string.Format("Endpoint={0};SharedAccessKeyName={1};SharedAccessKey={2}", EventHubSBConnectionString, IotHubSharedKeyName, IotHubSharedKeyValue);
        //public static string EventHubConsumerGroupName = CloudConfigurationManager.GetSetting("EventHubConsumerGroupName"); // use a consumer group for each event processor, in our case there's just one



        ///// <summary>
        ///// Event hub details for messages to be sent to devices from the Cloud
        ///// </summary>
        //public static string EventHubNameC2D = CloudConfigurationManager.GetSetting("EventHubNameC2D");
        //public static string EventHubConnectionStringC2D = CloudConfigurationManager.GetSetting("EventHubConnectionStringC2D"); // "Endpoint=sb://smarthvacdemo.servicebus.windows.net/;SharedAccessKeyName=smarthvacdemo;SharedAccessKey=NlwBYDjyjRQdlEdB+k5L7fRsAvsas+Fiy0rlEOsykLI=;EntityPath=smarthvacdevicemessages";
        //public static string EventHubConsumerGroupNameC2D = CloudConfigurationManager.GetSetting("EventHubConsumerGroupNameC2D"); // use a consumer group for each event processor, in our case there's just one

        ///// <summary>
        ///// Redis cache connection string
        ///// </summary>
        //public static string RedisConnectionString = CloudConfigurationManager.GetSetting("RedisConnectionString");

    }
}
