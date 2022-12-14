using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Modules
{
    [Discord.Interactions.Group("event", "Commands for managing events")]
    public class EventModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private List<EmojiInfo> _emojiList;

        private struct Event
        {
            [JsonProperty("id")]
            public int Id;
            [JsonProperty("title")]
            public string Title;
            [JsonProperty("description")]
            public string? Description;
            [JsonProperty("date")]
            public DateTime Date;
            [JsonProperty("votes")]
            public List<EventVote> Votes;
            [JsonProperty("messageIds")]
            public List<string> MessageIds;
        }

        private struct EventData
        {
            [JsonProperty("title")]
            public string Title;
            [JsonProperty("description")]
            public string? Description;
            [JsonProperty("date")]
            public DateTime Date;
        }

        private struct EventVote
        {
            [JsonProperty("id")]
            public int Id;
            [JsonProperty("name")]
            public string Name;
            [JsonProperty("emoji")]
            public string Emoji;
            [JsonProperty("discordUserIds")]
            public List<string> UserIds;
        }

        private struct EventVoteData
        {
            [JsonProperty("name")]
            public string Name;
            [JsonProperty("emoji")]
            public string Emoji;
            [JsonProperty("discordUserId")]
            public string UserId;
        }

        private struct EmojiInfo
        {
            [JsonProperty("slug")]
            public string Slug;
            [JsonProperty("character")]
            public string Character;
            [JsonProperty("unicodeName")]
            public string UnicodeName;
            [JsonProperty("codePoint")]
            public string CodePoint;
            [JsonProperty("group")]
            public string Group;
            [JsonProperty("subGroup")]
            public string SubGroup;
        }

        public EventModule(HttpClient httpClient, IConfiguration configuration, DiscordSocketClient client)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _client = client;
            _client.ReactionAdded += HandleAddReaction;
            _client.ReactionRemoved += HandleRemoveReaction;
            _client.ButtonExecuted += HandleButtonPress;
            GetEmojiList();
        }

        [SlashCommand("create", "Create an event people can vote on")]
        public async Task CreateEvent(
            string title,
            string date,
            string description = null)
        {
            await DeferAsync();
            DateTime dateTime;
            try
            {
                dateTime = DateTime.Parse(date);
            }
            catch (FormatException exception)
            {
                await FollowupAsync("Couldn't parse given date");
                return;
            }

            var eventData = new EventData() { Title = title, Description = description, Date = dateTime.ToUniversalTime() };
            var httpContent = new StringContent(JsonConvert.SerializeObject(eventData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_configuration["Database:apiUrl"] + "events", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                await LogHttpRequestException(response, "Failed to add event to database");
                await FollowupAsync("Failed to create event", ephemeral: true);
                return;
            }

            var newEvent = JsonConvert.DeserializeObject<Event>(await response.Content.ReadAsStringAsync());
            var embed = CreateEventEmbed(newEvent);

            var message = await FollowupAsync(embed: embed, allowedMentions: Discord.AllowedMentions.None);
            await PostMessageId(newEvent.Id, message.Id.ToString());
        }

        [SlashCommand("get", "Fetch specified event")]
        public async Task GetEventCommand(int id)
        {
            await DeferAsync();
            var fetchedEvent = await GetEvent(id);
            if (!fetchedEvent.HasValue)
            {
                await FollowupAsync("Event not found");
                return;
            }
            var embed = CreateEventEmbed(fetchedEvent.Value);
            var message = await FollowupAsync(embed: embed, allowedMentions: Discord.AllowedMentions.None);
            await PostMessageId(fetchedEvent.Value.Id, message.Id.ToString());
        }

        [SlashCommand("list", "Get a list of all existing events")]
        public async Task GetAllEventsList(bool ephemeral = true)
        {
            await DeferAsync(ephemeral: ephemeral);
            var response = await _httpClient.GetAsync(_configuration["Database:apiUrl"] + "events");
            if (!response.IsSuccessStatusCode)
            {
                await FollowupAsync("No events found");
                return;
            }

            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(await response.Content.ReadAsStringAsync());
            events.Sort((event1, event2) => event1.Date.CompareTo(event2.Date));
            var builder = new EmbedBuilder().WithTitle("Events");
            foreach (Event eventItem in events)
            {
                builder.AddField($"{eventItem.Title}",
                    (eventItem.Description != null ? $"> {eventItem.Description}\n" : "") +
                    $"> <t:{((DateTimeOffset)eventItem.Date).ToUnixTimeSeconds()}:F>\n" +
                    $"> `id: {eventItem.Id}`\n");
            }
            await FollowupAsync(embed: builder.Build(), ephemeral: ephemeral);

        }

        [SlashCommand("delete", "Delete specified event")]
        public async Task DeleteEventCommand(int id)
        {
            await DeferAsync(ephemeral: true);
            var selectedEvent = await GetEvent(id);
            if (!selectedEvent.HasValue)
            {
                await FollowupAsync("Event not found", ephemeral: true);
                return;
            }
            var embed = CreateEventEmbed(selectedEvent.Value);
            var components = new ComponentBuilder()
                .WithButton("Delete", $"event-delete-{selectedEvent.Value.Id}", ButtonStyle.Danger)
                .WithButton("Cancel", "event-delete-cancel", ButtonStyle.Secondary)
                .Build();
            await FollowupAsync("Delete this event?", embed: embed, components: components, ephemeral: true);
        }

        private Embed CreateEventEmbed(Event embedEvent)
        {
            var builder = new EmbedBuilder()
                .WithTitle(embedEvent.Title)
                .WithDescription(embedEvent.Description)
                .AddField("Date/Time", $"<t:{((DateTimeOffset)embedEvent.Date).ToUnixTimeSeconds()}:F>")
                .WithFooter(new EmbedFooterBuilder().WithText($"(id: {embedEvent.Id})"));

            if (embedEvent.Votes != null)
            {
                foreach (var vote in embedEvent.Votes)
                {
                    string usersText = "";
                    foreach (var id in vote.UserIds)
                    {
                        var user = _client.GetUser(UInt64.Parse(id));
                        if (user != null)
                        {
                            usersText += user.Mention + "\n";
                        }
                    }
                    builder.AddField($"{vote.Name} {vote.Emoji} ({vote.UserIds.Count})", usersText, inline: true);
                }
            }
            return builder.Build();
        }

        private async Task<Nullable<Event>> GetEvent(int id)
        {
            var response = await _httpClient.GetAsync(_configuration["Database:apiUrl"] + $"events/{id}");
            if (!response.IsSuccessStatusCode)
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "EventModule", "Event not found"));
                return null;
            }
            var fetchedEvent = JsonConvert.DeserializeObject<Event>(await response.Content.ReadAsStringAsync());
            return fetchedEvent;
        }

        private async Task PostMessageId(int eventId, string messageId)
        {
            var response = await _httpClient.PostAsync(_configuration["Database:apiUrl"] + $"events/{eventId}/messages", new StringContent(messageId, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                await LogHttpRequestException(response, "Failed to add messageId to event in database");
            }
        }

        private async Task HandleAddReaction(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            var eventId = await GetEventIdFromMessageId(message.Id.ToString());
            if (eventId.HasValue)
            {
                await PostEventVote(eventId.Value, await CreateEventVoteDataFromReaction(reaction));
                var updatedEvent = await GetEvent(eventId.Value);

                await UpdateEventMessageEmbed(message, updatedEvent.Value);
            }
        }

        private async Task HandleRemoveReaction(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            var eventId = await GetEventIdFromMessageId(message.Id.ToString());
            if (eventId.HasValue)
            {
                await RemoveEventVote(eventId.Value, reaction.Emote.ToString(), reaction.UserId.ToString());
                var updatedEvent = await GetEvent(eventId.Value);

                await UpdateEventMessageEmbed(message, updatedEvent.Value);
            }
        }

        private async Task PostEventVote(int eventId, EventVoteData voteData)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(voteData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["Database:apiUrl"] + $"events/{eventId}/votes", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                await LogHttpRequestException(response, "Failed to post event voteData to database");
            }
        }

        private async Task RemoveEventVote(int eventId, string emoji, string userId)
        {
            var response = await _httpClient.DeleteAsync(_configuration["Database:apiUrl"] + $"events/{eventId}/votes/{emoji}/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                await LogHttpRequestException(response, "Failed to delete event vote in database");
            }
        }

        private async Task UpdateEventMessageEmbed(Cacheable<IUserMessage, ulong> message, Event updatedEvent)
        {
            var fetchedMessage = await message.GetOrDownloadAsync();
            await fetchedMessage.ModifyAsync(msg =>
                {
                    var newEmbed = CreateEventEmbed(updatedEvent);
                    msg.Embeds = new Embed[] { newEmbed };
                });
        }

        private async Task GetEmojiList()
        {
            var response = await _httpClient.GetAsync($"https://emoji-api.com/emojis?access_key={_configuration["EmojiApiKey"]}");
            if (!response.IsSuccessStatusCode)
            {
                await LogHttpRequestException(response, "Failed to load emoji list");
                return;
            }
            _emojiList = JsonConvert.DeserializeObject<List<EmojiInfo>>(await response.Content.ReadAsStringAsync());
        }

        private async Task LogHttpRequestException(HttpResponseMessage response, string message)
        {
            await Program.Log(new LogMessage(
                    LogSeverity.Error,
                    "EventModule",
                    message,
                    new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode)));
        }

        private async Task<int?> GetEventIdFromMessageId(string messageId)
        {
            var response = await _httpClient.GetAsync(_configuration["Database:apiUrl"] + $"events/getidfrommessage/{messageId}");
            if (!response.IsSuccessStatusCode)
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "EventModule", "Event not found"));
                return null;
            }
            int fetchedEventId = int.Parse(await response.Content.ReadAsStringAsync());
            return fetchedEventId;
        }

        private async Task<EventVoteData> CreateEventVoteDataFromReaction(SocketReaction reaction)
        {
            string normalEmoji = _emojiList.FirstOrDefault(emoji => emoji.Character == reaction.Emote.ToString()).UnicodeName;
            string emojiName = normalEmoji != null ? normalEmoji : reaction.Emote.Name;
            string emojiChar = reaction.Emote.ToString();

            var voteData = new EventVoteData() { Name = emojiName, Emoji = emojiChar, UserId = reaction.UserId.ToString() };
            return voteData;
        }

        private async Task DeleteEvent(int id)
        {
            var response = await _httpClient.DeleteAsync(_configuration["Database:apiUrl"] + $"events/{id}");
            if (!response.IsSuccessStatusCode)
            {
                await Program.Log(new LogMessage(LogSeverity.Info, "EventModule", "Event not found"));
                return;
            }
        }

        private async Task HandleButtonPress(SocketMessageComponent component)
        {
            if (component.Data.CustomId.Contains("event-delete") && !component.HasResponded)
            {
                string response;
                if (!component.Data.CustomId.Contains("event-delete-cancel"))
                {
                    int eventId = int.Parse(component.Data.CustomId.Split('-')[2]);
                    await DeleteEvent(eventId);
                    response = "Event has been deleted";
                }
                else
                {
                    response = "Event was not deleted";
                }
                await component.UpdateAsync(message =>
                    {
                        message.Content= response;
                        message.Embeds = null;
                        message.Components = null;
                    });
            }
        }
    }
}
