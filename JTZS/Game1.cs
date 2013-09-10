using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace JTZS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private static GraphicsLib graphicsLib;
        private static SoundsLib soundsLib;
        private static MenuControl menuControl;

        KeyboardState currentKeyboardState;

        Player player;
        BoundingSphere bSphere;
        private float healthTimer;
        private float healthInterval;
        private float zombieTimer;
        private float zombieInterval;
        private static Random random;
        private int kills;
        private int zombieHorde; // montako zombieta ilmestyy p‰ivityksess‰
        private float hordeInterval;
        private float hordeTimer;
        private List<Bullet> bullets = new List<Bullet>();
        private List<Zombie> zombies = new List<Zombie>();
        private List<Zombie> killedZombies = new List<Zombie>();
        private List<Bullet> destroyedBullets = new List<Bullet>();
        private List<Item> items = new List<Item>();
        private List<Item> destroyeditems = new List<Item>();
        private float shot;
        private float soundTimer;
        private float soundInterval;
        private float itemTimer;
        private float itemInterval;


        private bool scoresSaved = false;
        private bool highScoreEntered = false;
        HighScores highScores;
        Popup popup;



        enum GameState { gameMenu, gamePlaying, gameHighScores, gamePopup, gameHelp }

        GameState state;

        #region Game state management

        void startGame()
        {
            state = GameState.gamePlaying;
            gameInit();
        }

        void gameOver()
        {

            state = GameState.gameHighScores;
        }

        void toMenu()
        {
            Initialize();
            highScoreEntered = false;
            state = GameState.gameMenu;
        }
        void gamePopup()
        {
            state = GameState.gamePopup;
        }

        void toHelp()
        {
            state = GameState.gameHelp;
        }

        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void gameInit()
        {
            zombieTimer = 0;
            zombieInterval = 3500;
            random = new Random();
            shot = 0;
            kills = 0;
            zombieHorde = 0;
            hordeInterval = 17000;
            hordeTimer = 0;
            soundTimer = 0;
            itemTimer = 0;
            itemInterval = 7000;
            healthTimer = 0;
            healthInterval = 1000f;
            // asetetaan pelaaja oikealle paikalle ja "nollataan" taulukot
            player = new Player(new Vector2(400, 300), graphicsLib, 100);
            zombies = new List<Zombie>();
            killedZombies = new List<Zombie>();
            bullets = new List<Bullet>();
            destroyedBullets = new List<Bullet>();
            items = new List<Item>();
            destroyeditems = new List<Item>();
        }

        

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            state = GameState.gameMenu;
            gameInit();
            base.Initialize();
            soundInterval = soundsLib.scream.Duration.Milliseconds;
            player = new Player(new Vector2(400, 300), graphicsLib, 100);

            
            
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsLib = new GraphicsLib(Content); // ladataan grafiikkakirjasto
            soundsLib = new SoundsLib(Content);
            menuControl = new MenuControl(spriteBatch, soundsLib.menuSound, graphicsLib);
            popup = new Popup(spriteBatch, graphicsLib.text, graphicsLib.popup);
            highScores = HighScores.Load("Content/hiscores.xml");
            highScores.Sort();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #region UpdateZombieTimer, UpdateCollision, UpdateItem, PlayerInArea, Alive, Attack

        /// <summary>
        /// Luodaan zombeja pelialueen laidalle ajoittain
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateZombieTimer(GameTime gameTime)
        {
            zombieTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (zombieTimer >= zombieInterval)
            {
                int XY = random.Next(4);
                if (XY == 0)
                {
                    zombies.Add(new Zombie(new Vector2(0, random.Next(600)), graphicsLib));
                }
                if (XY == 1)
                {
                    zombies.Add(new Zombie(new Vector2(800, random.Next(600)), graphicsLib));
                }
                if (XY == 2)
                {
                    zombies.Add(new Zombie(new Vector2(random.Next(800), 0), graphicsLib));
                }
                if (XY == 3)
                {
                    zombies.Add(new Zombie(new Vector2(random.Next(800), 600), graphicsLib));
                }
                if (zombieInterval > 100)
                    zombieInterval -= 10;
                zombieTimer = 0;
            }
        }

        /// <summary>
        /// Luo uuden esineen pelialueelle. Koordinaatit
        /// m‰‰r‰ytyv‰t satunnaisesti.
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateItem(GameTime gameTime)
        {
            itemTimer += gameTime.ElapsedGameTime.Milliseconds;
            
            if (itemTimer > itemInterval)
            {
                int itemnro = random.Next(3);

                if (itemnro == 0)
                {
                    items.Add(
                        new MedKit(
                          new Vector2((float)(random.NextDouble() * 800), (float)(random.NextDouble() * 800)), graphicsLib)
                        );
                }
                if (itemnro == 1)
                {
                    items.Add(
                        new Rifle(
                          new Vector2((float)(random.NextDouble() * 800), (float)(random.NextDouble() * 800)), graphicsLib)
                        );
                }
                if (itemnro == 2)
                {
                    items.Add(
                        new Machinegun(
                          new Vector2((float)(random.NextDouble() * 800), (float)(random.NextDouble() * 800)), graphicsLib)
                        );
                }
                itemTimer = 0;
            }
            
        }

        /// <summary>
        /// Tarkistaa ammusten, zombien, pelaajan ja esineiden v‰lisi‰ tˆrm‰yksi‰
        /// ja tˆrm‰yksen sattuessa tekee jatkotoimenpiteit‰.
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateCollisions(GameTime gameTime)
        {
            healthTimer += gameTime.ElapsedGameTime.Milliseconds;
            
            foreach (Zombie z in zombies)
            {
                soundTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (z.bSphere.Intersects(player.Sphere()))
                {
                    if (soundTimer > soundInterval)
                    {
                        //soundsLib.scream.Play(0.8f);
                        soundsLib.scream.Play( );
                        soundTimer = 0;
                    }
                    z.Collision();
                    if (healthTimer > healthInterval)
                        player.Health -= 5;
                }                                              
            }
            
            foreach (Item i in items)
            {
                if (i.Sphere().Intersects(player.Sphere()))
                {
                    i.Collision(player);
                    destroyeditems.Add(i);
                }
            }

            foreach (Item i in destroyeditems)
            {
                items.Remove(i);
            }
            
            foreach (Bullet b in bullets)
            {
                bSphere = new BoundingSphere(new Vector3(b.position.X + Game1.graphicsLib.bullet.Width / 2, b.position.Y + Game1.graphicsLib.bullet.Height / 2, 0), 3);
                foreach (Zombie z in zombies)
                {
                    
                    if (bSphere.Intersects(z.bSphere))
                    {
                        destroyedBullets.Add(b);
                        z.Health -= player.Damage;
                        z.Speed -= 0.3f;
                        z.Damage -= 2;
                        if (z.Health <= 0)
                        {
                            killedZombies.Add(z);
                            kills++;
                            z.Speed = 0;
                        }
                    }
                }
                if (!b.InArea()) destroyedBullets.Add(b);
            }

            foreach (Zombie z in killedZombies)
            {
                if (z.currentFrame >= 4)
                zombies.Remove(z);
            }

            foreach (Bullet b in destroyedBullets)
            {
                bullets.Remove(b);
            }
        }

        /// <summary>
        /// Pit‰‰ huolen siit‰, ett‰ pelaaja ei voi liikkua pelialueen ulkopuolella
        /// </summary>
        private void PlayerInArea()
        {
            player.PlayerInArea();
        }


        /// <summary>
        /// Tulitus‰‰ni, luo uuden Bullet -olion ja v‰hent‰‰ ammuksia
        /// </summary>
        private void Attack()
        {
            //soundsLib.gunShot.Play(0.5f);
            soundsLib.gunShot.Play( );
            Bullet bullet = new Bullet(player.Position, player.Direction, graphicsLib);
            bullets.Add(bullet);
            if (player.Ammo > 0)
            {
                player.Ammo--;
            }
        }

        /// <summary>
        /// Tarkistaa, onko healthia viel‰ j‰ljell‰
        /// </summary>
        /// <returns>true, jos on ja false muulloin</returns>
        private bool Alive()
        {
            if (player.Health < 1) return false;
            else return true;
        }
        #endregion


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            currentKeyboardState = Keyboard.GetState();

            switch(state)
            {
                #region menu
                case GameState.gameMenu:
                   
                  if (menuControl.Exit) base.Exit();
                  if (menuControl.Play)
                  {
                      startGame();
                  }
                  if (menuControl.HighScores)
                  {
                      scoresSaved = true;
                      gameOver();
                  }
                  if (menuControl.Help) toHelp();
                  menuControl.Update(gameTime);

                break;
                #endregion
                #region playing
                case GameState.gamePlaying:
                if (currentKeyboardState.IsKeyDown(Keys.Escape)) toMenu();

                if (!Alive())
                {
                    if (highScores.IsHighScore(kills))
                    {
                      gamePopup();
                    }else
                      gameOver();
                }



                  shot += gameTime.ElapsedGameTime.Milliseconds;


                  player.Update(gameTime);

                  UpdateCollisions(gameTime);
                  UpdateItem(gameTime);
                  PlayerInArea();
                    
                  for (int i = 0; i <= zombieHorde; i++ ) // montako zombieta luodaan jokaisella p‰ivityksell‰
                      UpdateZombieTimer(gameTime);

                  hordeTimer += gameTime.ElapsedGameTime.Milliseconds;

                  if (hordeTimer > hordeInterval)
                  {
                      soundsLib.zombieGroans.Play();
                      zombieHorde++;
                      hordeTimer = 0;
                  }

                  if (currentKeyboardState.IsKeyDown(Keys.Space))
                  {
                      if (shot > player.ShotInterval) // tulinopeus
                      {
                          Attack();
                          shot = 0;
                      }

                  }

                  foreach (Bullet b in bullets)
                  {
                     b.Update(gameTime);
                  }

                  foreach (Zombie z in zombies)
                  {
                      z.Update(gameTime, player.Position);
                  }

                  foreach (Item i in items)
                  {
                      i.Update(gameTime);
                  }


                break;
                #endregion
                #region highscores
                case GameState.gameHighScores:
                
                if (currentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    highScores.Save("Content/hiscores.xml");
                    scoresSaved = true;
                    toMenu();
                }
                break;
                #endregion

                #region help
                case GameState.gameHelp:
                if (currentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    toMenu();
                }
                break;
                #endregion
                #region popup
                case GameState.gamePopup:
                  popup.Update();
                break;
                #endregion

            }

            base.Update(gameTime);
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();

            switch(state)
            {
                #region menu
                case GameState.gameMenu:
                 spriteBatch.Draw(Game1.graphicsLib.backgroundMenu, new Vector2(0, 0), Color.White);
                 menuControl.Draw(gameTime);

               break;
                #endregion
                #region playing
                case GameState.gamePlaying:
                 spriteBatch.Draw(Game1.graphicsLib.background, new Vector2(0, 0), Color.White);

                

                 player.Draw(spriteBatch);

                 foreach (Item i in items)
                 {
                     i.Draw(spriteBatch);
                 }


                 foreach (Bullet b in bullets)
                 {
                     b.Draw(spriteBatch);
                 }

                 foreach (Zombie z in zombies)
                 {
                     z.Draw(spriteBatch);
                 }



                 spriteBatch.Draw(Game1.graphicsLib.health, new Rectangle(10, 10, player.Health*2, 30), new Rectangle(10, 10, player.Health*3, 30), Color.Red);
                 spriteBatch.DrawString(Game1.graphicsLib.text, "Health " + player.Health, new Vector2(10, 15), Color.White);
                 spriteBatch.DrawString(Game1.graphicsLib.text, "Kills: " + kills, new Vector2(550,10), Color.White);
                 spriteBatch.DrawString(Game1.graphicsLib.text, "Weapon: " + player.CurrenWeapon, new Vector2(250, 10), Color.White);
                 if (!player.DefaultWeapon())
                 {
                     spriteBatch.DrawString(Game1.graphicsLib.text, "Ammo: " + player.Ammo, new Vector2(250, 30), Color.White);
                 }
                 else
                 {
                     spriteBatch.DrawString(Game1.graphicsLib.text, "Ammo: " + "infinite", new Vector2(250, 30), Color.White);
                 }

               break;
                #endregion
                #region highscores
                case GameState.gameHighScores:
                 spriteBatch.Draw(Game1.graphicsLib.backgroundMenu, new Vector2(0, 0), Color.White);
                 spriteBatch.DrawString(Game1.graphicsLib.text, "High scores", new Vector2(350, 10), Color.Red);
                 spriteBatch.DrawString(Game1.graphicsLib.text, "Player        Kills  ", new Vector2(350, 60), Color.Red);

                 if (!highScoreEntered)
                 {
                     if (highScores.IsHighScore(kills))
                     {
                         AddHighScore(gameTime, highScores);
                     }
                 }

                 if (highScoreEntered)
                 {
                     if (!scoresSaved)
                     {
                         highScores.Save("Content/hiscores.xml");
                         scoresSaved = true;
                     }
                 }

                 if ((scoresSaved || highScores.IsHighScore(kills)))
                 {
                     DrawHighScores(spriteBatch);
                 }
                 DrawHighScores(spriteBatch);
                 spriteBatch.DrawString(Game1.graphicsLib.text, "Press Escape to exit", new Vector2(320, 500), Color.Red);

                 
                break;
                #endregion
                #region help
                case GameState.gameHelp:
                spriteBatch.Draw(Game1.graphicsLib.backgroundMenu, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(Game1.graphicsLib.text, "Help", new Vector2(350, 10), Color.Red);
                spriteBatch.DrawString(Game1.graphicsLib.text, "Controls\n" +
                                                               "--------------\n"+
                                                               "move: arrow keys\n"+
                                                               "shoot: space bar\n"+
                                                               "\n"+
                                                               "How to play?\n"+
                                                                "--------------\n"+
                                                                "Select 'Play' from the menu and\n"+
                                                                "press enter.\n"+
                                                                "\n"+
                                                                "Idea is to survive as long as you\n"+
                                                                "can and kill as meany zombies you\n"+
                                                                "can.\n"+
                                                                "\n"+
                                                                "There are three different weapons in\n"+
                                                                "this game:\n"+
                                                                "\n"+
                                                                "- Assault rifle: basic weapon with infinite ammunition\n"+
                                                                "- Machinegun: firerate is high\n"+
                                                                "- Rifle: firerate is low, but creates a great amount of\n"+
                                                                "damage.\n", new Vector2(100, 50), Color.Red);
                spriteBatch.DrawString(Game1.graphicsLib.text, "Press Escape to exit", new Vector2(320, 550), Color.Red);
                break;
                #endregion

                #region popup

                case GameState.gamePopup:

                  spriteBatch.Draw(Game1.graphicsLib.backgroundMenu, new Vector2(0, 0), Color.White);
                  popup.Draw();

                  if (popup.IsDone())
                      gameOver();

                break;

                #endregion
            }
            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        /// <summary>
        /// Piirt‰‰ highscore -listan
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawHighScores(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 20; i++)
            {
                spriteBatch.DrawString(Game1.graphicsLib.text, String.Format("{0}. ", i + 1 ), new Vector2(320, 100 + (i * 20)), Color.Red);
                spriteBatch.DrawString(Game1.graphicsLib.text, highScores.Names[i].ToString(), new Vector2(350, 100 + (i * 20)), Color.Red);
                spriteBatch.DrawString(Game1.graphicsLib.text, "" + highScores.Kills[i], new Vector2(460, 100 +(i * 20)), Color.Red);
            }
        }

        /// <summary>
        /// Lis‰‰ highscore -listaan uuden tuloksen
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="highScores"></param>
        private void AddHighScore(GameTime gameTime, HighScores highScores)
        {

            highScores.AddScore(popup.Name, kills);
            highScoreEntered = true;


        }




    }
}
