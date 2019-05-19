using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WfPrimeTracker.Business.Services;
using WfPrimeTracker.Dtos.UserData;
using WfPrimeTracker.Infrastructure;
using WfPrimeTracker.Server.Models;

namespace WfPrimeTracker.Server.Controllers.V1 {
    public class UsersController : ApiController {
        private readonly IUserService _service;

        public UsersController(IUserService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<UserSaveDataDto> GetUserData() {
            var result = await _service.GetUserData();
            return result;
        }

        [HttpPost]
        public async Task SaveUserData(UserSaveDataDto dto) {
            await _service.SaveUserData(dto);
        }
    }
}
