using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FNAScreenSaver
{
    /// <summary>
    /// основной класс
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /// <summary>
        /// задний фон
        /// </summary>
        public Texture2D backgroundTexture;
        /// <summary>
        /// снежинка
        /// </summary>
        public Texture2D snowFlakeTexture;

        SnowFlake[] snowFlakes;   // структура теперь берётся из отдельного файла
        /// <summary>
        /// рандом
        /// </summary>
        public Random random = new Random();

        const int SnowCount = 1200;
        const int sizeFlake = 50;
        const float SnowDepthDivider = 50f;
        const float SnowParallaxMultiplier = 20f;
        const float SnowBaseSpeed = 30f;
        const float SnowSpeedScale = 100f;
        const float SnowScaleMin = 0.3f;
        const float SnowScaleRange = 0.7f;

        /// <summary>
        /// конструктор по умолчанию
        /// </summary>
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            Window.IsBorderlessEXT = true;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            snowFlakes = new SnowFlake[SnowCount];
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("villageBackground");
            snowFlakeTexture = Content.Load<Texture2D>("snowFlake");

            for (int i = 0; i < SnowCount; i++)
            {
                snowFlakes[i].Position = new Vector2(
                    random.Next(graphics.PreferredBackBufferWidth),
                    random.Next(graphics.PreferredBackBufferHeight)
                );

                snowFlakes[i].Scale = SnowScaleMin + (float)random.NextDouble() * SnowScaleRange;
                snowFlakes[i].Speed = SnowBaseSpeed + snowFlakes[i].Scale * SnowSpeedScale;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().GetPressedKeys().Length > 0)
                Exit();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < SnowCount; i++)
            {
                snowFlakes[i].Position.Y += snowFlakes[i].Speed * delta;
                snowFlakes[i].Position.X += (float)Math.Sin(snowFlakes[i].Position.Y / SnowDepthDivider) * SnowParallaxMultiplier * delta;

                if (snowFlakes[i].Position.Y > graphics.PreferredBackBufferHeight)
                {
                    snowFlakes[i].Position.Y = -random.Next(20);
                    snowFlakes[i].Position.X = random.Next(graphics.PreferredBackBufferWidth);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            spriteBatch.Draw(backgroundTexture,
                new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                Color.White);

            foreach (var flake in snowFlakes)
            {
                spriteBatch.Draw(
                    snowFlakeTexture,
                    new Rectangle((int)flake.Position.X, (int)flake.Position.Y, sizeFlake, sizeFlake),
                    Color.White
                );
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}