using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace JTZS
{
    public class Popup
    {
        public string[,] KeyConvert = 
    { 
        {"D1", "1"}, {"D2", "2"}, 
        {"D3", "3"}, {"D4", "4"}, 
        {"D5", "5"}, {"D6", "6"},
        {"D7", "7"}, {"D8", "8"}, 
        {"D9", "9"}, {"D0", "0"}, 
        {"NumPad1", "1"}, {"NumPad2", "2"}, 
        {"NumPad3", "3"}, {"NumPad4", "4"}, 
        {"NumPad5", "5"}, {"NumPad6", "6"}, 
        {"NumPad7", "7"}, {"NumPad8", "8"}, 
        {"NumPad9", "9"}, {"NumPad0", "0"}, 
        {"Space", " "}, 
        {"A", "a"}, {"B", "b"}, 
        {"C", "c"}, {"D", "d"}, 
        {"E", "e"}, {"F", "f"},
        {"G", "g"}, {"H", "h"}, 
        {"I", "i"}, {"J", "j"}, 
        {"K", "k"}, {"L", "l"}, 
        {"M", "m"}, {"N", "n"}, 
        {"O", "o"}, {"P", "p"}, 
        {"Q", "q"}, {"R", "r"}, 
        {"S", "s"}, {"T", "t"}, 
        {"U", "u"}, {"V", "v"}, 
        {"W", "w"}, {"X", "x"},
        {"Y", "y"}, {"Z", "z"} 
    };

        Keys[] keymap;
        string name = "player1";
        int maxLength = 10;
        KeyboardState ks, last_ks;
        SpriteBatch spriteBatch;
        SpriteFont Arial;
        Texture2D popuptex;
        bool nameGiven = false;

        public Popup(SpriteBatch sb, SpriteFont arial, Texture2D ptex)
        {
            spriteBatch = sb;
            Arial = arial;
            popuptex = ptex;
        }

        public string Name
        {
            get { return name; }
            set { this.name = value;  }
        }

        public void Update()
        {
            ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Back) &&
                last_ks.IsKeyUp(Keys.Back) &&
                name.Length > 0)
            {
                name = name.Substring(0, name.Length - 1);
            }

            if (ks.IsKeyDown(Keys.Enter) &&
                last_ks.IsKeyUp(Keys.Enter))
            {
                this.name = Name;
                nameGiven = true;                

            }

            if (name.Length < maxLength)
            {
                keymap = (Keys[])ks.GetPressedKeys();
                foreach (Keys k in keymap)
                {
                    // 47 keys stored in KeyConvert[,]
                    for (int I = 0; I < 47; I++)
                    {
                        if (k.ToString() == KeyConvert[I, 0] &&
                            last_ks.IsKeyUp(k))
                        {
                            name += KeyConvert[I, 1];
                            break;
                        }
                    }
                }
            }

            last_ks = ks;


        }
        public bool IsDone()
        {
            return nameGiven;
        }

        public void Draw()
        {

            spriteBatch.Draw(popuptex, new Vector2(250, 260), Color.Red);
            spriteBatch.DrawString(Arial, "Your Name: " + name + "<", new Vector2(300, 300), Color.White);

        }
    }
}
