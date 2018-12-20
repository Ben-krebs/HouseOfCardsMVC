using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HouseOfCardsMVC.Models;

namespace HouseOfCardsMVC.Controllers
{
    public class GameController : Controller
    {
        #region Actions

        // GET: Game
        public ActionResult Index()
        {
            if(Session["Game_Id"] == null)
            {
                return RedirectToAction("Start");
            }
            else
            {
                GameModel game = HttpContext.Application["Game-" + Session["Game_Id"]] as GameModel;
                PlayerModel player = HttpContext.Application["Player-" + Session.SessionID] as PlayerModel;
                game.PopulatePlayers(HttpContext);
                player.Game = game;

                switch (game.Phase)
                {
                    case 0: return View("~/Views/Game/Start.cshtml", player);
                    case 1: return View("~/Views/Game/Scheme.cshtml", player);
                    case 2: return View("~/Views/Game/Investigate.cshtml", player);
                    case 3: return View("~/Views/Game/Accuse.cshtml", player);
                }
            }
            return View();
        }

        public ActionResult Start()
        {
            if (Session["Game_Id"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index");
            }      
        }

        #endregion

        #region Methods
        public GameModel CreateGameHandler(string Player_Name)
        {
            var game = GameMethods.CreateGame();

            var player = PlayerMethods.CreatePlayer(HttpContext.Session.SessionID, game.Id, Player_Name);

            // Add the player Id to the game
            game.Player_Ids = "," + player.Id;
            HttpContext.Application["Game-" + game.Id] = game;

            player.Game_Id = game.Id;
            HttpContext.Application["Player-" + player.Id] = player;

            Session["Game_Id"] = game.Id;
           // new Hubs.GameHub().JoinGroup(game.Id.ToString());

            return game;
        }

        public bool JoinGameHandler(int Game_Id, string Player_Name)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;
            if(game != null && game.Phase == 0)
            {
                var player = PlayerMethods.CreatePlayer(HttpContext.Session.SessionID, Game_Id, Player_Name);

                // Add the player Id to the game

                string player_Ids = game.Player_Ids;
                if (!(player_Ids ?? "").Contains(player.Id))
                {
                    player_Ids += "," + player.Id;
                }
                game.Player_Ids = player_Ids;
                HttpContext.Application["Game-" + game.Id] = game;

                player.Game_Id = game.Id;
                HttpContext.Application["Player-" + player.Id] = player;

                Session["Game_Id"] = game.Id;
               // new Hubs.GameHub().JoinGroup(game.Id.ToString());

                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartGameHandler(int Game_Id)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;
            game = GameMethods.StartGame(game);
            HttpContext.Application["Game-" + game.Id] = game;

            BeginPhase1Handler(game.Id);
        }

        public void EndGameHandler(int Game_Id)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;
            GameMethods.EndGame(game, HttpContext);

            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All;
            hubContext.Redirect("/Game/");
        }

        public void BeginPhase1Handler(int Game_Id)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;
            PhaseMethods.BeginPhase1(game, HttpContext);

            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            hubContext.Clients.All.redirect("/Game/");
        }

        public void BeginPhase2Handler(int Game_Id)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;
            // If no illegal actions ahve happened then move to the next round
            if (PhaseMethods.BeginPhase2(game, HttpContext) == 0)
            {
                PhaseMethods.EndRound(game, HttpContext);
            }

            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All;
            hubContext.Redirect("/Game/");
        }

        public void BeginPhase3Handler(int Game_Id)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;
            PhaseMethods.BeginPhase3(game, HttpContext);

            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All;
            hubContext.Redirect("/Game/");
        }

        public string EndRoundHandler(int Game_Id, string Vote_Ids)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;
            game.Vote_Ids = Vote_Ids;
            var result = PhaseMethods.EndRound(game, HttpContext);

            return result;
            //var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All;
            //hubContext.Redirect("/Game/");
        }




        /// <summary>
        /// When a player confirms they are ready to proceed with the phase this fires, it checks if there are any other players we are waiting on, if not it proceeds with the round
        /// </summary>
        /// <param name="Game_Id"></param>
        public int ReadyHandler(int Game_Id, string Player_Id, int? Selected_Card_Id, string Selected_Target_Id, int Phase)
        {
            var game = HttpContext.Application["Game-" + Game_Id] as GameModel;

            // Assign the selected action to the player, and mark them as ready
            var currentPlayer = HttpContext.Application["Player-" + Player_Id] as PlayerModel;
            currentPlayer = PlayerMethods.ReadyPlayer(currentPlayer, Selected_Card_Id, Selected_Target_Id);
            HttpContext.Application["Player-" + currentPlayer.Id] = currentPlayer;

            // run through all the other players of the game and check their readiness
            string[] player_Ids = game.Player_Ids.SplitAndTrim(',');
            int PendingPlayers = 0;
            foreach(string id in player_Ids)
            {
                var player = HttpContext.Application["Player-" + id] as PlayerModel;
                if (!player.Ready)
                {
                    PendingPlayers++;
                }
            }
            // if there are no players still to go, begin the next phase
            if(PendingPlayers == 0)
            {
                switch (Phase)
                {
                    case 1:
                        BeginPhase2Handler(Game_Id);
                        break;
                    case 2:
                        BeginPhase3Handler(Game_Id);
                        break;
                }
            }

            return PendingPlayers;
        }
        #endregion
    }
}