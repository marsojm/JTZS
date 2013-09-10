using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace JTZS
{
    public class Menu
    {
        private string name = "";
        private string text = "";
        SpriteFont font;
        private Vector2 position;
        private Color color;       //perusväri
        private Color selectedColor; // väri, kun on aktivoituna

        private bool selected = false;

        public bool Selected
        {
            get { return this.selected; }
            set { this.selected = value; }

        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// konstruktori
        /// </summary>
        /// <param name="name">valikon nimi</param>
        /// <param name="text">valikon teksti</param>
        /// <param name="font">käytetty fontti</param>
        /// <param name="position">valikon sijainti</param>
        /// <param name="color">väri</param>
        /// <param name="selectedColor">väri kun on aktoivoituna</param>
        /// <param name="selected">onko aktivoitu vai ei</param>
        public Menu(string name, string text, SpriteFont font, Vector2 position, Color color, Color selectedColor, bool selected)
        {
            this.name = name;
            this.text = text;
            this.font = font;
            this.position = position;
            this.color = color;
            this.selectedColor = selectedColor;
            this.selected = selected;

        }


        public void Draw(SpriteBatch sb)
        {
            if (selected)
            {
                sb.DrawString(font, text, position, selectedColor);
            }
            else
            {
                sb.DrawString(font, text, position, color);
            }
        }



    }
}
