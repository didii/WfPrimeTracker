﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WfPrimeTracker.Business.Services;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Server.Controllers.V1 {
    public class PrimeItemsController : ApiController {
        private readonly IPrimeItemService _service;

        public PrimeItemsController(IPrimeItemService service) {
            _service = service;
        }

        [HttpGet]
        [ResponseCache(Duration = 2 * 60 * 60, Location = ResponseCacheLocation.Any)] //= 2 hours
        public async Task<IEnumerable<PrimeItemDto>> GetAll() {
            return await _service.GetAll();
        }

        [HttpGet("{id}/image")]
        [ResponseCache(Duration = 2 * 60 * 60, Location = ResponseCacheLocation.Any)] //= 2 hours
        public async Task<IActionResult> GetImage(int id) {
            return File(await _service.GetImage(id), "image/png");
        }
    }
}
