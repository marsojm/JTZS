using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JTZS
{
    /// <summary>
    /// Luokka lataa kaikki pelin käyttämät grafiikat
    /// </summary>
    public class GraphicsLib
    {
        public SpriteFont text;
        public Texture2D backgroundMenu;
        public Texture2D background;
        public Texture2D player;
        public Texture2D zombie;
        public Texture2D bullet;
        public Texture2D health;
        public Texture2D popup;
        public Texture2D crosshair;
        public Texture2D zombiedeath;
        public Texture2D medkit;
        public Texture2D machinegun;
        public Texture2D rifle;

        public GraphicsLib(ContentManager content)
        {
            text = content.Load<SpriteFont>("Arial");
            backgroundMenu = content.Load<Texture2D>("tausta1");
            background = content.Load<Texture2D>("ruoho");
            player = content.Load<Texture2D>("hahmo");
            zombie = content.Load<Texture2D>("zombie");
            bullet = content.Load<Texture2D>("bullet");
            health = content.Load<Texture2D>("health");
            popup = content.Load<Texture2D>("popup");
            crosshair = content.Load<Texture2D>("crosshair");
            zombiedeath = content.Load<Texture2D>("zombiedeath");
            medkit = content.Load<Texture2D>("medkit");
            machinegun = content.Load<Texture2D>("mg");
            rifle = content.Load<Texture2D>("rifle");
        }
    }
}
