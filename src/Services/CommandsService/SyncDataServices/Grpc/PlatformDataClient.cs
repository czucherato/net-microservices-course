using System;
using AutoMapper;
using Grpc.Net.Client;
using PlatformService;
using CommandsService.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        public PlatformDataClient(
            IMapper mapper,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcPlatform"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}