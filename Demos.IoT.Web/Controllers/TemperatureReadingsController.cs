using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Demos.IoT.Models;

using StackExchange.Redis;

using Swashbuckle.Swagger.Annotations;
using Swashbuckle.Swagger;

namespace Demos.IoT.Web.Controllers
{
    public class TemperatureReadingsController : ApiController
    {
        [SwaggerOperation("GetTemperatureReadings")]
        public IEnumerable<DeviceTemperatureReadings> Get( int count )
        {
            return WebApiApplication.RedisHelper.GetDevicesTemperatureReadings( count );
        }

    }

}
