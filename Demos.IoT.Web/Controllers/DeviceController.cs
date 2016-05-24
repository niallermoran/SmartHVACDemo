using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Demos.IoT.Models;

using Swashbuckle.Swagger.Annotations;

namespace Demos.IoT.Web.Controllers
{
    public class DeviceController : ApiController
    {
        [SwaggerOperation("GetAllDevices")]
        /// <summary>
        /// Gets a list of all devices
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeviceModel> Get()
        {
            return DeviceFactory.Instance.Devices;
        }

        [SwaggerOperation("GetDevice")]
        // GET: api/Device/5
        public DeviceModel Get(string id)
        {
            return (from d in DeviceFactory.Instance.Devices where d.DeviceId == id select d).FirstOrDefault();
        }

    }

}
