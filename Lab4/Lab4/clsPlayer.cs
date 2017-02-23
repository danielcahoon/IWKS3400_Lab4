using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Lab4
{
    class clsPlayer 
    {
        public clsSprite paddle;
        public static Rectangle sourceAsh;
        public static Rectangle sourceGary;

        int frames = 0;
        float delay = 50f; // Framerate of sprite animation
        float elapsed = 0f; // checks how many frames went by
        //KeyboardState movementKey = Keyboard.GetState();
        PongGame.PlayerType playerType;
        public int score;

        public clsPlayer(Texture2D texture, PongGame.PlayerType typeOfPlayer, Vector2 position, Vector2 newSize, Vector2 screenSize)
        {
            score = 0;
            playerType = typeOfPlayer;
            switch(playerType)
            {
                case PongGame.PlayerType.PlayerOne:
                    //position associated with right side
                    paddle = new clsSprite(texture, position, newSize, (int)screenSize.X, (int)screenSize.Y);
                    break;
                case PongGame.PlayerType.PlayerTwo:
                    paddle = new clsSprite(texture, position, newSize, (int)screenSize.X, (int)screenSize.Y);
                    break;
                case PongGame.PlayerType.CPU:
                    paddle = new clsSprite(texture, position, newSize, (int)screenSize.X, (int)screenSize.Y);
                    break;
            }
        }
        public void animateAsh(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= 3)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }
            sourceAsh = new Rectangle(51 * frames, 0, 51, 235);
        }
        public void animateGary(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= 3)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }
            sourceGary = new Rectangle(51 * frames, 0, 51, 235);
        }
        public void move(clsSprite gameBall, KeyboardState movementKey, GameTime gameTime)
        {            
            if (playerType == PongGame.PlayerType.PlayerOne)
            {
                if (movementKey.IsKeyDown(Keys.Up))
                {
                    this.animateAsh(gameTime);
                    paddle.velocity = new Vector2(0, -5);
                    paddle.Move();
                }
                else if (movementKey.IsKeyDown(Keys.Down))
                {
                    this.animateAsh(gameTime);
                    paddle.velocity = new Vector2(0, 5);
                    paddle.Move();
                }
            }
            else if (playerType == PongGame.PlayerType.PlayerTwo)
            {
                if (movementKey.IsKeyDown(Keys.W))
                {
                    this.animateGary(gameTime);
                    paddle.velocity = new Vector2(0, -5);
                    paddle.Move();
                }
                else if (movementKey.IsKeyDown(Keys.S))
                {
                    this.animateGary(gameTime);
                    paddle.velocity = new Vector2(0, 5);
                    paddle.Move();
                }
            }
            else if (playerType == PongGame.PlayerType.CPU)
            {
                //Add code for difficulty here regarding when the 
                //paddle finds out where the ball is
                if (Game1.gameSettings.difficulty == pongSettings.Difficulty.Easy)
                {
                    paddle.Move();
                    this.animateGary(gameTime);
                    paddle.withinScreen();
                }
                else if(Game1.gameSettings.difficulty == pongSettings.Difficulty.Medium)
                {
                    if (gameBall.position.X - paddle.position.X < 50 && gameBall.position.Y > (paddle.center.Y - paddle.size.Y / 2))
                    {
                        this.animateGary(gameTime);
                        paddle.velocity = new Vector2(0, 5);
                        paddle.Move();
                    }
                    else if (gameBall.position.X - paddle.position.X < 50 && gameBall.position.Y < (paddle.center.Y - paddle.size.Y / 2))
                    {
                        this.animateGary(gameTime);
                        paddle.velocity = new Vector2(0, -5);
                        paddle.Move();
                    }
                    else
                    {
                        this.animateGary(gameTime);
                        paddle.Move();
                    }
                    paddle.withinScreen();
                }
                else if(Game1.gameSettings.difficulty == pongSettings.Difficulty.Hard)
                {
                    if (gameBall.position.X - paddle.position.X < 300 && gameBall.position.Y > (paddle.center.Y - paddle.size.Y / 2))
                    {
                        this.animateGary(gameTime);
                        paddle.velocity = new Vector2(0, 5);
                        paddle.Move();
                    }
                    else if (gameBall.position.X - paddle.position.X < 300 && gameBall.position.Y < (paddle.center.Y - paddle.size.Y / 2))
                    {
                        this.animateGary(gameTime);
                        paddle.velocity = new Vector2(0, -5);
                        paddle.Move();
                    }
                    else
                    {
                        this.animateGary(gameTime);
                        paddle.Move();
                    }
                    paddle.withinScreen();
                }
            }
        }
        public void scorePoint()
        {
            ++score;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            paddle.Draw(spriteBatch);
        }
        public void Reset()
        {
            score = 0;
            paddle.Reset();
        }
    }
}
