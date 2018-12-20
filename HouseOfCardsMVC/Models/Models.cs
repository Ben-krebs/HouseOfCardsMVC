using System;
using System.Web;
using System.Collections.Generic;

namespace HouseOfCardsMVC.Models
{
    //////// Cache Models //////////////

    public class GameModel
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int Phase { get; set; }
        public DateTime Started { get; set; }
        public string Player_Ids { get; set; }
        public string Event_Ids { get; set; }
        public string Historic_Event_Ids { get; set; } // Cool
        public int Round { get; set; }
        // Collection of players found via their Id
        public PlayerModel[] Players { get; set; }
    }

    public class PlayerModel
    {
        public string Id { get; set; }
        public int Game_Id { get; set; }
        public string Name { get; set; }
        public bool Ready { get; set; }
        public string Dirt { get; set; }
        public string Defense { get; set; }
        public bool Baiting { get; set; }
        public int? SelectedCard { get; set; }
        public string SelectedTarget { get; set; }
        public int PendingScore { get; set; }
        public int Score { get; set; }
        public bool Dirty { get { return String.IsNullOrEmpty(Dirt); } }

        public List<string> Messages { get; set; }
    
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
        public int Score { get; set; }
        public string Target { get; set; }
        public string Category { get; set; }
        public string Defense { get; set; }
        public bool Baiting { get; set; }
    }


}
