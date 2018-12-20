using System;
using System.Web;
using System.Collections.Generic;

namespace HouseOfCardsMVC.Models
{
    //////// Cache Models //////////////

    public class GameModel
    {
        public GameModel()
        {
            this.Votes = new HashSet<VoteModel>();
            this.Players = new HashSet<PlayerModel>();
            this.Active_Card_Ids = new HashSet<int>();
        }

        public int Id { get; set; }
        public bool Active { get; set; }
        public int Phase { get; set; }
        public DateTime Started { get; set; }
        public string Player_Ids { get; set; }
        public string Event_Ids { get; set; }
        public string Historic_Event_Ids { get; set; } // Cool
        public int Round { get; set; }

        public HashSet<int> Active_Card_Ids { get; set; }
        // Collection of players found via their Id
        public HashSet<PlayerModel> Players { get; set; }
        public HashSet<VoteModel> Votes { get; set; }

        public void PopulatePlayers(HttpContextBase Context)
        {
            foreach (var id in this.Player_Ids.SplitAndTrim(','))
            {
                var gamePlayer = Context.Application["Player-" + id] as PlayerModel;
                this.Players.Add(gamePlayer);
            }
        }
    }

    public class PlayerModel
    {
        public PlayerModel()
        {
            this.Messages = new HashSet<string>();
        }

        public string Id { get; set; }
        public int Game_Id { get; set; }
        public string Name { get; set; }
        public bool Ready { get; set; }
        public string Dirt { get; set; }
        public string Defense { get; set; }
        public bool Baiting { get; set; }
        public bool Voting { get; set; }

        public int? SelectedCard { get; set; }
        public string SelectedTarget { get; set; }

        public int PendingDefense { get; set; }
        public int PendingScore { get; set; }

        public int Score { get; set; }
        public bool Dirty { get { return String.IsNullOrEmpty(Dirt); } }

        public HashSet<string> Messages { get; set; }
    
        public int[] Card_Ids { get; set; }
        // Game found via Id
        public GameModel Game { get; set; }
    }

    public class CardModel
    {
        public int Id { get; set; }
        public bool Selected { get; set; }
        public string Target { get; set; }
    }

    public class VoteModel
    {
        public string Id { get; set; }
        public int Game_Id { get; set; }
        public string Voter_Ids { get; set; }
        public string Target { get; set; }
        public int Votes { get { return (Voter_Ids ?? "").Split(',').Length; } }
    }

    public class InvestigationModel
    {
        public int Game_Id { get; set; }
        public string Target_Id { get; set; }
        public string Instigator_Id { get; set; }
        public string Type { get; set; }
    }


    /////////// DataTypes //////////////
    ///

    public class Card
    {
        public int Id { get; set; }
        public int Phase { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Dirty { get; set; }

        public int Attack { get; set; }
        public int Score { get; set; }
        public int Defense { get; set; }

        public string Target { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }

        public int Effect_Id { get; set; }
    }


    public class MessageViewModel
    {
        public MessageViewModel()
        {
            this.Buttons = new HashSet<ButtonViewModel>();
        }

        public string Title { get; set; }
        public string Body { get; set; }

        public HashSet<ButtonViewModel> Buttons {get;set;}
    }

    public class ButtonViewModel
    {
        public string Title { get; set; }
        public string Action { get; set; }
    }

}
