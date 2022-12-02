using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    internal class PingModule : InteractionModuleBase
    {
        [SlashCommand("ping", "Pings the bot")]
        public async Task Ping() => await RespondAsync("pong");
    }
}
