using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DiscordBot
{
    internal class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        private readonly DiscordSocketClient _client;

        private readonly InteractionService _interactions;
        private readonly IServiceProvider _services;

        private Program()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
            });

            _interactions = new InteractionService(_client, new InteractionServiceConfig
            {
                LogLevel = LogSeverity.Info,
            });

            _client.Log += Log;
            _interactions.Log += Log;

            _services = ConfigureServices();
        }
        private static IServiceProvider ConfigureServices()
        {
            // Setup DI container here
            var map = new ServiceCollection();
            return map.BuildServiceProvider();
        }

        private static Task Log(LogMessage message)
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

        public async Task MainAsync()
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            await InitCommands();

            var token = configuration["BotToken"];

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private async Task InitCommands()
        {
            await _interactions.AddModuleAsync<PingModule>(_services);
        }
    }
}