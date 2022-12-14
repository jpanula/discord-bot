using BackendAPI.Models;
using BackendAPI.Repositories;

namespace BackendAPI.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandGroupRepository _commandGroupRepo;
        private readonly ISubCommandRepository _subCommandRepo;
        private readonly ICommandParameterRepository _parameterRepo;

        public CommandService(ICommandGroupRepository commandGroupRepo, ISubCommandRepository subCommandRepo, ICommandParameterRepository parameterRepo)
        {
            _commandGroupRepo = commandGroupRepo;
            _subCommandRepo = subCommandRepo;
            _parameterRepo = parameterRepo;
        }

        public List<CommandGroup> GetCommandGroups()
        {
            return _commandGroupRepo.Get();
        }

        public CommandGroup GetCommandGroupById(int id)
        {
            return _commandGroupRepo.GetById(id);
        }

        public SubCommand GetSubCommandById(int id)
        {
            return _subCommandRepo.GetById(id);
        }
    }
}
