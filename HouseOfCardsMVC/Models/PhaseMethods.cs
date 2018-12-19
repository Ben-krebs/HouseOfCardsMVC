using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseOfCardsMVC.Models
{
    public class PhaseMethods
    {

        /// <summary>
        /// Deal the hands to each player for phase 1
        /// </summary>
        /// <param name="Game"></param>
        /// <param name="Context"></param>
        public static void BeginPhase1(GameModel Game, HttpContextBase Context)
        {
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            foreach (string id in player_Ids)
            {
                var player = Context.Application["Player-" + id] as PlayerModel;
                player.Card_Ids = GenerateHand(1);
                Context.Application["Player-" + id] = player;
            }
            Game.Phase = 1;
            Context.Application["Game-" + Game.Id] = Game;
        }

        /// <summary>
        /// Take the actions that have been performed in phase 1 and apply them to the pending scores and defense states of each player
        /// </summary>
        /// <param name="Game"></param>
        /// <param name="Context"></param>
        public static void BeginPhase2(GameModel Game, HttpContextBase Context)
        {
             
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            Dictionary<string, int> PendingScores = new Dictionary<string, int>();
            PlayerModel[] players = new PlayerModel[player_Ids.Length];

            // First build up the array of card effects
            for (int i = 0; i < player_Ids.Length; i++)
            {
                players[i] = Context.Application["Player-" + player_Ids[i]] as PlayerModel;
                PendingScores.Add(player_Ids[i], 0);
            }

            // Then calculate the card actions
            foreach (var player in players)
            {
                //var player = players.FirstOrDefault(a => a.Id == id);

                // Find the action the player has made this round
                var card = Constants.Cards.GetCard(player.SelectedCard);
                // Apply the self actions
                player.Dirt = card.Dirty ? card.Name : null;
                player.Defense = card.Defense;
                if (card.Target == Constants.List_CardTargets.Self)
                {
                    player.PendingScore += card.Score;
                }
                else if (card.Target == Constants.List_CardTargets.Other)
                {
                    // Store the pending action against its target
                    PendingScores[player.SelectedTarget] += card.Score;
                }
                else if (card.Target == Constants.List_CardTargets.Global)
                {
                    // Store the pending action against its targets
                    foreach(var score in PendingScores.Where(a => a.Key != player.Id))
                    {
                        PendingScores[score.Key] += card.Score;
                    }
                }

                // Create the new hand for the player
                player.Card_Ids = GenerateHand(2);          
            }

            // Finally, apply them and generate the card messages
            foreach (var player in players)
            {
                player.PendingScore -= PendingScores[player.Id];
                player.Messages.Add("X players have been found to be acting illegally.");

                Context.Application["Player-" + player.Id] = player;
            }
            Game.Phase = 2;
            Context.Application["Game-" + Game.Id] = Game;
        }

        /// <summary>
        /// Takes the actions people performed in phase and applies it to their players for investigation
        /// </summary>
        /// <param name="Game"></param>
        /// <param name="Context"></param>
        public static void BeginPhase3(GameModel Game, HttpContextBase Context)
        {
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            List<InvestigationModel> PendingInvestigations = new List<InvestigationModel>();
            PlayerModel[] players = new PlayerModel[player_Ids.Length];

            // First build up the array of card effects
            for (int i = 0; i < player_Ids.Length; i++)
            {
                players[i] = Context.Application["Player-" + player_Ids[i]] as PlayerModel;
            }

            // Then calculate the card actions
            foreach (var player in players)
            {
                // Find the action the player has made this round
                var card = Constants.Cards.GetCard(player.SelectedCard);
                // Apply the self actions
                player.Defense = card.Defense;
                player.Baiting = card.Baiting;

                if (card.Target == Constants.List_CardTargets.Other)
                {
                    // Store the pending action against its target
                    PendingInvestigations.Add(new InvestigationModel { Game_Id = Game.Id, Instigator_Id = player.Id, Target_Id = player.SelectedTarget, Type = card.Category });
                }
                else if (card.Target == Constants.List_CardTargets.Global)
                {
                    // Store the pending action against its targets
                    foreach (var target in players.Where(a => a.Id != player.Id))
                    {
                        PendingInvestigations.Add(new InvestigationModel { Game_Id = Game.Id, Instigator_Id = player.Id, Target_Id = target.Id, Type = card.Category });
                    }
                }

                // Create the new hand for the player
                player.Card_Ids = new int[0];
            }

            // Finally, apply them and generate the card messages
            foreach (var player in players)
            {
                foreach (var investigation in PendingInvestigations.Where(a => a.Instigator_Id == player.Id))
                {
                    var target = players.FirstOrDefault(a => a.Id == investigation.Target_Id);
                    string result = "";
                    if (target.Defense == investigation.Type)
                    {
                        result = target.Name + " is clean";
                    }
                    else
                    {                    
                        switch (investigation.Type)
                        {
                            case "Dirt":
                                result = target.Name + (target.Dirty ? " has dirt on them" : " is clean");
                                break;
                            case "Score":
                                result = target.Name + " is looking to " + (target.PendingScore >= 0 ? "gain" : "looe") + " points of popularity";
                                break;
                            case "Target":
                               // Find out who this player has targeted with their last action
                                break;
                        }

                    }

                    player.Messages.Add(result);
                }

                Context.Application["Player-" + player.Id] = player;
            }
            Game.Phase = 3;
            Context.Application["Game-" + Game.Id] = Game;
        }

        public static void EndRound(GameModel Game, HttpContextBase Context)
        {
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            PlayerModel[] players = new PlayerModel[player_Ids.Length];

            // First build up the array of card effects
            for (int i = 0; i < player_Ids.Length; i++)
            {
                players[i] = Context.Application["Player-" + player_Ids[i]] as PlayerModel;
            }

            // Find out the results of the votes


            // Finally add the new scores to each player
            foreach (var player in players)
            {
                player.Score += player.PendingScore;
                Context.Application["Player-" + player.Id] = player;
            }

            Game.Phase = 1;
            Context.Application["Game-" + Game.Id] = Game;
        }

        public static int[] GenerateHand(int Phase)
        {
            //CardModel[] hand = new CardModel[4];
            int[] ids = new int[4];
            Random r = new Random();

            Card[] cards = Phase == 1 ? Constants.Cards.Phase1 : Constants.Cards.Phase2;

            for (int i = 0; i < 4; i++)
            {
                int j = r.Next(0, cards.Length);
                while (ids.Contains(j))
                {
                    j = r.Next(0, cards.Length);
                }

                ids[i] = j;
                //hand[i] = new CardModel { Id = cards[j].Id };

            }
            return ids;
        }
    }
}