using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NGChat.Infrastructure.Utils
{
    public class EmoticonParser
    {
        private Dictionary<string, string> _emoticons = new Dictionary<string, string>();
        
        public EmoticonParser()
        {
            RegisterEmoticons();
        }

        public string Parse(string text)
        {
            StringBuilder transformed = new StringBuilder(text);

            foreach (var emoticon in _emoticons)
            {
                var variants = emoticon.Key.Split(' ');

                for (int i = 0; i < variants.Length; i++)
                {
                    string upper = variants[i].ToString().ToUpper();
                    string lower = variants[i].ToString().ToLower();
                    string emoteElement = String.Format("<span class=\"emote {0}\"></span>", emoticon.Value);
                    transformed.Replace(upper, emoteElement);
                    transformed.Replace(lower, emoteElement);
                }
            }

            return transformed.ToString();
        }

        private void RegisterEmoticons()
        {
            _emoticons.Add(";dupa ;ass", "ass");
            _emoticons.Add("d(^^)b d(^-^)b d(^_^)b d(-_-)b d(^.^)b d(^,^)b d-_-b ;muza", "music");
            _emoticons.Add(":D ;D :-D ;-D", "grin");

            _emoticons.Add(";ble ;blee ;bleee ;do_bani", "ble");
            _emoticons.Add(";bije ;boks", "boxing");
            _emoticons.Add(";bezradny", "helpless");
            _emoticons.Add("B) B-) 8-) ;cool", "cool");
            _emoticons.Add("B-D 8-D ;cwaniak ;cwaniak2", "cool2");
            _emoticons.Add("B] B-] 8-]", "cool3");

            _emoticons.Add(";elo", "elo");
            _emoticons.Add(":E ;E ;szczerbol", "jagged");

            _emoticons.Add(";haha ;hahaha", "haha");
            _emoticons.Add(";hej ;hejka ;hey ;heej", "hey");

            _emoticons.Add(";joke", "joke");
            _emoticons.Add(";jupi ;hura ;hurra ;radocha", "jupi");

            _emoticons.Add(";kawa", "coffee");

            _emoticons.Add(";lol ;lol2", "lol");
            _emoticons.Add(";luzik", "chill-out");

            _emoticons.Add(";niedowiarek ;puknij_sie ;puknij_się", "disbeliever");
            _emoticons.Add(";nono ;nonono", "nono");

            _emoticons.Add(";oklaski ;brawo", "well-done");
            _emoticons.Add("o.o o,o o_o 0_0", "huh");
            _emoticons.Add(":O ;O =O :-O ;-O ;szok ;zdziwiony", "shocked");

            _emoticons.Add(";paa ;zlezka ;złezka ;zlezką ;złezką", "bye");
            _emoticons.Add(";papa ;papapa ;papa2", "bye2");
            _emoticons.Add(";prosi", "beg");
            _emoticons.Add(";pierdol_sie", "fuck-off2");
            _emoticons.Add(";pocieszacz ;głaszcze ;glaszcze", "comforting");
            _emoticons.Add(";pomysl ;pomysł ;mam_pomysl ;idea", "idea");
            _emoticons.Add(":P :-P =P :PP :-PP =PP :PPP :-PPP =PPP", "tongue");
            _emoticons.Add(";P ;-P ;PP ;-PP xPP ;PPP ;-PPP xPPP", "tongue2");

            _emoticons.Add(";rotfl ;rotfl2 ;rotfl3", "rotfl");

            _emoticons.Add(";spoko ;spox", "cool4");
            _emoticons.Add(";szpan", "crafty");
            _emoticons.Add(";spadaj ;spadówa", "fuck-off");
            _emoticons.Add(":s ;s :-s ;-s ;uoee", "none2");

            _emoticons.Add(";tak ;yes ;tiaa", "yes");

            _emoticons.Add(";xd", "craze");

            _emoticons.Add(";w00t", "w00t");
            _emoticons.Add(";w8 ;wait", "wait");
            _emoticons.Add(";wow", "wow");
            
            _emoticons.Add(":/ ;/ :-/ ;-/ ;kwasny ;kwaśny", "get-lost");
            _emoticons.Add(":\\ ;\\ :-\\ ;-\\", "get-lost2");
            _emoticons.Add(";3m_sie ;3m_się", "hold-on");
            _emoticons.Add("-.- -,- -_- =,= =.= =_=", "huh2");
            _emoticons.Add(":< ;< :{ ;{ ;foch", "injured");
            _emoticons.Add(":| ;|", "none");
            _emoticons.Add(":-| ;-| =|", "none3");
            _emoticons.Add(":(( =(( :((( =((( :(((( =(((( ;smutny", "sad2");
            _emoticons.Add(":( =( :-(", "sad");
            _emoticons.Add(";( ;-( :'( :''( ); )-: )': )\": ;cry ;placze ;płacze", "cry");
            _emoticons.Add(":) =) :-) (: (= (-: :)) :))) :)))) ;wesoly ;wesoły", "smile");
            _emoticons.Add(":] =] :-]", "smile2");
            _emoticons.Add(";] ;-]", "smile3");
            _emoticons.Add(":' :-' ;' ;-'", "whistle");
            _emoticons.Add(";-) ;) (-; (; ;mruga", "blink");
            _emoticons.Add(">:][ >:[] >;][ >;[]", "angry");
            _emoticons.Add(">:/ >:\\ >;/ >;\\", "angry2");
            _emoticons.Add(":[ >:( >:< >:(( >:[ ;zdenerwowany ;zly> ;zły ;wrrrr!", "angry3");
            
            
        }
    }
}