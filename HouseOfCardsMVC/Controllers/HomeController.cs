using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HouseOfCardsMVC.Models;
namespace HouseOfCardsMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(Session["Game_Id"] != null)
            {
                return RedirectToAction("Index", "Game");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Partial_Game(string Partial, int Game_Id)
        {
            GameModel model = HttpContext.Application["Game-" + Game_Id] as GameModel;
            return PartialView("~/Views/" + Partial + ".cshtml", model);
        }

        public ActionResult Partial_Player(string Partial)
        {
            PlayerModel model = HttpContext.Application["Player-" + Session.SessionID] as PlayerModel;
            return PartialView("~/Views/" + Partial + ".cshtml", model);
        }

        public ActionResult Partial_Card(string Partial, int Card_Id)
        {
            return PartialView("~/Views/" + Partial + ".cshtml", Constants.Cards.GetCard(Card_Id));
        }
    }
}