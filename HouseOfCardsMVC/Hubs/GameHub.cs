using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace HouseOfCardsMVC.Hubs
{
    public class GameHub : Hub
    {
        public void Redirect(string url)
        {
            // Call the redirect method for all clients
            Clients.Others.redirectToUrl(url);
            Clients.Caller.redirectToUrl(url);
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
            await Groups.Add(ConnectionId, groupName);
            await Clients.Group(groupName).AlertOnJoin(Context.ConnectionId + ": " + PlayerName + " added to group");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
            await Clients.Group(groupName).AlertOnLeave(Context.ConnectionId + " removed from group");
        }
    }

    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
}