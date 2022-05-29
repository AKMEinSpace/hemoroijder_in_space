using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace hemoroijder_in_space
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D bg;
        Texture2D black_hole;
        Texture2D player_sprite;
        Texture2D asteroid_sprite;
        Vector2 new_origin;
        Vector2 black_hole_origin;
        Vector2 black_hole_center;
        Vector2 player_pos;
        Vector2 player_origin;
        float rotation_bh;
        float rotation_player;
        float angle = 0;
        float countdown;
        Rectangle BlackHoleRectangle;
        Rectangle PlayerRectangle;
        int player_life;
        int player_score;

        List<Vector2> asteroid_pos = new List<Vector2>();
        List<int> asteroids = new List<int>();
        List<float> asteroid_angle = new List<float>();

        Random angle_spawn = new Random();

        SpriteFont score_count;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player_life = 3;
            player_score = 0;
            int res = 1000;
            int black_hole_diameter = 300;
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = res;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = res;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

            new_origin.X = new_origin.Y = (res / 2 - black_hole_diameter/2);
            black_hole_center = new Vector2(black_hole_diameter / 2, black_hole_diameter/ 2);
            black_hole_origin = new Vector2(new_origin.X + 150, new_origin.Y + 150);




            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Console.WriteLine(Content.RootDirectory);
            // TODO: use this.Content to load your game content here
            bg = Content.Load<Texture2D>("bg1000");
            black_hole = Content.Load<Texture2D>("black_hole_v2");
            player_sprite = Content.Load<Texture2D>("player90v2");
            asteroid_sprite = Content.Load<Texture2D>("planet1");

            score_count = Content.Load<SpriteFont>("Text/score");
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || player_life < 1)
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                angle += (float)Math.PI / 150;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                angle -= (float)Math.PI / 150;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                rotation_player += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                rotation_player -= 0.1f;
            PlayerRectangle = new Rectangle((int)player_pos.X, (int)player_pos.Y, player_sprite.Width, player_sprite.Height);
            BlackHoleRectangle = new Rectangle((int)new_origin.X, (int)new_origin.Y, black_hole.Width, black_hole.Height) ;
            if (countdown > 0)
            {
                countdown -= (float)gameTime.TotalGameTime.TotalSeconds/10;
            }
            else if (asteroids.Count < 100) {
                asteroid_angle.Add((float)angle_spawn.NextDouble() * (float)(Math.PI * 2));
                asteroids.Add(0);
                asteroid_pos.Add(Vector2.Zero);
                countdown = 1000;

            }
            else
            {
                asteroids.Clear();
            }
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i] += 1;
                
            }


            bool collide = false;
            foreach(Vector2 pos in asteroid_pos)
            {
                Rectangle temp = new Rectangle((int)pos.X, (int)pos.Y, asteroid_sprite.Width, asteroid_sprite.Height);
                if (BlackHoleRectangle.Intersects(temp))
                {
                    player_life -= 1;
                    asteroids.Clear();
                    asteroid_angle.Clear();
                    collide = true;
                    break;
                }
                else if (PlayerRectangle.Intersects(temp))
                {
                    player_score += 1;
                    asteroids[asteroid_pos.IndexOf(pos)] = 0;
                }
            }
            if (collide == true)
            {
                asteroid_pos.Clear();
            }

            player_pos = new_origin + new Vector2(150 + (float)Math.Cos(angle) *250, 150+ (float)Math.Sin(angle)*250);
            player_origin = new Vector2(PlayerRectangle.Width / 2, PlayerRectangle.Height / 2);

            // TODO: Add your update logic here
            rotation_bh += 0.01f;
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroid_pos[i] = black_hole_origin + new Vector2((float)Math.Cos(asteroid_angle[i]) * (700 - asteroids[i] - (float)gameTime.TotalGameTime.TotalSeconds / 1000), (float)Math.Sin(asteroid_angle[i]) * (700 - asteroids[i] - (float)gameTime.TotalGameTime.TotalSeconds / 10));

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin();
            _spriteBatch.Draw(bg,Vector2.Zero, Color.White);
            _spriteBatch.Draw(black_hole,black_hole_origin, null, Color.White, rotation_bh, black_hole_center, 1f/player_life , SpriteEffects.None,0);
            _spriteBatch.Draw(player_sprite, player_pos, null, Color.White, rotation_player, player_origin, 1f, SpriteEffects.None, 0);
            for(int i = 0; i < asteroid_angle.Count;i++)
            {
                _spriteBatch.Draw(asteroid_sprite, asteroid_pos[i],null, Color.White, 0f, player_origin, 0.7f, SpriteEffects.None, 0);
            }
            _spriteBatch.DrawString(score_count, string.Format("Life: {0}",player_life), new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(score_count, String.Format("Score: {0}", player_score), new Vector2(10, 30), Color.White);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
