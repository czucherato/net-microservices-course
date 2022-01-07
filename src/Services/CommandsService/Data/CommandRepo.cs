using System;
using System.Linq;
using CommandsService.Models;
using System.Collections.Generic;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        // Platforms
        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentException(null, nameof(plat));
            }

            _context.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        // Commands
        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentException(null, nameof(command));
            }

            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.FirstOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(c => c.PlatformId == platformId).OrderBy(c => c.Platform.Name);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }
    }
}