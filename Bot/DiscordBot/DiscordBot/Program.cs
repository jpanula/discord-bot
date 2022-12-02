using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DiscordBot
{
    internal class Program
    {
        private DiscordSocketClient _client;

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task CreateCommands()
        {
            var globalPingCommand = new SlashCommandBuilder();
            globalPingCommand.WithName("ping");
            globalPingCommand.WithDescription("Pings the bot");

            try
            {
                await _client.CreateGlobalApplicationCommandAsync(globalPingCommand.Build());
            }
            catch (ApplicationCommandException exception)
            {
                await Log(new LogMessage(LogSeverity.Error, "CreateCommands", JsonConvert.SerializeObject(exception.Errors, Formatting.Indented), exception));
            }
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "ping":
                    command.RespondAsync("pong");
                    break;
                default:
                    command.RespondAsync("Unrecognized command");
                    break;
            }
        }

        public static Task Main(string[] args) => new Program().MainAsync();
        public async Task MainAsync()
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += CreateCommands;
            _client.SlashCommandExecuted += SlashCommandHandler;

            var token = configuration["BotToken"];

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();




            await Task.Delay(-1);
        }
    }
}