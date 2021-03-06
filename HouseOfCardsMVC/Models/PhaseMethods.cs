﻿using System;
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
            PlayerModel[] players = new PlayerModel[player_Ids.Length];
            HashSet<string> scores = new HashSet<string>();
            scores.Add("Round " + Game.Round);
            for (int i = 0; i < player_Ids.Length; i++)
            {
                players[i] = Context.Application["Player-" + player_Ids[i]] as PlayerModel;
                scores.Add(players[i].Name + ": " + players[i].Score);
            }

            // Then calculate the card actions
            foreach (var player in players)
            {
                // Clear the player
                PlayerMethods.ClearPlayer(player);

                // Add the current scores
                player.Messages = scores;

                player.Card_Ids = GenerateHand(1, Game.Random);
                Context.Application["Player-" + player.Id] = player;
            }

            Game.Phase = 1;
            Game.DirtCount = 0;
            Game.Active_Card_Ids.Clear();
            Context.Application["Game-" + Game.Id] = Game;
        }

        /// <summary>
        /// Take the actions that have been performed in phase 1 and apply them to the pending scores and defense states of each player
        /// </summary>
        /// <param name="Game"></param>
        /// <param name="Context"></param>
        public static int BeginPhase2(GameModel Game, HttpContextBase Context)
        {
            Game.Active_Card_Ids.Clear();

            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            Dictionary<string, int> PendingAttacks = new Dictionary<string, int>();
            PlayerModel[] players = new PlayerModel[player_Ids.Length];
            int dirtCount = 0;

            // First build up the array of card effects
            for (int i = 0; i < player_Ids.Length; i++)
            {
                players[i] = Context.Application["Player-" + player_Ids[i]] as PlayerModel;
                PendingAttacks.Add(player_Ids[i], 0);
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
                        PendingAttacks[player.SelectedTarget] += card.Attack;
                    }
                    else if (card.Target == Constants.List_CardTargets.Global)
                    {
                        // Store the pending action against its targets
                        foreach (var key in player_Ids.Where(a => a != player.Id))
                        {
                            PendingAttacks[key] += card.Attack;
                        }
                    }
                    if(card.Effect_Id > 0)
                    {
                        Game.Active_Card_Ids.Add(card.Effect_Id);

                        switch (card.Effect_Id)
                        {
                            case 8:
                                players.First(a => a.Id == player.SelectedTarget).Active_Card_Ids.Add(8);
                                break;
                            case 9:
                                PendingAttacks[player.Id] += card.Attack;
                                break;
                        }

                    }                    
                }

                // Clear the player
                PlayerMethods.ClearPlayer(player, false);
            }

            // Finally, apply them and generate the card messages
            foreach (var player in players)
            {
                //Clear any old messages
                player.Messages.Clear();

                // Create the new hand for the player
                player.Card_Ids = GenerateHand(2, Game.Random);

                PendingAttacks[player.Id] -= player.PendingDefense;

                if (!player.Dirty && Game.Active_Card_Ids.Contains(6))
                {
                    PendingAttacks[player.Id] += 100;
                }
                if (player.Dirty && Game.Active_Card_Ids.Contains(7))
                {
                    PendingAttacks[player.Id] += 300;
                }
                if (PendingAttacks[player.Id] > 0)
                {
                    player.PendingScore -= PendingAttacks[player.Id];
                }

                player.PendingDefense = 0;

                if (dirtCount > 0) { player.Messages.Add(dirtCount + " players have been found to be acting illegally."); }
                Context.Application["Player-" + player.Id] = player;
            }
            Game.Phase = 2;
            Game.DirtCount = dirtCount;
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
                    player.DefenseType += card.SubType;

                    if (card.Type == "Investigation")
                    {
                        if (card.Target == Constants.List_CardTargets.Other)
                        {
                            // Store the pending action against its target
                            PendingInvestigations.Add(new InvestigationModel { Game_Id = Game.Id, Instigator_Id = player.Id, Target_Id = player.SelectedTarget, Type = card.SubType });
                        }
                        else if (card.Target == Constants.List_CardTargets.Global)
                        {
                            // Store the pending action against its targets
                            foreach (var target in players.Where(a => a.Id != player.Id))
                            {
                                PendingInvestigations.Add(new InvestigationModel { Game_Id = Game.Id, Instigator_Id = player.Id, Target_Id = target.Id, Type = card.SubType });
                            }
                        }
                    }
                    else if (card.Type == "Twist")
                    {
                        if (card.Effect_Id > 0)
                        {
                            Game.Active_Card_Ids.Add(card.Effect_Id);
                            switch (card.Effect_Id)
                            {
                                case 13: // Baiting card
                                    player.Baiting = true;
                                    break;
                            }
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
                    if (target.DefenseType == investigation.Type || Game.Active_Card_Ids.Contains(1))
                    {
                        switch (investigation.Type)
                        {
                            case "Dirt":
                                result = target.Name + " is clean";
                                break;
                            default:  result = "The investigation returned nothing of note";
                                break;       
                        }                        
                    }
                    else
                    {                    
                        switch (investigation.Type)
                        {
                            case "Dirt":
                                result = target.Name + (target.Dirty ? " has dirt on them" : " is clean");
                                break;
                            case "Score":
                                result = target.Name + " is looking to " + (target.PendingScore >= 0 ? "gain " : "lose ") + target.PendingScore + " points of popularity";
                                break;
                            case "Target":
                                string attackedPlayer = "an unknown player";
                                if (String.IsNullOrEmpty(target.SelectedTarget))
                                {
                                    switch (Constants.Cards.GetCard(player.SelectedCard).Target)
                                    {
                                        case "Global": attackedPlayer = "everyone"; break;
                                        case "Self": attackedPlayer = "themself"; break;
                                        case "Other": attackedPlayer = "another player"; break;
                                    }
                                }
                                else
                                {
                                    attackedPlayer = players.First(a => a.Id == target.SelectedTarget).Name;
                                }
                                result = target.Name + " targeted " + attackedPlayer + " this round";
                                break;
                            case "History":
                                result = target.Name + " has made " + target.HistoricDirt + " illegal actions in this game";
                                break;
                        }
                    }
                    player.Messages.Add(result);
                }

                if (vote_master_id == player.Id)
                {
                    player.Voting = true;
                }
                else
                {
                    player.Messages.Add("Another player is controlling the vote, discuss as a group and figure out who to accuse of foul play");
                }

                // Clear the player
                PlayerMethods.ClearPlayer(player, false);

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

            bool gameOver = false;
            // Finally add the new scores to each player
            foreach (var player in players)
            {


                if(player.Dirty)
                {
                    if((Game.Vote_Ids ?? "").Contains(player.Id))
                    {
                        if (player.Baiting)
                        {
                            player.PendingScore += 300;
                        }
                        else
                        {
                            player.PendingScore -= 600;
                        }           
                    }
                }
                else 
                {
                    if ((player.Baiting) && (Game.Vote_Ids ?? "").Contains(player.Id))
                    {
                        player.PendingScore += 400;
                    }
                    player.PendingScore += (100 * correctVotes);
                    player.PendingScore -= (100 * wrongVotes);
                }

                //Clear any old messages
                player.Messages.Clear();

                player.Score += player.PendingScore;
                if(player.Score > 5000)
                {
                    gameOver = true;
                }
                player.Messages.Add("Your popularity " + (player.PendingScore >= 0 ? "increased by " : "dropped by ") + player.PendingScore + " points in the last round, your current score is " + player.Score);
    
                Context.Application["Player-" + player.Id] = player;
            }

            Game.Phase = 1;
            Game.Round += 1;
            Context.Application["Game-" + Game.Id] = Game;


            return gameOver ? "Game OVer" : (String.IsNullOrEmpty(Game.Vote_Ids) ? "No Vote" : (correctVotes == 0 ? "Wrong" : wrongVotes == 0 ? "Correct" : "Partial"));
        }

        public static int[] GenerateHand(int Phase, Random r)
        {
            //CardModel[] hand = new CardModel[4];
            int[] ids = new int[4];

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