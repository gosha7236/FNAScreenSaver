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

            struct SnowFlake
            {
           /// <summary>
           /// позиция
           /// </summary>
                public Vector2 Position;
           /// <summary>
           /// скорость
           /// </summary>
                public float Speed;
           /// <summary>
           /// масштаб
           /// </summary>
                public float Scale;
            }

          SnowFlake[] snowFlakes;
        /// <summary>
        /// рандом
        /// </summary>
           public Random random = new Random();

            const int SnowCount = 1200; // можно менять 1000-1500
        const int sizeFlake = 50;
        const float SnowDepthDivider = 50f; // задает "глубину" сцены
        const float SnowParallaxMultiplier = 20f; // увеличивает видимое смещение при паденнии
        const float SnowBaseSpeed = 30f; // базовая скорость падения для самого маленького медленного снежка
        const float SnowSpeedScale = 100f; // множитель скорости, привязанный к размеру/глубине
        const float SnowScaleMin = 0.3f; // минимальный масштаб снежинки
        const float SnowScaleRange = 0.7f; // размах случайного добавления к минимальному масштабу
        /// <summary>
        /// конструктор класса по умолчанию
        /// </summary>
        public Game()
            {
                graphics = new GraphicsDeviceManager(this);
                Content.RootDirectory = "Content";
                IsMouseVisible = false;
                Window.IsBorderlessEXT = true;

                // Разворачиваем на весь экран
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

                // Инициализация снежинок
                for (int i = 0; i < SnowCount; i++)
                {
                    snowFlakes[i].Position = new Vector2(
                        random.Next(graphics.PreferredBackBufferWidth),
                        random.Next(graphics.PreferredBackBufferHeight)
                    );

                    // масштаб — случайный (маленькие "далеко", большие — "близко")
                    snowFlakes[i].Scale = SnowScaleMin + (float)random.NextDouble() * SnowScaleRange;
                    // скорость — зависит от масштаба
                    snowFlakes[i].Speed = SnowBaseSpeed + snowFlakes[i].Scale * SnowSpeedScale;
                }
            }
            protected override void Update(GameTime gameTime)
            {
                // Завершение по нажатию клавиши
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    Exit();
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                // Двигаем снежинки
                for (int i = 0; i < SnowCount; i++)
                {
                    snowFlakes[i].Position.Y += snowFlakes[i].Speed * delta;
                    snowFlakes[i].Position.X += (float)Math.Sin(snowFlakes[i].Position.Y / SnowDepthDivider) * SnowParallaxMultiplier * delta;

                    // Если снежинка вышла за экран — переносим вверх
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

                // Рисуем фон
                spriteBatch.Draw(backgroundTexture,
                    new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                    Color.White);

                // Рисуем снежинки
                foreach (var flake in snowFlakes)
                {
                    spriteBatch.Draw(snowFlakeTexture, new Rectangle(
                        (int)flake.Position.X, (int)flake.Position.Y, sizeFlake, sizeFlake), Color.White);
                }
                spriteBatch.End();
                base.Draw(gameTime);
        }
            }
    }


