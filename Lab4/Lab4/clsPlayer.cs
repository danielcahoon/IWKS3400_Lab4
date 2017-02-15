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

        public void move(clsSprite gameBall, KeyboardState movementKey)
        {
            //Console.WriteLine("{0}'s paddle position = {1}", playerType, paddle.position);
            if (playerType == PongGame.PlayerType.PlayerOne)
            {
                if (movementKey.IsKeyDown(Keys.Up))
                {
                    paddle.velocity = new Vector2(0, -5);
                    paddle.Move();
                }
                else if (movementKey.IsKeyDown(Keys.Down))
                {
                    paddle.velocity = new Vector2(0, 5);
                    paddle.Move();
                }
            }
            else if (playerType == PongGame.PlayerType.PlayerTwo)
            {
                if (movementKey.IsKeyDown(Keys.W))
                {
                    paddle.velocity = new Vector2(0, -5);
                    paddle.Move();
                }
                else if (movementKey.IsKeyDown(Keys.S))
                {
                    paddle.velocity = new Vector2(0, 5);
                    paddle.Move();
                }
            }
            else if (playerType == PongGame.PlayerType.CPU)
            {
                //Add code for difficulty here regarding when the 
                //paddle finds out where the ball is
                if (gameBall.position.X - paddle.position.X < 300 && gameBall.position.Y > (paddle.center.Y - paddle.size.Y / 2))
                {
                    paddle.velocity = new Vector2(0, 5);
                    paddle.Move();
                }
                else if (gameBall.position.X - paddle.position.X < 300 && gameBall.position.Y < (paddle.center.Y - paddle.size.Y / 2))
                {
                    paddle.velocity = new Vector2(0, -5);
                    paddle.Move();
                }
                else
                {
                    paddle.Move();
                }
                paddle.withinScreen();
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
