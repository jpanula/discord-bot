using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    internal class InteractionHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _handler;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        public InteractionHandlingService(DiscordSocketClient client, InteractionService handler, IServiceProvider services, IConfiguration configuration)
        {
            _client = client;
            _handler = handler;
            _services = services;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            _client.Ready += ReadyAsync;
            _handler.Log += Program.Log;

            await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;
        }

        public async Task ReadyAsync()
        {
            await _handler.RegisterCommandsToGuildAsync(Convert.ToUInt64(_configuration["TestGuild"]), true);
        }

        public async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, interaction);
                var result = await _handler.ExecuteCommandAsync(context, _services);
            }
            catch
            {
                if (interaction.Type is InteractionType.ApplicationCommand)
                {
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }
    }
}
