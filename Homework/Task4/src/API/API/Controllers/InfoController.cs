using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        private InfoRepository _infoRepository;

        public InfoController(InfoRepository infoRepository)
        {
            _infoRepository = infoRepository;
        }
        [HttpGet]
        public async Task<int> Get()
        {
            return await _infoRepository.GetCount();
        }

        [HttpPost]
        public async Task Post()
        {
            var info = new Info
            {
                Created = DateTime.Now
            };
            await _infoRepository.Store(info);
        }
    }
}
