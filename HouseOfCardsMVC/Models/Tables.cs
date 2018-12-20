using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace HouseOfCardsMVC.Models
{
    public class Constants
    {
        public class Cards
        {
            public static readonly Card[] Phase1 = CardTypes.Where(a => a.Phase == 1).ToArray();
            public static readonly Card[] Phase2 = CardTypes.Where(a => a.Phase == 2).ToArray();
          
            public static Card GetCard(int? Id)
            {
                return CardTypes.First(a => a.Id == Id);
            }
        }

        public static List<Card> CardTypes
        {
            get
            {
                return new List<Card>
                {
                    new Card { Id = -1,  Category = "", Description = "Someone intercepted your card, you have one less option to play this round ", Name = "Blank", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },

                    new Card { Id = 0,  Category = "", Description = "a", Name = "Collude with the Russians", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 1,  Category = "", Description = "q", Name = "Hack into another players emails", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 2,  Category = "", Description = "w", Name = "Evade tax", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 3,  Category = "", Description = "e", Name = "Plant incriminating documents in someone’s office", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 4,  Category = "", Description = "r", Name = "Accept undisclosed donations", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 5,  Category = "", Description = "t", Name = "Publically lie", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 6,  Category = "", Description = "y", Name = "Organise assassination ", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 7,  Category = "", Description = "Threaten to nuke another country", Name = "Flaunt your big red button", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 8,  Category = "", Description = "i", Name = "Waterboard someone for information", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 9,  Category = "", Description = "o", Name = "Hire an impersonator to do something bad", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 10, Category = "", Description = "p", Name = "Cut off your opponents finger and feed it to your dog to get rid of the evidence", Dirty = true, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },

                    new Card { Id = 11, Category = "", Description = "a", Name = "Appear on a talk show", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 12, Category = "", Description = "s", Name = "Win a debate against another player", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 13, Category = "", Description = "d", Name = "Post an unsavoury video of another player", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 14, Category = "", Description = "f", Name = "Start a charity", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 15, Category = "", Description = "g", Name = "Accidentally’ tell a bad story about another player during an interview", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 16, Category = "", Description = "h", Name = "Encourage another player’s employees to start a union", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 17, Category = "", Description = "j", Name = "Give a rallying speech", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 18, Category = "", Description = "k", Name = "Host a fun event", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 19, Category = "", Description = "l", Name = "Donate to those in need", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 20, Category = "", Description = "z", Name = "Get photos of you smiling with a baby published in a newspaper", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 21, Category = "", Description = "x", Name = "Get unflattering caricatures of your opponents published", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Global },

                    new Card { Id = 22, Category = "", Description = "c", Name = "Everyone that played a legal card loses 100 points", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 23, Category = "", Description = "v", Name = "Everyone that played an illegal card loses 300 points", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 24, Category = "", Description = "b", Name = "Choose someone that won’t be able to select which card they play next turn", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 25, Category = "", Description = "n", Name = "Everyone loses 150 points", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 26, Category = "", Description = "m", Name = "Random player loses 500 points", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 27, Category = "", Description = ",", Name = "Nobody gets defence cards in round 2", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 53, Category = "", Description = ",", Name = "Target only receives 3 cards in round 2", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Other },

                    new Card { Id = 28, Category = "", Description = ".", Name = "Hire a PI", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 29, Category = "", Description = "/", Name = "Audit Financial records", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 30, Category = "", Description = ";", Name = "Look through their rubbish to find dirt", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 31, Category = "", Description = ";", Name = "Conduct a federal investigation", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 32, Category = "", Description = ";", Name = "Interview employees", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 33, Category = "", Description = ";", Name = "Send Death Threat", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 34, Category = "", Description = ";", Name = "Hire a photographer to follow someone around", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },

                    new Card { Id = 35, Category = "", Description = ";", Name = "Alibi", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 36, Category = "", Description = ";", Name = "Call an emergency press conference", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 37, Category = "", Description = ";", Name = "Political rebuttal", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 38, Category = "", Description = ";", Name = "Feign ignorance", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 39, Category = "", Description = ";", Name = "Body double", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 40, Category = "", Description = ";", Name = "Make a public apology", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 41, Category = "", Description = ";", Name = "Fire your second in command", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 42, Category = "", Description = ";", Name = "Take a trip to the Bahamas", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 43, Category = "", Description = ";", Name = "Call the media “Fake News”", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 44, Category = "", Description = ";", Name = "Make fake promises", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 45, Category = "", Description = ";", Name = "Point fingers", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 46, Category = "", Description = ";", Name = "Find a scapegoat", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },

                    new Card { Id = 47, Category = "", Description = ";", Name = "Baiting card", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 48, Category = "", Description = ";", Name = "Make investigation cards work incorrectly", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 49, Category = "", Description = ";", Name = "Nobody’s defence cards work", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 50, Category = "", Description = ";", Name = "Players that play illegal cards only get half the points they should if they’re successful", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 51, Category = "", Description = ";", Name = "Person with the highest amount of pending points will get the pending point of the person with the lowest and vise-versa", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 52, Category = "", Description = ";", Name = "Swap pending points with another player", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                };
            }
        }

        public static class List_CardTargets
        {
            public const string Self = "Self",
                Other = "Other",
                Global = "Global";
        }
    }
}
