using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace HouseOfCardsMVC
{
    public class GameHub : Hub
    {
        public void Redirect(string url)
        {
            // Call the redirect method for all clients
            Clients.All.redirectToUrl(url);
        }

        public void AlertJoin(string Name)
        {
            // Call the redirect method for all clients
            Clients.All.AlertOnJoin(Name);
        }

        public void AlertLeave(string Name)
        {
            // Call the redirect method for all clients
            Clients.All.AlertOnLeave(Name);
        }

        public async Task JoinGroup(string groupName, string ConnectionId, string PlayerName)
        {
            //var game = HttpContext.Current.Application["Game-" + groupName] as Models.GameModel;
            //foreach(var id in game.Player_Ids.SplitAndTrim(','))
            //{
            //    var player = HttpContext.Current.Application["Player-" + id] as Models.PlayerModel;
            //    await Clients.All.AlertOnJoin(Context.ConnectionId + ": " + PlayerName + " added to group");
            //}
            await Groups.Add(ConnectionId, groupName);
            await Clients.Group(groupName).AlertOnJoin(PlayerName + " has joined");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
            await Clients.Group(groupName).AlertOnLeave(Context.ConnectionId + " has left");
        }
    }
}