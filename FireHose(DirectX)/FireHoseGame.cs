﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;
using Tao.Sdl; 

#endregion

namespace FireHose_DirectX_
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FireHoseGame : Game
    {

        #region fields and properties
        //Standard mono stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        Song mainSong;
        float songVolume;

        public string WinMessage = null;
        public Color WinColor;

        //We make a list of Controls so we can easily pass the right control information from separate player objects to their associated (by an integer value) control object
        public List<Controls> Controls;

        //Define the farseer physics world variable
        World world;

        //define the GameWorld property so we can access it from other classes
        public World GameWorld
        {
            get { return world; }
        }

        Texture2D levelTexture;
        Texture2D winBarTexture;


        //TEMPORARY STUFF FOR GAME EXPO
        CountDown controlsCountdown;

        //Define the level variables (these are just boxes that are static and register collisions)
        Body level;
        Body ceiling;
        Body bottom;
        Body bottomLeft;
        Body bottomRight;
        Body topLeft;
        Body topRight;
        Body sideLeft;
        Body sideRight;
        Body startLeft;
        Body startRight;
        Body lipLeft;
        Body lipRight;

        public List<Body> levelBlocks;
        private List<Altar> altars;

        private Vector2 levelOrigin;

        
        public Vector2 LevelOrigin
        {
            get { return levelOrigin; }
        }

        private Vector2 screenCenter;

        public Vector2 ScreenCenter
        {
            get { return screenCenter; }
        }


        //Level Block Positions

        private Vector2 levelPosition;

        public Vector2 LevelPosition
        {
            get { return levelPosition; }
            set { levelPosition = value; }
        }

        private Vector2 ceilingPosition;

        public Vector2 CeilingPosition
        {
            get { return ceilingPosition; }
            set { ceilingPosition = value; }
        }

        private Vector2 bottomPosition;

        public Vector2 BottomPosition
        {
            get { return bottomPosition; }
            set { bottomPosition = value; }
        }

        private Vector2 bottomLeftPosition;

        public Vector2 BottomLeftPosition
        {
            get { return bottomLeftPosition; }
            set { bottomLeftPosition = value; }
        }

        private Vector2 bottomRightPosition;

        public Vector2 BottomRightPosition
        {
            get { return bottomRightPosition; }
            set { bottomRightPosition = value; }
        }

        private Vector2 topLeftPosition;

        public Vector2 TopLeftPosition
        {
            get { return topLeftPosition; }
            set { topLeftPosition = value; }
        }

        private Vector2 topRightPosition;

        public Vector2 TopRightPosition
        {
            get { return topRightPosition; }
            set { topRightPosition = value; }
        }

        private Vector2 sideLeftPosition;

        public Vector2 SideLeftPosition
        {
            get { return sideLeftPosition; }
            set { sideLeftPosition = value; }
        }

        private Vector2 sideRightPosition;

        public Vector2 SideRightPosition
        {
            get { return sideRightPosition; }
            set { sideRightPosition = value; }
        }

        private Vector2 startLeftPosition;

        public Vector2 StartLeftPosition
        {
            get { return startLeftPosition; }
            set { startLeftPosition = value; }
        }

        private Vector2 startRightPosition;

        public Vector2 StartRightPosition
        {
            get { return startRightPosition; }
            set { startRightPosition = value; }
        }

        private Vector2 lipLeftPosition;

        public Vector2 LipLeftPosition
        {
            get { return lipLeftPosition; }
            set { lipLeftPosition = value; }
        }

        private Vector2 lipRightPosition;

        public Vector2 LipRightPosition
        {
            get { return lipRightPosition; }
            set { lipRightPosition = value; }
        }

        //Level Hitbox positions
        Vector2 box1400;
        Vector2 boxceiling;
        Vector2 boxbottom;
        Vector2 boxbottomleft;
        Vector2 boxbottomright;
        Vector2 boxtopleft;
        Vector2 boxtopright;
        Vector2 boxsideleft;
        Vector2 boxsideright;
        Vector2 boxstartleft;
        Vector2 boxstartright;
        Vector2 boxlipleft;
        Vector2 boxlipright;

        Player player1;
        Player player2;

        //Altars
        Altar altar1;
        Altar altar2;
        Altar altar3;
        Altar altar4;
        Altar altar5;

        //Define the list of keys-- so we can easily pass the right keys to the right player object AND define the keys later on
        //TODO: Probably need to do the same thing for gamepad controls

        public List<Keys> Player1KeyControls;
        public List<Keys> Player2KeyControls;
        
        //define the particle texture for either water or fire variables        
        public Texture2D ParticleTexture;

        //these properties grab the screencenter property and determine where the players start       
        public Vector2 Player1StartPosition
        {
            get { return (new Vector2(250, 800));  }

        }

        public Vector2 Player2StartPosition
        {
            get { return (new Vector2(1550, 800)); }

        }

        //private Rectangle drawRectangle; 

        #endregion

        public FireHoseGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 1000;
            
            this.graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            world = new World(new Vector2(0, 9.8f));
            
            //This conversion doohicky is from farseer, and it is necessary to define our conversion ratio because farseer is build to work with metric units
            //TODO: probably need a public variable for this.

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);


            //TEMPORARY STUFF FOR GAME EXPO
            controlsCountdown = new CountDown(1800);
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            //This is where we make our list of control objects
            //I add in a dummy one for controls[0] so that I don't have to pass a zero to indicate player one, a one to indicate player two
            List<Controls> controls = new List<Controls>(); 
            controls.Add(new Controls(1));
            controls.Add(new Controls(1));
            controls.Add(new Controls(2));

            Controls = controls; 

            //Here we can define lists of our player keys
            //TODO: probably need to make one of these for gamepad buttons as well so we can 
            Player1KeyControls = new List<Keys>() 
            {
                Keys.A, Keys.D, Keys.Space, Keys.W, Keys.F
            };

           
            Player2KeyControls = new List<Keys>() 
            {
                Keys.J, Keys.L, Keys.LeftControl, Keys.I, Keys.H
            };

            //this.IsMouseVisible = true;

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // TODO: use this.Content to load your game content here

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                        
            levelTexture = Content.Load<Texture2D>("level.png");
            winBarTexture = Content.Load<Texture2D>("WinBar");

            //define the screen center and where the middle level piece texture should be placed in relation to the level object
            screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            levelOrigin = new Vector2(levelTexture.Width / 2f, levelTexture.Height / 2f);

            //This lineardamping is for when we implement a fixture beneath the player body object that slows it while on the ground
            //player.LinearDamping = 2f;

            mainSong = Content.Load<Song>("juanitos-firehose");
            MediaPlayer.Play(mainSong);
            MediaPlayer.Volume = .10f;
            MediaPlayer.IsRepeating = true;

            

            BuildLevel();

            player1 = new Player(Player1StartPosition, world, 1);
            player1.LoadContent(this.Content);

            player2 = new Player(Player2StartPosition, world, 2);
            player2.LoadContent(this.Content);

            BuildAltars();

            font = Content.Load<SpriteFont>("PescaFont");
            
        }

      

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
          //  if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            //TODO the previous is useful for exiting, but is busted on lab computers!

            // TODO: Add your update logic here

            Controls[1].Update();
            Controls[2].Update();

            player1.Update(Controls[1], gameTime, Player1KeyControls);
            player2.Update(Controls[2], gameTime, Player2KeyControls);

            altar1.Update();
            altar2.Update();
            altar3.Update();
            altar4.Update();
            altar5.Update();

            KeepScore();

            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            songVolume = .5f * (((float)player1.PlayerScore + (float)player2.PlayerScore) / 2000000); 
            MediaPlayer.Volume = .10f + songVolume;

            //TEMPORARY STUFF FOR GAME EXPO
            YouWin();

            //TEMPORARY STUFF FOR GAME EXPO
            NewGame(Controls[1], Controls[2]);



            //TEMPORARY STUFF FOR GAME EXPO
            controlsCountdown.Update();

            base.Update(gameTime);

        }


        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);

            //draw level and players
            spriteBatch.Begin();

            DrawLevels();

            //spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(level.Position), null , Color.White, 0f, LevelOrigin, 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(level.Position), null, Color.White, 0f, LevelOrigin, new Vector2(2f,1f), SpriteEffects.None, 0f);

            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);

            altar1.Draw(spriteBatch);
            altar2.Draw(spriteBatch);
            altar3.Draw(spriteBatch);
            altar4.Draw(spriteBatch);
            altar5.Draw(spriteBatch);

            //Draw Winning Bars
            spriteBatch.Draw(winBarTexture, new Vector2(0, 10), null, Color.Black, 0f, new Vector2(0, 0), new Vector2(2f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(winBarTexture, new Vector2(0, 10), null, Color.CornflowerBlue, 0f, new Vector2(0, 0), new Vector2(player1.WinPercent, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(winBarTexture, new Vector2(1800, 60), null, Color.GreenYellow, (float)Math.PI, new Vector2(0, 0), new Vector2(player2.WinPercent, 1f), SpriteEffects.None, 0f);

            //Draw the center line
            spriteBatch.Draw(levelTexture, new Vector2(900, 0), null, Color.Aquamarine, 0f, Vector2.Zero, new Vector2(.2f, 1f), SpriteEffects.None, 0f);

            spriteBatch.DrawString(font, player1.PlayerScore.ToString(), new Vector2((900 * player1.WinPercent) + 120, 20), Color.CornflowerBlue, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, player2.PlayerScore.ToString(), new Vector2(1660 - (900 * player2.WinPercent), 20), Color.GreenYellow, 0f, new Vector2(0,0), 2f, SpriteEffects.None, 0f);

           // spriteBatch.DrawString(font, Mouse.GetState().Position.ToString(), new Vector2(100, 300), Color.Black);

            if (player1.RestartTimer < player1.TotalRestartTime)
            {
                spriteBatch.DrawString(font, ((int)(player1.RestartTimer / 60) + 1).ToString(), new Vector2(50, 800), Color.CornflowerBlue, 0f, new Vector2(0, 0), 5f, SpriteEffects.None, 0f);
            }

            if (player2.RestartTimer < player2.TotalRestartTime)
            {
                spriteBatch.DrawString(font, ((int)(player2.RestartTimer / 60) + 1).ToString(), new Vector2(1600, 800), Color.GreenYellow, 0f, new Vector2(0, 0), 5f, SpriteEffects.None, 0f);
            }

            if (WinMessage != null)
            {
                spriteBatch.DrawString(font, WinMessage, new Vector2(100, 500), WinColor, 0f, new Vector2(0, 0), 10f, SpriteEffects.None, 0f);
              
                spriteBatch.DrawString(font, "Press BACK to restart match", new Vector2(100, 700), Color.White, 0f, new Vector2(0, 0), 3f, SpriteEffects.None, 0f);
            }


            //TEMPORARY STUFF FOR GAME EXPO
            if (controlsCountdown.CountDownIsDone == false)
            {
                spriteBatch.DrawString(font, "Firehose!", new Vector2(520, 400), Color.Yellow, 0f, new Vector2(0, 0), 5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Controls:", new Vector2(520, 480), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Left Shoulder Button + Left Thumbstick to Walk", new Vector2(520, 520), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Left Thumbstick to Blast Water", new Vector2(520, 540), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Right Thumbstick to Blast Fire", new Vector2(520, 560), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Goal:", new Vector2(980, 480), Color.LightGreen, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Light Altars, Burninate Peasants", new Vector2(980, 520), Color.LightGreen, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }


            spriteBatch.End();
                        
            base.Draw(gameTime);
        }

        public bool levelCollided(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat3)
            {
                fixtureB.Body.Awake = false;
                return true;
            }
            else
            {
                return true;
            }
        }

        public void BuildAltars()
        {
            altar1 = new Altar(world, new Vector2(130, 695));
            altar1.LoadContent(this.Content);

            altar2 = new Altar(world, new Vector2(1660, 695));
            altar2.LoadContent(this.Content);

            altar3 = new Altar(world, new Vector2(900, 700));
            altar3.LoadContent(this.Content);

            altar4 = new Altar(world, new Vector2(600, 350));
            altar4.LoadContent(this.Content);

            altar5 = new Altar(world, new Vector2(1200, 350));
            altar5.LoadContent(this.Content);
            
            Altar[] theAltars = { altar1, altar2, altar3, altar4, altar5 };

            altars = new List<Altar>(theAltars);

        }

        public void BuildLevel()
        {
            levelPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(0f, 0f);
            ceilingPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(0f, -7.5f);
            bottomPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(0f, 7f);
            bottomLeftPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(-13f, 7f);
            bottomRightPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(13f, 7f);
            topLeftPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(-12f, -4f);
            topRightPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(12f, -4f);
            sideLeftPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(-14f, 0f);
            sideRightPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(14f, 0f);
            startLeftPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(-12f, 4f);
            startRightPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(12f, 4f);
            lipLeftPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(-9.5f, 3.8f);
            lipRightPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(9.5f, 3.8f);

            level = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(800f), ConvertUnits.ToSimUnits(200f), 1f, levelPosition);
            ceiling = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(2000f), ConvertUnits.ToSimUnits(100f), 1f, ceilingPosition);
            bottom = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(500f), ConvertUnits.ToSimUnits(400f), 1f, bottomPosition);
            bottomLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(600f), ConvertUnits.ToSimUnits(50f), 1f, bottomLeftPosition);
            bottomRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(600f), ConvertUnits.ToSimUnits(50f), 1f, bottomRightPosition);
            topLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(500f), ConvertUnits.ToSimUnits(25f), 1f, topLeftPosition);
            topRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(500f), ConvertUnits.ToSimUnits(25f), 1f, topRightPosition);
            sideLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(25f), ConvertUnits.ToSimUnits(500f), 1f, sideLeftPosition);
            sideRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(25f), ConvertUnits.ToSimUnits(500f), 1f, sideRightPosition);
            startLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(300f), ConvertUnits.ToSimUnits(25f), 1f, startLeftPosition);
            startRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(300f), ConvertUnits.ToSimUnits(25f), 1f, startRightPosition);
            lipLeft = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(25f), ConvertUnits.ToSimUnits(50f), 1f, lipLeftPosition);
            lipRight = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(25f), ConvertUnits.ToSimUnits(50f), 1f, lipRightPosition);

            Body[] levelItems = {
                                    level, ceiling, bottom, bottomLeft, bottomRight, topLeft, topRight, sideLeft, sideRight, startLeft, startRight, lipLeft, lipRight
                                };


            levelBlocks = new List<Body>(levelItems);
            
            foreach (Body levelThing in levelBlocks)
            {
                levelThing.CollisionCategories = Category.Cat4;
                levelThing.CollidesWith = Category.Cat1 | Category.Cat2 | Category.Cat3 | Category.Cat13;
                levelThing.IsStatic = true;
                levelThing.Restitution = .3f;
                levelThing.Friction = .5f;
                levelThing.OnCollision += levelCollided;
            }
        }

        public void DrawLevels()
        {
            box1400 = new Vector2(8f, 2f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(level.Position), null, Color.White, 0f, levelOrigin, box1400, SpriteEffects.None, 0f);

            boxceiling = new Vector2(20f, 1f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(ceiling.Position), null, Color.White, 0f, levelOrigin, boxceiling, SpriteEffects.None, 0f);

            boxbottom = new Vector2(5f, 4f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(bottom.Position), null, Color.White, 0f, levelOrigin, boxbottom, SpriteEffects.None, 0f);

            boxbottomleft = new Vector2(6f, .5f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(bottomLeft.Position), null, Color.White, 0f, levelOrigin, boxbottomleft, SpriteEffects.None, 0f);

            boxbottomright = new Vector2(6f, .5f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(bottomRight.Position), null, Color.White, 0f, levelOrigin, boxbottomright, SpriteEffects.None, 0f);

            boxtopleft = new Vector2(5f, .25f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(topLeft.Position), null, Color.White, 0f, levelOrigin, boxtopleft, SpriteEffects.None, 0f);

            boxtopright = new Vector2(5f, .25f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(topRight.Position), null, Color.White, 0f, levelOrigin, boxtopright, SpriteEffects.None, 0f);

            boxsideleft = new Vector2(.25f, 5f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(sideLeft.Position), null, Color.White, 0f, levelOrigin, boxsideleft, SpriteEffects.None, 0f);

            boxsideright = new Vector2(.25f, 5f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(sideRight.Position), null, Color.White, 0f, levelOrigin, boxsideright, SpriteEffects.None, 0f);

            boxstartleft = new Vector2(3f, .25f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(startLeft.Position), null, Color.White, 0f, levelOrigin, boxstartleft, SpriteEffects.None, 0f);

            boxstartright = new Vector2(3f, .25f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(startRight.Position), null, Color.White, 0f, levelOrigin, boxstartright, SpriteEffects.None, 0f);

            boxlipleft = new Vector2(.25f, .5f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(lipLeft.Position), null, Color.White, 0f, levelOrigin, boxlipleft, SpriteEffects.None, 0f);

            boxlipright = new Vector2(.25f, .5f);
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(lipRight.Position), null, Color.White, 0f, levelOrigin, boxlipright, SpriteEffects.None, 0f);
        }

        public void KeepScore() 
        {
            foreach (Altar altar in altars)
            {
                if (altar.AltarIsLit)
                {
                    if (altar.PlayerNumber == 1)
                        player1.PlayerScore += 75;
                    if (altar.PlayerNumber == 2)
                        player2.PlayerScore += 75;
                }
                if (altar.FivePercentPenalty)
                {
                    if (altar.PlayerNumber == 1)
                    {
                        player1.PlayerScore = player1.PlayerScore - (int)(.05f * player1.PlayerScore);
                        player2.PlayerScore = player2.PlayerScore + (int)(.05f * player1.PlayerScore);
                    }
                    if (altar.PlayerNumber == 2)
                    {
                        player2.PlayerScore = player2.PlayerScore - (int)(.05f * player2.PlayerScore);
                        player1.PlayerScore = player1.PlayerScore + (int)(.05f * player2.PlayerScore);
                    }
                }
            }
        }

        public void YouWin()
        {
            if (player1.PlayerScore >= 1000000)
            {
                WinMessage = "Player One Wins!";
                WinColor = Color.CornflowerBlue;
                player2.PlayerScore = 0;
            }
            if (player2.PlayerScore >= 1000000)
            {
                WinMessage = "Player Two Wins!";
                WinColor = Color.GreenYellow;
                player1.PlayerScore = 0;
            }
            
        }


        //TEMPORARY STUFF FOR GAME EXPO
        public void NewGame(Controls controls1, Controls controls2)
        {
            if (WinMessage != null)
            {
                if (controls1.onPress(Keys.Escape, Buttons.Back) || controls2.onPress(Keys.Escape, Buttons.Back))
                {
                    controlsCountdown = null;

                    controlsCountdown = new CountDown(1800);

                    player1.PlayerHealth = -100;
                    player2.PlayerHealth = -100;
                    
                    player1.Restart();
                    player2.Restart();
                    
                    foreach (Altar altar in altars)
                    {
                        altar.AltarAmount = 0;
                    }

                    player1.PlayerScore = 0;
                    player2.PlayerScore = 0;

                    WinMessage = null;
                }
            }
        }
    }
}
