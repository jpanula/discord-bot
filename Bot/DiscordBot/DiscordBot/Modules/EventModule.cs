using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class EventModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private List<ulong> _eventMessageIds = new List<ulong>();

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

        public EventModule(HttpClient httpClient, IConfiguration configuration, DiscordSocketClient client)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _client = client;
        }

        [SlashCommand("event", "Create an event people can vote on")]
        public async Task CreateEvent(
            string title,
            string date,
            string description = null)
        {
            await DeferAsync();
            var dateTime = DateTime.Parse(date);

            var eventData = new EventData() { Title = title, Description = description, Date = dateTime.ToUniversalTime() };
            var httpContent = new StringContent(JsonConvert.SerializeObject(eventData), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(_configuration["Database:apiUrl"] + "events", httpContent);
            if (!httpResponse.IsSuccessStatusCode)
            {
                await Program.Log(new LogMessage(
                    LogSeverity.Error,
                    "EventModule",
                    "Failed to add event to database",
                    new HttpRequestException("Failed to add event to database", null, httpResponse.StatusCode)));
                await FollowupAsync("Failed to add event to database", ephemeral: true);
                return;
            }

            var embed = eventEmbed(JsonConvert.DeserializeObject<Event>(await httpResponse.Content.ReadAsStringAsync()));
            var message = await FollowupAsync(embed: embed, allowedMentions: Discord.AllowedMentions.None);
            _eventMessageIds.Add(message.Id);
        }

        [SlashCommand("getevent", "Get info of a specific event")]
        public async Task GetEvent(int id)
        {
            await DeferAsync();
            var httpResponse = await _httpClient.GetAsync(_configuration["Database:apiUrl"] + $"events/{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                await FollowupAsync("Event not found", ephemeral: true);
                return;
            }
            var embed = eventEmbed(JsonConvert.DeserializeObject<Event>(await httpResponse.Content.ReadAsStringAsync()));
            var message = await FollowupAsync(embed: embed, allowedMentions: Discord.AllowedMentions.None);
            _eventMessageIds.Add(message.Id);
        }

        private Embed eventEmbed(Event embedEvent)
        {
            var builder = new EmbedBuilder()
                .WithTitle(embedEvent.Title)
                .WithDescription(embedEvent.Description)
                .AddField("Date/Time", $"<t:{((DateTimeOffset)embedEvent.Date).ToUnixTimeSeconds()}:F>");

            if (embedEvent.Votes != null)
            {
                foreach (var vote in embedEvent.Votes)
                {
                    builder.AddField($"<:{vote.Name}:{vote.Emoji}>", vote.UserIds.Select(id => _client.GetUser(UInt64.Parse(id)).Mention + "\n"), inline: true);
                }
            }
            return builder.Build();
        }
    }
}
