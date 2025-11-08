using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace FNAScreenSaver
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;

            Texture2D backgroundTexture;
            Texture2D snowFlakeTexture;

            struct SnowFlake
            {
                public Vector2 Position;
                public float Speed;
                public float Scale;
            }

            SnowFlake[] snowFlakes;
            Random random = new Random();

            const int SnowCount = 1200; // можно менять 1000-1500
        const int sizeFlake = 50;

            public Game1()
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
                    snowFlakes[i].Scale = 0.3f + (float)random.NextDouble() * 0.7f;
                    // скорость — зависит от масштаба
                    snowFlakes[i].Speed = 30f + snowFlakes[i].Scale * 100f;
                }
            }
            protected override void Update(GameTime gameTime)
            {
                // Завершение по нажатию клавиши
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    Exit();
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                // Двигаем снежинки
                for (int i = 0; i < SnowCount; i++)
                {
                    snowFlakes[i].Position.Y += snowFlakes[i].Speed * delta;
                    snowFlakes[i].Position.X += (float)Math.Sin(snowFlakes[i].Position.Y / 50f) * 20f * delta;

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


