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
using System.Runtime.InteropServices;

namespace GravField
{
    //For setting window location
    public class User32
    {
        [DllImport("user32.dll")]
        public static extern void SetWindowPos(uint Hwnd, uint Level, int X,
            int Y, int W, int H, uint Flags);
    } 

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        static public MouseState mouseState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static public Random rand = new Random();
        Texture2D whiteBallImage;

        static public List<Particle> balls = new List<Particle>();
        static public int numBalls = 25000;
        public static Vector2 gravPoint = new Vector2(512, 384);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
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

            User32.SetWindowPos((uint)this.Window.Handle, 0, 2000, 50,
                   graphics.PreferredBackBufferWidth,
                   graphics.PreferredBackBufferHeight, 0);

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            whiteBallImage = Content.Load<Texture2D>("WhiteParticle");

            Color c = new Color();
            double theta;
            double r;
            for (int i = 0; i < numBalls; i++)
            {
                c.A = 255;
                c.R = (byte)rand.Next(0, 256);
                c.G = (byte)rand.Next(0, 256);
                c.B = (byte)rand.Next(0, 256);
                r = rand.NextDouble() * 700 + 100;
                theta = rand.NextDouble() * (2 * Math.PI);
                Particle ball = new Particle();
                ball.Setup(whiteBallImage, c,
                    rand.Next(-100,1124), rand.Next(-100,868),
                    (float)rand.NextDouble() * .2f - .1f, (float)rand.NextDouble() * .2f - .1f,
                    .008f, 1f * (float)Math.Pow(10, 1));
                ball.bound = false;
                balls.Add(ball);
            }
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();
            foreach (Particle ball in balls)
            {
                ball.Update();
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            foreach (Particle ball in balls)
            {
                ball.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
