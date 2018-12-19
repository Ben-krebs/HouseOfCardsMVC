using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace HouseOfCardsMVC.Models
{
    public class CardMethods
    {
        public static void ActivateCard_Phase1(PlayerModel Player)
        {
            Card card = Constants.Cards.GetCard(Player.SelectedCard);

            Player.Dirt = card.Dirty ? card.Name : null;
            Player.Defense = card.Defense;
            if(card.Target == Constants.List_CardTargets.Self)
            {
                Player.PendingScore = card.Score;
            }
        }
    }
}