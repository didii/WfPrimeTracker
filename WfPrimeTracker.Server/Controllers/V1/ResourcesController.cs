using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WfPrimeTracker.Business.Services;

namespace WfPrimeTracker.Server.Controllers.V1 {
    public class ResourcesController : ApiController {
        private readonly IResourceService _service;

        public ResourcesController(IResourceService service) {
            _service = service;
        }

        [HttpGet("{id}/image")]
        [ResponseCache(Duration = 2 * 60 * 60, Location = ResponseCacheLocation.Any)] //= 2 hours
        public async Task<IActionResult> GetImage(int id) {
            return File(await _service.GetImage(id), "image/png");
        }
    }
}
