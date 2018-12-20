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
                    new Card { Id = -1,  Type = "", Description = "Someone intercepted your card, you have one less option to play this round ", Name = "Blank", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },

                    new Card { Id = 0,  Type = "Scandal", Description = "You call up ya boi Vlad and he’s gonna rig the election for you", Name = "Collude with the Russians",
                        Dirty = true, Image = "", Phase = 1, Score = 400, Attack = 400, Defense = 0, Target = List_CardTargets.Other },
                    new Card { Id = 1,  Type = "Scandal", Description = "q", Name = "Hack into another players emails",
                        Dirty = true, Image = "", Phase = 1, Score = 100, Attack = 500, Defense = 0, Target = List_CardTargets.Other },
                    new Card { Id = 2,  Type = "Scandal", Description = "w", Name = "Evade tax",
                        Dirty = true, Image = "", Phase = 1, Score = 400, Attack = 0, Defense = 100, Target = List_CardTargets.Self },
                    new Card { Id = 3,  Type = "Scandal", Description = "e", Name = "Plant incriminating documents in someone’s office",
                        Dirty = true, Image = "", Phase = 1, Score = 0, Attack = 600, Defense = 0, Target = List_CardTargets.Other },
                    new Card { Id = 4,  Type = "Scandal", Description = "r", Name = "Accept undisclosed donations",
                        Dirty = true, Image = "", Phase = 1, Score = 450, Attack = 0, Defense = 0, Target = List_CardTargets.Self },
                    new Card { Id = 5,  Type = "Scandal", Description = "t", Name = "Publically lie",
                        Dirty = true, Image = "", Phase = 1, Score = 300, Attack = 0, Defense = 100, Target = List_CardTargets.Self },
                    new Card { Id = 6,  Type = "Scandal", Description = "y", Name = "Organise assassination ",
                        Dirty = true, Image = "", Phase = 1, Score = 0, Attack = 600, Defense = 0, Target = List_CardTargets.Other },
                    new Card { Id = 7,  Type = "Scandal", Description = "Threaten to nuke another country", Name = "Flaunt your big red button",
                        Dirty = true, Image = "", Phase = 1, Score = 200, Attack = 100, Defense = 200, Target = List_CardTargets.Global },
                    new Card { Id = 8,  Type = "Scandal", Description = "i", Name = "Waterboard someone for information",
                        Dirty = true, Image = "", Phase = 1, Score = 40, Attack = 0, Defense = 0, Target = List_CardTargets.Self },
                    new Card { Id = 9,  Type = "Scandal", Description = "o", Name = "Hire an impersonator to do something bad",
                        Dirty = true, Image = "", Phase = 1, Score = 0, Attack = 100, Defense = 200, Target = List_CardTargets.Global },
                    new Card { Id = 10, Type = "Scandal", Description = "p", Name = "Cut off your opponents finger and feed it to your dog to get rid of the evidence",
                        Dirty = true, Image = "", Phase = 1, Score = 0, Attack = 300, Defense = 200, Target = List_CardTargets.Other },

                    new Card { Id = 11, Type = "Campaign", Description = "a", Name = "Appear on a talk show",
                        Dirty = false, Image = "", Phase = 1, Score = 150, Attack = 50, Defense = 50, Target = List_CardTargets.Self },
                    new Card { Id = 12, Type = "Campaign", Description = "s", Name = "Win a debate against another player",
                        Dirty = false, Image = "", Phase = 1, Score = 200, Attack = 200, Defense = 0,Target = List_CardTargets.Other },
                    new Card { Id = 13, Type = "Campaign", Description = "d", Name = "Post an unsavoury video of another player",
                        Dirty = false, Image = "", Phase = 1, Score = 0, Attack = 300, Defense = 0,Target = List_CardTargets.Other },
                    new Card { Id = 14, Type = "Campaign", Description = "f", Name = "Start a charity",
                        Dirty = false, Image = "", Phase = 1, Score = 200, Attack = 0, Defense = 200,Target = List_CardTargets.Self },
                    new Card { Id = 15, Type = "Campaign", Description = "g", Name = "Accidentally’ tell a bad story about another player during an interview",
                        Dirty = false, Image = "", Phase = 1, Score = 50, Attack = 200, Defense = 0,Target = List_CardTargets.Other },
                    new Card { Id = 16, Type = "Campaign", Description = "h", Name = "Encourage another player’s employees to start a union",
                        Dirty = false, Image = "", Phase = 1, Score = 0, Attack = 250, Defense = 0,Target = List_CardTargets.Self },
                    new Card { Id = 17, Type = "Campaign", Description = "j", Name = "Give a rallying speech",
                        Dirty = false, Image = "", Phase = 1, Score = 150, Attack = 0, Defense = 50,Target = List_CardTargets.Self },
                    new Card { Id = 18, Type = "Campaign", Description = "k", Name = "Host a fun event",
                        Dirty = false, Image = "", Phase = 1, Score = 200, Attack = 0, Defense = 0,Target = List_CardTargets.Self },
                    new Card { Id = 19, Type = "Campaign", Description = "l", Name = "Donate to those in need",
                        Dirty = false, Image = "", Phase = 1, Score = 200, Attack = 0, Defense = 50,Target = List_CardTargets.Self },
                    new Card { Id = 20, Type = "Campaign", Description = "z", Name = "Get photos of you smiling with a baby published in a newspaper",
                        Dirty = false, Image = "", Phase = 1, Score = 300, Attack = 0, Defense = 0,Target = List_CardTargets.Self },
                    new Card { Id = 21, Type = "Campaign", Description = "x", Name = "Get unflattering caricatures of your opponents published",
                        Dirty = false, Image = "", Phase = 1, Score = 0, Attack = 100, Defense = 0,Target = List_CardTargets.Global },
                    new Card { Id = 55, Type = "Campaign", Description = "Your work hard, you deserve a break, it will only be 5 minutes (This card does nothing)", Name = "Take a power nap",
                        Dirty = false, Image = "", Phase = 1, Score = 0, Attack = 0, Defense = 0,Target = List_CardTargets.Self },

                    new Card { Id = 22, Type = "Twist", Description = "Everyone that played a legal card loses 100 points", Name = "Rat Race", Dirty = false, Image = "", Phase = 1, Effect_Id = 6, Target = List_CardTargets.Global },
                    new Card { Id = 23, Type = "Twist", Description = "Everyone that played an illegal card loses 300 points", Name = "Public opinion poll", Dirty = false, Image = "", Phase = 1, Effect_Id = 7, Target = List_CardTargets.Global },
                    new Card { Id = 24, Type = "Twist", Description = "Choose someone that won’t be able to select which card they play next turn", Name = "New Intern", Dirty = false, Image = "", Phase = 1, Effect_Id = 8, Target = List_CardTargets.Other },
                    new Card { Id = 25, Type = "Twist", Description = "Everyone loses 150 points", Name = "Bad day at the office", Dirty = false, Image = "", Phase = 1, Attack = 150, Effect_Id = 9, Target = List_CardTargets.Global },
                    new Card { Id = 26, Type = "Twist", Description = "Random player loses 500 points", Name = "An act of God", Dirty = false, Image = "", Phase = 1, Effect_Id = 10, Target = List_CardTargets.Self },
                    new Card { Id = 27, Type = "Twist", Description = "Nobody gets defence cards in round 2", Name = "Parlimentary Audit", Dirty = false, Image = "", Phase = 1, Effect_Id = 11, Target = List_CardTargets.Global },
                    new Card { Id = 53, Type = "Twist", Description = "Target player receives one less card during Phase 1 of the next round. \r\n\r\n Vladmir appreciates your co-operation", Name = "Intercept delivery", Dirty = false, Image = "", Phase = 1, Effect_Id = 12, Target = List_CardTargets.Other },

                    new Card { Id = 28, Type = "Investigation", SubType = "Dirt", Description = "Reveals if the player has done an illegal action this round", Name = "Hire a PI",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 29, Type = "Investigation", SubType = "Score", Description = "Reveals The expected increase or decrease in popularity this player is expecting", Name = "Audit Financial records",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 30, Type = "Investigation", SubType = "Dirt", Description = "Reveals if the player has done an illegal action this round", Name = "Look through their rubbish to find dirt",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 31, Type = "Investigation", SubType = "History", Description = "Reveals how many illegal moves the player has already made this game", Name = "Conduct a federal investigation",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 32, Type = "Investigation", SubType = "Dirt", Description = "Reveals if the player has done an illegal action this round", Name = "Interview employees",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 33, Type = "Investigation", SubType = "Target", Description = "Reveals who the player has targeted this round", Name = "Send Death Threat",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 34, Type = "Investigation", SubType = "Target", Description = "Reveals who the player has targeted this round", Name = "Hire a photographer to follow someone around",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Other },
                    new Card { Id = 56, Type = "Investigation", Description = "It started out by clicking on an innocent link on Facebook... that was 2 hours ago (This card does nothing)", Name = "Trawl through the internet",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Attack = 0, Defense = 0,Target = List_CardTargets.Self },

                    new Card { Id = 35, Type = "Defense", SubType = "Dirt", Description = "Conceals any illegal move you have made this round", Name = "Alibi",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 36, Type = "Defense", SubType = "Dirt", Description = "Conceals any illegal move you have made this round", Name = "Call an emergency press conference",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 37, Type = "Defense", SubType = "Target", Description = "Conceals the identity of any victim of a scandal you made", Name = "Political rebuttal",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 38, Type = "Defense", SubType = "History", Description = "Protects knowledge about historic illegal moves you have made", Name = "Feign ignorance",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 40, Type = "Defense", SubType = "Dirt", Description = "Conceals any illegal move you have made this round", Name = "Make a public apology",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 41, Type = "Defense", SubType = "Target", Description = "Conceals the identity of any victim of a scandal you made", Name = "Fire your second in command",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 42, Type = "Defense", SubType = "Dirt", Description = "Conceals any illegal move you have made this round", Name = "Take a trip to the Bahamas",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 43, Type = "Defense", SubType = "Score", Description = "Protects information about your expected score increase this round", Name = "Call the media “Fake News”",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 44, Type = "Defense", SubType = "Dirt", Description = "Conceals any illegal move you have made this round", Name = "Make fake promises",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 45, Type = "Defense", SubType = "Dirt", Description = "Conceals any illegal move you have made this round", Name = "Point fingers",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 46, Type = "Defense", SubType = "Target", Description = "Conceals the identity of any victim of a scandal you made", Name = "Find a scapegoat",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 53, Type = "Defense", SubType = "Score", Description = "No one understands ho to use your accounting package, getting tax records out of this will be impossible", Name = "Outdated accounting software",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Global },
                    new Card { Id = 54, Type = "Defense", SubType = "Dirt", Description = "You have hired a body dboule to act on your behalf in public while you lurk from the shadows", Name = "Body Double",
                        Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Global },

                    new Card { Id = 47, Type = "Twist", Description = "Try to get accused, if you performed an illegal action you will receive 300 points, if you performed a legal action you will gaim 400 points", Name = "Baiting card", Dirty = false, Image = "", Phase = 2, Effect_Id = 13, Target = List_CardTargets.Self },
                    new Card { Id = 48, Type = "Twist", Description = ";", Name = "Make investigation cards work incorrectly", Dirty = false, Image = "", Phase = 2, Effect_Id = 1, Target = List_CardTargets.Global },
                    new Card { Id = 49, Type = "Twist", Description = ";", Name = "Nobody’s defence cards work", Dirty = false, Image = "", Phase = 2, Effect_Id = 2, Target = List_CardTargets.Global },
                    new Card { Id = 50, Type = "Twist", Description = ";", Name = "Players that play illegal cards only get half the points they should if they’re successful", Dirty = false, Image = "", Phase = 2, Effect_Id = 3, Target = List_CardTargets.Global },
                    new Card { Id = 51, Type = "Twist", Description = ";", Name = "Person with the highest amount of pending points will get the pending point of the person with the lowest and vise-versa", Dirty = false, Image = "", Phase = 2, Effect_Id = 4, Target = List_CardTargets.Self },
                    new Card { Id = 52, Type = "Twist", Description = ";", Name = "Swap pending points with another player", Dirty = false, Image = "", Phase = 2, Effect_Id = 5, Target = List_CardTargets.Other },
                };
            }
        }

        public static class List_CardTargets
        {
            public const string Self = "Self",
                Other = "Other",
                Global = "Global";
        }

        public class Card_Effect
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }


        public static List<Card_Effect> CardEffects
        {
            get
            {
                return new List<Card_Effect> {
                    new Card_Effect {Id = 0, Description = "" },
                    new Card_Effect {Id = 1, Description = "" },
                    new Card_Effect {Id = 2, Description = "" },
                    new Card_Effect {Id = 3, Description = "" },
                    new Card_Effect {Id = 4, Description = "" },
                    new Card_Effect {Id = 5, Description = "" },
                    new Card_Effect {Id = 6, Description = "" },
                    new Card_Effect {Id = 7, Description = "" },
                    new Card_Effect {Id = 8, Description = "" },
                    new Card_Effect {Id = 9, Description = "" },
                    new Card_Effect {Id = 10, Description = "" },
                    new Card_Effect {Id = 11, Description = "" },
                    new Card_Effect {Id = 12, Description = "" }
                };
            }
        }
    }
}
