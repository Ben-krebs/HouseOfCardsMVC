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

                // Clear the player
                PlayerMethods.ClearPlayer(player);

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
        public static int BeginPhase2(GameModel Game, HttpContextBase Context)
        {
             
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            Dictionary<string, int> PendingScores = new Dictionary<string, int>();
            PlayerModel[] players = new PlayerModel[player_Ids.Length];
            int dirtCount = 0;

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
                if (player.SelectedCard != null)
                {
                    // Find the action the player has made this round
                    var card = Constants.Cards.GetCard(player.SelectedCard);
                    // Apply the self actions
                    player.Dirt = card.Dirty ? card.Name : null;
                    dirtCount += player.Dirty ? 1 : 0;

                    player.PendingDefense += card.Defense;
                    player.PendingScore += card.Score;

                    if (card.Target == Constants.List_CardTargets.Other)
                    {
                        // Store the pending action against its target
                        PendingScores[player.SelectedTarget] += card.Attack;
                    }
                    else if (card.Target == Constants.List_CardTargets.Global)
                    {
                        // Store the pending action against its targets
                        foreach (var score in PendingScores.Where(a => a.Key != player.Id))
                        {
                            PendingScores[score.Key] += card.Attack;
                        }
                    }
                }
                //Clear any old messages
                player.Messages.Clear();

                // Create the new hand for the player
                player.Card_Ids = GenerateHand(2);          
            }

            // Finally, apply them and generate the card messages
            foreach (var player in players)
            {
                player.PendingScore -= PendingScores[player.Id];
                if (dirtCount > 0) { player.Messages.Add(dirtCount + " players have been found to be acting illegally."); }
                Context.Application["Player-" + player.Id] = player;
            }
            Game.Phase = 2;
            Context.Application["Game-" + Game.Id] = Game;

            return dirtCount;
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
                if (player.SelectedCard != null)
                {
                    // Find the action the player has made this round
                    var card = Constants.Cards.GetCard(player.SelectedCard);
                    // Apply the self actions
                    //player.Defense += card.e;

                    if (card.Target == Constants.List_CardTargets.Other)
                    {
                        // Store the pending action against its target
                        PendingInvestigations.Add(new InvestigationModel { Game_Id = Game.Id, Instigator_Id = player.Id, Target_Id = player.SelectedTarget, Type = card.Type });
                    }
                    else if (card.Target == Constants.List_CardTargets.Global)
                    {
                        // Store the pending action against its targets
                        foreach (var target in players.Where(a => a.Id != player.Id))
                        {
                            PendingInvestigations.Add(new InvestigationModel { Game_Id = Game.Id, Instigator_Id = player.Id, Target_Id = target.Id, Type = card.Type });
                        }
                    }
                }
                //Clear any old messages
                player.Messages.Clear();

                // Clear the hand for the player
                player.Card_Ids = new int[0];
            }

            // Finally, apply them and generate the card messages
            int index = new Random().Next(0, player_Ids.Length);
            string vote_master_id = player_Ids[index];

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
                                result = target.Name + " is looking to " + (target.PendingScore >= 0 ? "gain" : "lose") + " points of popularity";
                                break;
                            case "Target":
                               // Find out who this player has targeted with their last action
                                break;
                        }
                    }
                    player.Messages.Add(result);

                    if(vote_master_id == player.Id)
                    {
                        player.Voting = true;
                    }
                    else
                    {
                        player.Messages.Add("Anoter player is controlling the vote, discuss as a group and figure out who to accuse of foul play");
                    }
                }

                Context.Application["Player-" + player.Id] = player;
            }
            Game.Phase = 3;
            Context.Application["Game-" + Game.Id] = Game;
        }

        public static string EndRound(GameModel Game, HttpContextBase Context)
        {
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            PlayerModel[] players = new PlayerModel[player_Ids.Length];

            // First build up the array of card effects
            for (int i = 0; i < player_Ids.Length; i++)
            {
                players[i] = Context.Application["Player-" + player_Ids[i]] as PlayerModel;
            }

            int correctVotes = 0;
            int wrongVotes = 0;

            if (String.IsNullOrEmpty(Game.Vote_Ids))
            {
                // No vote
            }
            else
            {
                // Find out the results of the votes
                foreach (var vote_id in Game.Vote_Ids.SplitAndTrim(','))
                {
                    bool dirty = players.First(a => a.Id == vote_id).Dirty;
                    if (dirty)
                    {
                        correctVotes++;
                    }
                    else
                    {
                        wrongVotes++;
                    }
                }
            }

            // Finally add the new scores to each player
            foreach (var player in players)
            {
                if(player.Dirty)
                {
                    if((Game.Vote_Ids ?? "").Contains(player.Id))
                    {
                        player.PendingScore -= 600;
                    }
                }
                else 
                {
                    player.PendingScore += (100 * correctVotes);
                    player.PendingScore -= (100 * wrongVotes);
                }

                player.Score += player.PendingScore;
                Context.Application["Player-" + player.Id] = player;
            }

            Game.Phase = 1;
            Context.Application["Game-" + Game.Id] = Game;


            return (String.IsNullOrEmpty(Game.Vote_Ids) ? "No Vote" : (correctVotes == 0 ? "Wrong" : wrongVotes == 0 ? "Correct" : "Partial"));
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

                ids[i] = cards[j].Id;
            }
            return ids;
        }
    }
}