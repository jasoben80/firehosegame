﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;

#endregion

namespace FireHose_DirectX_
{
    class Gun
    {

        //Player player;
        public Vector2 PlayerPosition;
        public Vector2 ParticleEmitterLocation;
        public Vector2 ParticleVelocity;
        
        public Vector2 GunOrigin;
        public float GunRotation;

        // public String MyString;

        Texture2D fireGunTexture;
        Texture2D waterGunTexture;
        Texture2D particleTexture;
        Texture2D steamTexture;

        ParticleEngine particleEngine;

        public Color ParticleColor;

        public World ThisWorld;

        Random randomVelocity;
        public int randomizer;
        public int rx;
        public int ry;

        //this bool decides which gun to use
        public bool IsItFire;
        
        public bool isFiring = false;

        public int PlayerNumber;

        public Player Player;

        public Gun(Player player, Vector2 playerPosition, World world, bool isItFire, int playerNumber)
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            PlayerPosition = playerPosition;
            ThisWorld = world;
            PlayerNumber = playerNumber;
            Player = player;
            IsItFire = isItFire;

            randomVelocity = new Random();

            if (IsItFire == true)
            {
                ParticleColor = Color.Orange;
                randomizer = 15;
                
               
            }

            if (IsItFire == false)
            {
                ParticleColor = Color.Blue;
                randomizer = 20;
            }
            
        }

        public void LoadContent(ContentManager content)
        {

            fireGunTexture = content.Load<Texture2D>("firegun.png");
            waterGunTexture = content.Load<Texture2D>("watergun.png");
            particleTexture = content.Load<Texture2D>("particle.png");
            steamTexture = content.Load<Texture2D>("steam.png");

            particleEngine = new ParticleEngine(ThisWorld, particleTexture, steamTexture, PlayerPosition, ParticleVelocity, ParticleColor, PlayerNumber);
                
        }

        public void Update(Vector2 playerPosition, Controls controls, List<Keys> playerControls)
        {
            PlayerPosition = playerPosition;
            Blast(controls, playerControls);
            //if (isFiring == false)
            //{
            //    particleEngine.Update(isFiring, ParticleEmitterLocation, ParticleVelocity);
            //}

            particleEngine.Update(isFiring, ParticleEmitterLocation, ParticleVelocity);
            GunOrigin = controls.Fly(IsItFire) * 10;
            GunOrigin = new Vector2(GunOrigin.X, -1 * GunOrigin.Y);
            GunRotation = (float)(Math.Atan2(GunOrigin.Y, GunOrigin.X));

            
            //call dummy method for pushing information to text box for testing
            //GetLocation();
            
        }

        public void Draw(SpriteBatch sb)
        {
            if (IsItFire == true)
                sb.Draw(fireGunTexture, ConvertUnits.ToDisplayUnits(PlayerPosition), null, Color.White, GunRotation, new Vector2(0, 0), .2f, SpriteEffects.None, 0f);
            if (IsItFire == false)    
                sb.Draw(waterGunTexture, ConvertUnits.ToDisplayUnits(PlayerPosition), null, Color.White, GunRotation, new Vector2 (0,0), .2f, SpriteEffects.None, 0f);
            
            particleEngine.Draw(sb);
           
        }

        public void Blast(Controls controls, List<Keys> playerControls)
        {

            if (Player.playerBody.BodyType != BodyType.Kinematic)
            {
                rx = randomVelocity.Next(-randomizer, randomizer);
                ry = randomVelocity.Next(-randomizer, randomizer);

                Vector2 calculatedGunPosition = new Vector2((int)(Math.Cos(GunRotation) * 10), (int)(Math.Sin(GunRotation) * 10));
                ParticleEmitterLocation = ParticleEmitterLocation = PlayerPosition + calculatedGunPosition / 7;
                ParticleVelocity = ConvertUnits.ToDisplayUnits(ParticleEmitterLocation + new Vector2(rx / 15, ry / 15)) - ConvertUnits.ToDisplayUnits(PlayerPosition);

                if (!IsItFire)
                {

                    if ((!controls.isHeld(Keys.U, Buttons.LeftShoulder)) &&
                    ((controls.isThumbStick(Buttons.LeftThumbstickDown) ||
                    controls.isThumbStick(Buttons.LeftThumbstickUp) ||
                    controls.isThumbStick(Buttons.LeftThumbstickLeft) ||
                    controls.isThumbStick(Buttons.LeftThumbstickRight))))
                    //if(controls.isHeld(Keys.R, Buttons.A))
                    {
                        isFiring = true;
                        particleEngine.GenerateNewParticle(30);
                        particleEngine.Update(isFiring, ParticleEmitterLocation, ParticleVelocity);
                    }
                    else
                    {
                        isFiring = false;
                        particleEngine.Update(isFiring, ParticleEmitterLocation, ParticleVelocity);
                    }
                }
                else
                {
                    if (controls.isThumbStick(Buttons.RightThumbstickUp) || controls.isThumbStick(Buttons.RightThumbstickLeft) || controls.isThumbStick(Buttons.RightThumbstickRight) || controls.isThumbStick(Buttons.RightThumbstickDown))
                    {
                        isFiring = true;
                        particleEngine.GenerateNewParticle(120);
                        particleEngine.Update(isFiring, ParticleEmitterLocation, ParticleVelocity);
                    }
                    else
                    {
                        isFiring = false;
                        particleEngine.Update(isFiring, ParticleEmitterLocation, ParticleVelocity);
                    }
                }
            }
            
            
        }


        //This is a dummy method for rending information to the text box

        //public string GetLocation()
        //{
        //    //return MyString = ConvertUnits.ToDisplayUnits(particleEngine.ParticleEmitterLocation).ToString();
        //    return MyString = ConvertUnits.ToDisplayUnits(PlayerPosition).ToString();

        //}

    }
}
