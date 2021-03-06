﻿using System;
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
            Clients.All.AlertOnJoin(Name);
        }

        public void AlertLeave(string Name)
        {
            Clients.All.AlertOnLeave(Name);
        }

        public void AlertAccuse(string Name, string Id)
        {
            Clients.All.AlertOnAccuse(Name, Id);
        }

        public void AlertAcquit(string Id)
        {
            Clients.All.AlertOnAcquit(Id);
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

        public async Task LeaveGroup(string groupName, string Player_Id)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
            await Clients.Group(groupName).AlertOnLeave(Context.ConnectionId + " has left");

            var game = HttpContext.Current.Application["Game-" + groupName] as Models.GameModel;
            game.Player_Ids.Replace("," + Player_Id, "");

            HttpContext.Current.Application["Game-" + groupName] = game;
            HttpContext.Current.Application["Player-" + Player_Id] = null;
        }
    }
}