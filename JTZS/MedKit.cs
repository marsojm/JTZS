using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JTZS
{
    public class MedKit : Item
    {
        
        private GraphicsLib graphicsLib;
        private Vector2 position;
        private BoundingSphere bSphere;
        private String itemName;
        
        public MedKit(Vector2 position, GraphicsLib graphicsLib)
        {
            this.position = position;
            this.graphicsLib = graphicsLib;
            itemName = "MedKit";
            
        }

        public Vector2 Position()
        {
            return this.position;
        }

        public BoundingSphere Sphere()
        {
            return bSphere;
        }

        public string ItemName()
        {
            return itemName;
        }

        public void Update(GameTime gameTime)
        {
            bSphere = new BoundingSphere(
                new Vector3(position.X, position.Y, 0), 10
                );
        }

        public void Collision(Player player)
        {
            player.Health += 20;
            
            if (player.Health > 100) player.Health = 100;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    graphicsLib.medkit,
                    position,
                    null,
                    Color.White,
                    0,
                    new Vector2(graphicsLib.medkit.Width / 2, graphicsLib.medkit.Height / 2),
                    1f,
                    SpriteEffects.None,
                    0);
        }
    }
}
