﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace flappyBird
{
    public class Bird
    {

        private const double acceleration = 200.0;
        public double velocity = 0;
        private double position = 0.0;
        private float angle = 0f;
        private Texture2D texture;
        private Texture2D test_texture;
        public Bird(Texture2D texture2, Texture2D texture3)
        {
            texture = texture2;
            test_texture = texture3;
        }
        public void update(double interval)

        {
            velocity += acceleration * interval;
            position += velocity * interval;
            if ((angle <= 1.5 && velocity > 0) || (angle >= -1.5 && velocity < 0))
            {
                angle = (float)velocity / 200;
            }
        }
        public void jump() { velocity = -150; }
        public void draw(SpriteBatch spriteBatch)
        {
            int size = 260;
            Vector2 location = new Vector2(60, Convert.ToInt32(position));
            Rectangle sourceRectangle = new Rectangle(0, 0, size, size);
            Vector2 origin = new Vector2(size / 2, size / 2);
            float scale = 0.4f;

            spriteBatch.Draw(
                texture,
                location,
                null,
                sourceRectangle,
                origin, angle,
                new Vector2(scale, scale),
                Color.Green,
                SpriteEffects.None,
                1
            );
            spriteBatch.Draw(test_texture, new Rectangle(60 - (int)(size * (scale / 2)), (Convert.ToInt32(position)) - (int)(size * (scale / 2)), (int)(size * scale), (int)(size * scale)), Color.White);
        }
    }
    public class Pipes
    {
        private Texture2D texture;
        public Pipes(Texture2D texture2)
        {
            texture = texture2;
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(200, 60, 100, 100), Color.White);
        }
    }
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont font;
        private TimeSpan lastTime;

        private Bird bird;
        private Pipes pipes;
        private Boolean pressedLastTick = false;

        private int score = 0;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("score");
            bird = new Bird(Content.Load<Texture2D>("flappybird"), Content.Load<Texture2D>("pipe"));
            pipes = new Pipes(Content.Load<Texture2D>("pipe"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (lastTime == null) lastTime = gameTime.TotalGameTime;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            double interval = (gameTime.TotalGameTime - lastTime).TotalSeconds;
            lastTime = gameTime.TotalGameTime;
            Console.WriteLine(bird.velocity);
            bird.update(interval);
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (!pressedLastTick)
                {
                    Console.WriteLine("Space key pressed");
                    bird.jump();
                }
                pressedLastTick = true;
            }
            else
            {
                pressedLastTick = false;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);

            spriteBatch.Begin();

            bird.draw(spriteBatch);
            pipes.draw(spriteBatch);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(100, 100), Color.Green);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
