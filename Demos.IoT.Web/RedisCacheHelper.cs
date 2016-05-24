﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

using Demos.IoT.Models;

namespace Demos.IoT.Web
{
    public class RedisCacheHelper
    {
        IDatabase redisCache;

        public RedisCacheHelper()
        {
            var redisConnection = ConnectionMultiplexer.Connect(Strings.RedisConnectionString);
            redisCache = redisConnection.GetDatabase();
        }

        /// <summary>
        /// Gets the most recent temperature readings for each device.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DeviceTemperatureReadings> GetDevicesTemperatureReadings(int count)
        {
            // get the earliest time to return data for
            List<DeviceTemperatureReadings> readings = new List<DeviceTemperatureReadings>();

            var devices = DeviceFactory.Instance.Devices;

            foreach( var device in devices)
            {
                DeviceTemperatureReadings tempList = new DeviceTemperatureReadings();
                tempList.Device = device;

                // get's the next list item from the head for this device id
                var array = redisCache.ListRange(device.DeviceId, 0, count - 1);

                List<TemperatureReading> list = new List<TemperatureReading>();
                foreach (var s in array)
                {
                    try
                    {
                        var o = JsonConvert.DeserializeObject<TemperatureReading>(s);
                        list.Add(o);
                    }
                    catch { }
                }

                tempList.TemperatureReadings = (from l in list orderby l.Time select l).ToList();

                readings.Add(tempList);
            }

            return readings;
        }

        /// <summary>
        /// Gets the last temperature reading for all devices
        /// </summary>
        /// <returns></returns>
        public IEnumerable< TemperatureReading> GetDevicesLastTemperatureReading()
        {
            try
            {
                List<TemperatureReading> readings = new List<TemperatureReading>();
                var devices = DeviceFactory.Instance.Devices;

                foreach (var device in devices)
                {
                    try
                    {
                        // get's the next list item from the head for this device id
                        string json = redisCache.ListRange(device.DeviceId, 0, 1)[0];
                        var reading = Newtonsoft.Json.JsonConvert.DeserializeObject<TemperatureReading>(json);
                        readings.Add(reading);
                    }
                    catch
                    {
                    }
                }

                return readings;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
