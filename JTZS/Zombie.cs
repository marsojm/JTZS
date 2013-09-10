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
    public class Zombie
    {
        public Vector2 position;
        public Vector2 direction;
        public BoundingSphere bSphere;
        private float rotation;
        private float speed;
        private int health;
        private int damage;
        private GraphicsLib graphicsLib;

        private float animTimer = 0f;
        private float animInterval = 7000f / 1000f;
        public int currentFrame = 0;
        private int frameCount = 4;
        private float permanentRotation;
        Rectangle sourceRect;
        Rectangle destinationRect;

        public Zombie(Vector2 position, GraphicsLib graphicsLib)
        {
            this.position = position;
            speed = 0.9f;
            health = 3;
            damage = 5;
            destinationRect = new Rectangle((int)position.X, (int)position.Y, 30, 30);
            this.graphicsLib = graphicsLib;

        }

        public float Speed
        {
            get { return speed; }
            set { this.speed = value; }
        }

        public int Health
        {
            get { return health; }
            set { this.health = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { this.damage = value; }
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            destinationRect = new Rectangle((int)position.X, (int)position.Y, 30, 30);
            Move(playerPosition);
            bSphere = new BoundingSphere(
                new Vector3(position.X, position.Y , 0), 15
                );

            if (health <= 0)
            {
                permanentRotation = rotation;
                animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (animTimer > animInterval)
                {
                    currentFrame++;

                    if (currentFrame > frameCount - 1)
                    {
                        currentFrame = 4;
                    }
                    animTimer = 0f;
                }
                sourceRect = new Rectangle(currentFrame * 30, 0, 30, 30);
            }
            
        }

        public void Move(Vector2 playerPosition)
        {
            
            rotation = (float)Math.Atan2(playerPosition.Y - position.Y, playerPosition.X - position.X);     
            direction.X = (float)Math.Cos(rotation);
            direction.Y = (float)Math.Sin(rotation);
            position += Vector2.Multiply(direction, speed);
           
        }

        public void Collision()
        {
            position -= Vector2.Multiply(direction, 10*speed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {


            if (health > 0)
            {
                spriteBatch.Draw(
                graphicsLib.zombie,
                position,
                null,
                Color.White,
                rotation,
                new Vector2(graphicsLib.zombie.Width / 2, graphicsLib.zombie.Height / 2),
                1f,
                SpriteEffects.None,
                0);

            }
            else
            {   
                
                spriteBatch.Draw(graphicsLib.zombiedeath,
                    destinationRect,
                    sourceRect,
                    Color.White,
                    permanentRotation,
                    new Vector2(15, 15),
                    SpriteEffects.None,
                    0);
            }
        }

    }
}
