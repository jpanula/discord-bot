using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Magic8BallModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private struct Magic8BallResponseData
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("content")]
            public string Content { get; set; }
            [JsonProperty("type")]
            public int Type { get; set; }
        }

        public Magic8BallModule(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [SlashCommand("8ball", "Receive an answer to a yes/no question")]
        public async Task Magic8BallResponse(string question= "", bool ephemeral = false)
        {
            var httpResponse = await _httpClient.GetAsync(_configuration["Database:apiUrl"] + @"magic8ball/random/weighted");
            var magic8BallResponse = JsonConvert.DeserializeObject<Magic8BallResponseData>(await httpResponse.Content.ReadAsStringAsync());
            if (question == null || question == "")
            {
                await RespondAsync(magic8BallResponse.Content, ephemeral: ephemeral);
            }
            else
            {
                await RespondAsync($"{this.Context.User.Mention} asked: {question}\nAnswer: {magic8BallResponse.Content}.", ephemeral: ephemeral, allowedMentions: Discord.AllowedMentions.None);
            }
        }
    }
}
