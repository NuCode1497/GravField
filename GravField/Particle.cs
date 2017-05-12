//
// Copyright (c) 2010 Cody Neuburger
// All rights reserved.
//
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

namespace GravField
{
    public class Particle
    {
        public static float G = (6.673f * (float)Math.Pow(10, -11)); //universal gravitational constant
        public Vector2 origin = new Vector2();
        public float rotation = 0;
        public float angularVelocity = 0;
        public float angularAcceleration = 0;
        public Vector2 location = new Vector2(300, 300);
        public Vector2 velocity = new Vector2(0.5f, 0.5f);
        public Vector2 acceleration = new Vector2(0f, 0f);
        Texture2D image;
        public float radius = 137;
        public float scale = 1.0f;
        public Color color = Color.White;
        public float friction = 1f;
        public bool isActive = true;
        public bool hitWall = false;
        public float mass;
        public bool collidable = false;
        public bool bound = true;

        public virtual void Setup(Texture2D img, Color c, float x, float y, float vx, float vy, float scal, float m)
        {
            image = img;
            color = c;
            location.X = x;
            location.Y = y;
            velocity.X = vx;
            velocity.Y = vy;
            mass = m;
            scale = scal;
            origin.X = (image.Width) / 2;
            origin.Y = (image.Height) / 2;
        }

        public virtual void Update()
        {
            if (isActive)
            {
                velocity += acceleration;
                velocity *= friction;
                location += velocity;
            }

            acceleration = Vector2.Zero;

            if (location.X > 1124 || location.X < -100 || location.Y > 868 || location.Y < -100)
            {
                location.X = Game1.rand.Next(-100,1124);
                location.Y = Game1.rand.Next(-100, 868);
                velocity.X = (float)Game1.rand.NextDouble() * .2f - .1f;
                velocity.Y = (float)Game1.rand.NextDouble() * .2f - .1f;
            }

            float gravRadius = 20;
            float d = Vector2.Distance(Game1.gravPoint, location);
            double phi = Math.Atan2((Game1.gravPoint.Y - location.Y), (Game1.gravPoint.X - location.X));
            Vector2 gPoint = new Vector2((float)(Game1.gravPoint.X + gravRadius * Math.Cos(phi + 1)), (float)(Game1.gravPoint.Y + gravRadius * Math.Sin(phi + 1)));
            phi = Math.Atan2((gPoint.Y - location.Y), (gPoint.X - location.X));
            if (d > gravRadius)
            {
                double gravAccelMag = 100 / (d * d);
                d = Vector2.Distance(gPoint, location);
                Vector2 gravAccel = new Vector2((float)(gravAccelMag * Math.Cos(phi)), (float)(gravAccelMag * Math.Sin(phi)));
                acceleration += gravAccel;
            }
            else
            {
                velocity *= .9f;
            }

            if (Game1.mouseState.LeftButton == ButtonState.Pressed)
            {
                gPoint = new Vector2(Game1.mouseState.X, Game1.mouseState.Y);
                phi = Math.Atan2((gPoint.Y - location.Y), (gPoint.X - location.X));
                d = Vector2.Distance(gPoint, location);
                double gravAccelMag = -200 / (d * d);
                Vector2 gravAccel = new Vector2((float)(gravAccelMag * Math.Cos(phi)), (float)(gravAccelMag * Math.Sin(phi)));
                velocity += gravAccel;

            }
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.Draw(image, location, null, color, rotation, origin, scale, SpriteEffects.None, 1.0f);
            }
        }
    }
}
