using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;


namespace JTZS
{
    public class MenuControl
    {
        public Menu[] menu = new Menu[4];
        int selectedMenu = 0;

        KeyboardState previous;
        SpriteBatch spriteBatch;
        SpriteFont font;
        SoundEffect menuSound;
        private GraphicsLib graphicsLib;

        private bool exit = false;
        private bool play = false;
        private bool highScores = false;
        private bool help = false;

        public MenuControl(SpriteBatch spriteBatch, SoundEffect sEffect, GraphicsLib graphicsLib)
        {
            this.graphicsLib = graphicsLib;
            Color baseColor = Color.Brown;
            Color selectedColor = Color.Red;
            this.spriteBatch = spriteBatch;
            this.font = graphicsLib.text;
            menuSound = sEffect;

            menu[0] = new Menu("Play", "Play", font, new Vector2(50f, 100f), baseColor, selectedColor, true);
            menu[1] = new Menu("HighScores", "High Scores", font, new Vector2(50f, 150f), baseColor, selectedColor, false);
            menu[2] = new Menu("Help", "Help", font, new Vector2(50f, 200f), baseColor, selectedColor, false);
            menu[3] = new Menu("Quit", "Quit", font, new Vector2(50f, 250f), baseColor, selectedColor, false);

        }

        public void Update(GameTime time)
        {
            for (int i = 0; i < menu.Length; i++) menu[i].Selected = false;

            KeyboardState kbState = Keyboard.GetState();

            // jos ollaan 0:ssa valikkopalikassa, ja painetaan ylös, niin siirrytään alimpaan
            if ((kbState.IsKeyDown(Keys.Up)) && (previous.IsKeyUp(Keys.Up)))
            {
                menuSound.Play();
                selectedMenu -= 1;
                if (selectedMenu == -1)
                {
                    selectedMenu = menu.Length - 1;
                }
            }

            // jos ollaan viimeisessä valikkopalikassa ja painetaan alas, niin hypätään ylimpään
            if ((kbState.IsKeyDown(Keys.Down)) && (previous.IsKeyUp(Keys.Down)))
            {
                menuSound.Play();
                selectedMenu += 1;
                if (selectedMenu == menu.Length)
                {
                    selectedMenu = 0;
                }
            }

            
            if ((kbState.IsKeyDown(Keys.Enter)) && (previous.IsKeyUp(Keys.Enter)))
            {
                if (menu[selectedMenu].Name == "Quit")
                {
                    Play = false;
                    HighScores = false;
                    Exit = true;
                    Help = false;
                }

                if ((menu[selectedMenu].Name == "HighScores"))
                {
                    Play = false;
                    Exit = false;
                    HighScores = true;
                    Help = false;
                    
                }

                if ((menu[selectedMenu].Name == "Play"))
                {
                    HighScores = false;
                    Exit = false;
                    Play = true;
                    Help = false;
                }

                if ((menu[selectedMenu].Name == "Help"))
                {
                    HighScores = false;
                    Exit = false;
                    Play = false;
                    Help = true;
                }
            }

            menu[selectedMenu].Selected = true;

            previous = kbState;


        }

        public bool Exit
        {
            get { return this.exit; }
            set { this.exit = value; }
        }

        public bool Play
        {
            get { return this.play; }
            set { this.play = value; }
        }

        public bool HighScores
        {
            get { return this.highScores; }
            set { this.highScores = value; }
        }

        public bool Help
        {
            get { return this.help; }
            set { this.help = value; }
        }


        public void Draw(GameTime time)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                menu[i].Draw(spriteBatch);
            }

            spriteBatch.DrawString(font,"Johnson - The Zombie Slayer", new Vector2(300,20),Color.Red);
        }
    }
}
