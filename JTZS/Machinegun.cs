using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JTZS
{
    public class Machinegun: Item
    {
        private GraphicsLib graphicsLib;
        private Vector2 position;
        private BoundingSphere bSphere;
        private String itemName;

        public Machinegun(Vector2 position, GraphicsLib graphicsLib)
        {
            this.position = position;
            this.graphicsLib = graphicsLib;
            itemName = "Machinegun";
        }

        public Vector2 Position()
        {
            return position;
        }
        public BoundingSphere Sphere()
        {
            return bSphere;
        }

        public string ItemName()
        {
            return itemName;
        }

        public void Collision(Player player)
        {
            if (player.CurrenWeapon == itemName)
            {
                player.AddAmmo(100);

            }
            else
            {
                player.ShotInterval = 80f;
                player.CurrenWeapon = itemName;
                player.Damage = 1;
                player.Ammo = 100;
            }
        }

        public void Update(GameTime gameTime)
        {
            bSphere = new BoundingSphere(
                new Vector3(position.X, position.Y, 0), 10
                );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    graphicsLib.machinegun,
                    position,
                    null,
                    Color.White,
                    0,
                    new Vector2(graphicsLib.machinegun.Width / 2, graphicsLib.machinegun.Height / 2),
                    1f,
                    SpriteEffects.None,
                    0);
        }

    }
}
