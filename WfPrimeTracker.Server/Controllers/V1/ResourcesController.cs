using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WfPrimeTracker.Business.Services;

namespace WfPrimeTracker.Server.Controllers.V1 {
    [Route("api/resources")]
    [ApiController]
    public class ResourcesController : Controller {
        private readonly IResourceService _service;

        public ResourcesController(IResourceService service) {
            _service = service;
        }

        [HttpGet("{id}/image")]
        [ResponseCache(Duration = 7 * 24 * 60 * 60, Location = ResponseCacheLocation.Any)] //= 1 week
        public async Task<IActionResult> GetImage(int id) {
            return File(await _service.GetImage(id), "image/png");
        }
    }
}
