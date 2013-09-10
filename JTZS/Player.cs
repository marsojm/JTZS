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
    public class Player
    {
        private Vector2 position;
        private Vector2 direction; // minne suuntaan katsoo
        private Vector2 crossPosition;
        private GraphicsLib graphicsLib;
        private String currentWeapon;
        private String defaultWeapon;
        private BoundingSphere bSphere;
        private float rotation;
        private int health;
        private int ammo;
        private int damage;
        private float shotInterval;


        KeyboardState currentKeyboardstate;

        public Player(Vector2 position, GraphicsLib graphicsLib, int health)
        {
            this.position = position;
            bSphere = new BoundingSphere(new Vector3(position.X, position.Y,0), 12);
            this.graphicsLib = graphicsLib;
            this.health = health;
            defaultWeapon = "Assault Rifle";
            currentWeapon = defaultWeapon;
            shotInterval = 170f;
            damage = 1;
        }

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Vector2 Direction
        {
            get { return direction; }
            set { this.direction = value; }
        }

        public BoundingSphere Sphere()
        {
            return bSphere;
        }

        public String CurrenWeapon
        {
            get { return currentWeapon; }
            set { currentWeapon = value; }
        }

        public float ShotInterval
        {
            get { return shotInterval; }
            set { shotInterval = value; }
        }

        public void AddAmmo(int value)
        {
            ammo += value;
        }

        public int Ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }

        public int Damage
        {
            get { return damage;  }
            set { damage = value; }
        }

        public int Health
        {
            get { return health; }
            set { this.health = value; }
        }

        public bool DefaultWeapon()
        {
            if (currentWeapon == defaultWeapon) return true;
            else return false;
        }

        public void Update(GameTime gameTime)
        {
            Move();
            bSphere = new BoundingSphere(new Vector3(position.X, position.Y, 0), 12);

            if (ammo < 1)
            {
                shotInterval = 170f;
                currentWeapon = defaultWeapon;
                ammo = 0;
                damage = 1;
            }
        }

        public void PlayerInArea()
        {
            if (position.X < 0) position.X = 0;
            if (position.X > 800) position.X = 800;
            if (position.Y < 0) position.Y = 0;
            if (position.Y > 600) position.Y = 600;
        }

        public void Move()
        {
            currentKeyboardstate = Keyboard.GetState();

            direction.X = (float)Math.Cos(rotation);
            direction.Y = (float)Math.Sin(rotation);
            crossPosition = position;

            crossPosition.X += 150*direction.X;
            crossPosition.Y += 150*direction.Y;
          
            if (rotation >= MathHelper.TwoPi) rotation = 0;

            if (rotation < 0) rotation = MathHelper.TwoPi;

            if (currentKeyboardstate.IsKeyDown(Keys.Left))
            {
                rotation -= 0.08f;
            }

            if (currentKeyboardstate.IsKeyDown(Keys.Right))
            {
                rotation += 0.08f;
            }

            if (currentKeyboardstate.IsKeyDown(Keys.Up))
            {
                position += Vector2.Multiply(direction, 3);
            }

            if (currentKeyboardstate.IsKeyDown(Keys.Down))
            {
                position -= Vector2.Multiply(direction, 3);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                graphicsLib.player,
                position,
                null,
                Color.White,
                rotation,
                new Vector2(graphicsLib.player.Width / 2, graphicsLib.player.Height / 2),
                1f,
                SpriteEffects.None,
                0);
            
            spriteBatch.Draw(
                graphicsLib.crosshair,
                crossPosition,
                null,
                Color.White,
                rotation,
                new Vector2(graphicsLib.crosshair.Width / 2, graphicsLib.crosshair.Height / 2),
                1f,
                SpriteEffects.None,
                0);
            
        }

    }

    
}
