using System;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController(
            IMapper mapper,
            ICommandRepo repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        private readonly IMapper _mapper;
        private readonly ICommandRepo _repository;

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms From CommandsService");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Platforms Controller");
        }
    }
}
