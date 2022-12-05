using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Modules;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DiscordBot
{
    internal class Program
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;

        private readonly DiscordSocketConfig _socketConfig = new () {
            GatewayIntents = GatewayIntents.GuildMessageReactions
        };

        private readonly InteractionServiceConfig _interactionConfig = new()
        {
            LogLevel = LogSeverity.Info
        };

        private Program()
        {
            _configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            _services = new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton(_socketConfig)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), _interactionConfig))
                .AddSingleton<InteractionHandlingService>()
                .BuildServiceProvider();
        }

        public static Task Main(string[] args) => new Program().MainAsync();
        public async Task MainAsync()
        {
            var client = _services.GetRequiredService<DiscordSocketClient>();
            client.Log += Log;

            await _services.GetRequiredService<InteractionHandlingService>().InitializeAsync();

            await client.LoginAsync(TokenType.Bot, _configuration["BotToken"]);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        public static Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}