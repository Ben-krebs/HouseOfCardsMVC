using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace HouseOfCardsMVC.Models
{
    public class Constants
    {
        public static class CacheKeys
        {
            public static string Entry { get { return "_Entry"; } }
            public static string CallbackEntry { get { return "_Callback"; } }
            public static string CallbackMessage { get { return "_CallbackMessage"; } }
            public static string Parent { get { return "_Parent"; } }
            public static string Child { get { return "_Child"; } }
            public static string DependentMessage { get { return "_DependentMessage"; } }
            public static string DependentCTS { get { return "_DependentCTS"; } }
            public static string Ticks { get { return "_Ticks"; } }
            public static string CancelMsg { get { return "_CancelMsg"; } }
            public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
        }


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
                    new Card { Id = 0,  Category = "", Description = "a", Name = "1", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 1,  Category = "", Description = "q", Name = "2", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 2,  Category = "", Description = "w", Name = "3", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 3,  Category = "", Description = "e", Name = "4", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 4,  Category = "", Description = "r", Name = "5", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 5,  Category = "", Description = "t", Name = "6", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 6,  Category = "", Description = "y", Name = "7", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 7,  Category = "", Description = "u", Name = "8", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 8,  Category = "", Description = "i", Name = "9", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 9,  Category = "", Description = "o", Name = "0", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 10, Category = "", Description = "p", Name = "1", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 11, Category = "", Description = "a", Name = "2", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 12, Category = "", Description = "s", Name = "3", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 13, Category = "", Description = "d", Name = "4", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 14, Category = "", Description = "f", Name = "5", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 15, Category = "", Description = "g", Name = "6", Dirty = false, Image = "", Phase = 1, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 16, Category = "", Description = "h", Name = "7", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 17, Category = "", Description = "j", Name = "8", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 18, Category = "", Description = "k", Name = "9", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 19, Category = "", Description = "l", Name = "0", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 20, Category = "", Description = "z", Name = "1", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 21, Category = "", Description = "x", Name = "2", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 22, Category = "", Description = "c", Name = "3", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 23, Category = "", Description = "v", Name = "4", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 24, Category = "", Description = "b", Name = "5", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 25, Category = "", Description = "n", Name = "6", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 26, Category = "", Description = "m", Name = "7", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 27, Category = "", Description = ",", Name = "8", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 28, Category = "", Description = ".", Name = "9", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 29, Category = "", Description = "/", Name = "0", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
                    new Card { Id = 30, Category = "", Description = ";", Name = "1", Dirty = false, Image = "", Phase = 2, Score = 0, Target = List_CardTargets.Self },
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
