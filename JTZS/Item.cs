using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JTZS
{
    public interface Item
    {
        
        Vector2 Position();
        BoundingSphere Sphere();

        void Collision(Player player);

        void Update(GameTime gameTime);
        
        void Draw(SpriteBatch spriteBatch);

        String ItemName();
        
     
     
    }
}
