using BackendAPI.Models;
using BackendAPI.Repositories;

namespace BackendAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventVoteRepository _eventVoteRepository;

        public EventService(IEventRepository eventRepository, IEventVoteRepository eventVoteRepository)
        {
            _eventRepository = eventRepository;
            _eventVoteRepository = eventVoteRepository;
        }

        public List<Event> Get()
        {
            return _eventRepository.Get();
        }

        public Event GetById(int id)
        {
            return _eventRepository.GetById(id);
        }

        public List<EventVote> GetVotes(int id)
        {
            return _eventRepository.GetVotes(id);
        }

        public int? GetEventIdFromMessageId(string messageId)
        {
            var selectedEvent = _eventRepository.GetEventFromMessageId(messageId);
            if (selectedEvent == null)
            {
                return null;
            }
            return selectedEvent.Id;
        }

        public Event Add(EventData data)
        {
            var newEvent = new Event();
            newEvent.Title = data.Title;
            newEvent.Date = data.Date;
            newEvent.Description = data.Description;
            _eventRepository.Add(newEvent);
            return _eventRepository.GetLatest();
        }

        public EventVote AddVote(int eventId, EventVoteData data)
        {
            var updateEvent = _eventRepository.GetById(eventId);
            if (updateEvent == null)
            {
                return null;
            }

            var existingVote = updateEvent.Votes.FirstOrDefault(item => item.Emoji == data.Emoji);
            if (existingVote == null)
            {
                var newVote = new EventVote();
                newVote.Emoji = data.Emoji;
                newVote.DiscordUserIds = new List<string> { data.DiscordUserId };
                newVote.Name = data.Name;
                _eventVoteRepository.Add(newVote);

                newVote = _eventVoteRepository.GetLatest();
                updateEvent.Votes.Add(newVote);
                _eventRepository.Update(updateEvent.Id, updateEvent);
                return newVote;
            }
            if (!existingVote.DiscordUserIds.Contains(data.DiscordUserId))
            {
                existingVote.DiscordUserIds.Add(data.DiscordUserId);
                _eventVoteRepository.Update(existingVote.Id, existingVote);
            }
            return existingVote;
        }
        public Event AddMessageId(int id, string messageId)
        {
            var updatedEvent = _eventRepository.GetById(id);
            if (updatedEvent == null)
            {
                return null;
            }
            if (updatedEvent.MessageIds.Contains(messageId))
            {
                return updatedEvent;
            }
            updatedEvent.MessageIds.Add(messageId);
            _eventRepository.Update(id, updatedEvent);
            return updatedEvent;
        }

        public Event Delete(int id)
        {
            var deletedEvent = _eventRepository.GetById(id);
            if (deletedEvent == null)
            {
                return null;
            }

            foreach (var vote in deletedEvent.Votes)
            {
                _eventVoteRepository.Delete(vote.Id);
            }

            _eventRepository.Delete(id);
            return deletedEvent;
        }

        public EventVote DeleteVote(int eventId, string emoji, string userId)
        {
            var selectedEvent = _eventRepository.GetById(eventId);
            var selectedVote = selectedEvent.Votes.FirstOrDefault(item => item.Emoji == emoji);
            if (selectedVote == null)
            {
                return null;
            }
            if (!selectedVote.DiscordUserIds.Contains(userId))
            {
                return null;
            }
            selectedVote.DiscordUserIds.Remove(userId);
            if (selectedVote.DiscordUserIds.Count == 0)
            {
                _eventVoteRepository.Delete(selectedVote.Id);
            }
            else
            {
                _eventVoteRepository.Update(selectedVote.Id, selectedVote);
            }
            return selectedVote;
        }
    }
}
