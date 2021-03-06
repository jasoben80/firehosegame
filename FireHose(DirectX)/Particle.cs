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
using Tao.Sdl;

#endregion

namespace FireHose_DirectX_
{
    class Particle

    {
        //public Vector2 ParticlePosition;
        public Body ParticleBody;
        public Color ParticleColor;
        public float DrawScale;
        public int ParticleTTL;
        public int InitialTTL;
        public Texture2D CurrentTexture;
        private Random randomRotation;
        public float RandomRotation;

        public Particle(Body body, Color color, float scale, int ttl, Texture2D currentTexture)
        {
            //ParticlePosition = particlePosition;
            ParticleBody = body;
            ParticleColor = color;
            DrawScale = scale;
            ParticleTTL = ttl;
            InitialTTL = ttl;

            CurrentTexture = currentTexture;
            randomRotation = new Random();
            RandomRotation = randomRotation.Next(0, 360);

        }

        public void Update()
        {
            RandomRotation += .05f;
        }

       
    }
}
