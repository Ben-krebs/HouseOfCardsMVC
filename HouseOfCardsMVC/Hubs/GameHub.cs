using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace HouseOfCardsMVC.Hubs
{
    public class GameHub : Hub
    {
        public void Redirect(string url)
        {
            // Call the redirect method for all clients
            Clients.Others.redirect(url);
            Clients.Caller.redirect(url);
        }

        public void AlertJoin(string Name)
        {
            // Call the redirect method for all clients
            Clients.All.AlertJoin(Name);
        }
    }
}