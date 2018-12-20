using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace HouseOfCardsMVC.Models
{
    public class GameMethods
    {
        public static GameModel CreateGame()
        {
            var game = new GameModel {
                Id = new Random().Next(1, 10000),
                Active = false,
                Phase = 0,
                Round = 0,
                Random = new Random()
            };
            return game;
        }

        public static GameModel StartGame(GameModel game)
        {
            game.Active = true;
            game.Round = 1;
            game.Phase = 1;
            game.Started = DateTime.Now;
            return game;
        }

        public static Dictionary<string, int> WinGame(GameModel Game, HttpContextBase Context)
        {
            //List the player scores
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            Dictionary<string, int> results = new Dictionary<string, int>();
            foreach (string id in player_Ids)
            {
                var player = Context.Application["Player-" + id] as PlayerModel;
                results.Add(player.Name, player.Score);
            }
            return results;
        }

        public static void EndGame(GameModel Game, HttpContextBase Context)
        {
            //Remove all the players
            string[] player_Ids = Game.Player_Ids.SplitAndTrim(',');
            foreach (string id in player_Ids)
            {
                var player = Context.Application["Player-" + id] as PlayerModel;
                PlayerMethods.ClearPlayer(player);
                Context.Application["Player-" + id] = player;
            }

            //Remove the game
            Context.Application["Game-" + Game.Id] = null;
        }
    }
}