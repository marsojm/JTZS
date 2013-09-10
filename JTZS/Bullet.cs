using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JTZS
{
    public class Bullet
    {
        public Vector2 position;
        public Vector2 direction;
        private GraphicsLib graphicsLib;


        public Bullet(Vector2 position, Vector2 direction, GraphicsLib graphicsLib)
        {
            this.graphicsLib = graphicsLib;
            this.position = position;
            this.direction = direction;
        }

        public void Update(GameTime gameTime)
        {
            position += Vector2.Multiply(direction, 11);
        }

        /// <summary>
        /// Tarkistaa onko luoti vielä pelialueella.
        /// </summary>
        /// <returns>true, jos on ja false, jos ei ole</returns>
        public bool InArea()
        {
            if (this.position.X < 0 || this.position.X > 800 || this.position.Y < 0 || this.position.Y > 600)
                return false;
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                graphicsLib.bullet,
                position,
                null,
                Color.White,
                0,
                new Vector2(0, 0),
                1f,
                SpriteEffects.None,
                0);

        }


    }
}
