using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class PingModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Pings the bot")]
        public async Task Ping() => await RespondAsync($"pong ({Context.Client.Latency} ms)");
    }
}
