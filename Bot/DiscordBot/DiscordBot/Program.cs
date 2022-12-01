using Discord;

namespace DiscordBot
{
    internal class Program
    {
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        static void Main(string[] args) => new Program().MainAsync();
        public async Task MainAsync()
        {

        }
    }
}