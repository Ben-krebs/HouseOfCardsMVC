﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseOfCardsMVC.Models
{
    public class PlayerMethods
    {
        public static PlayerModel CreatePlayer(string Session_Id, int Game_Id, string Player_Name)
        {
            var player = new PlayerModel
            {
                Id = Session_Id,
                Name = Player_Name,
                Game_Id = Game_Id,
            };
            return player;
        }

        // Don't think we need this card
        public static PlayerModel ClearPlayer(PlayerModel Player, bool Full = true)
        {
            // Clear the player
            Player.Ready = false;
            Player.SelectedCard = null;
            Player.SelectedTarget = null;

            if (Full)
            {
                Player.PendingDefense = 0;
                Player.PendingScore = 0;
                Player.DefenseType = null;
                Player.Baiting = false;
                Player.Dirt = null;
                Player.Active_Card_Ids.Clear();
                //Clear any old messages
                //Player.Messages.Clear();        
            }

            return Player;
        }

        public static PlayerModel ReadyPlayer(PlayerModel Player, int? SelectedCard, string SelectedTarget)
        {
            Player.SelectedCard = SelectedCard;
            Player.SelectedTarget = SelectedTarget;
            Player.Ready = true;
            return Player;
        }
    }
}