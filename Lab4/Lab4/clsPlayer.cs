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
        KeyboardState movementKey = Keyboard.GetState();
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
            }
        }

        public void move(clsSprite gameBall)
        {
            if (playerType == PongGame.PlayerType.PlayerOne)
            {
                if (movementKey.IsKeyDown(Keys.Up))
                {
                    paddle.position = new Vector2(paddle.position.X, paddle.position.Y + 5);
                    paddle.withinScreen();
                }
                else if (movementKey.IsKeyDown(Keys.Down))
                {
                    paddle.position = new Vector2(paddle.position.X, paddle.position.Y - 5);
                    paddle.withinScreen();
                }
            }
            else if (playerType == PongGame.PlayerType.PlayerTwo)
            {
                if (movementKey.IsKeyDown(Keys.W))
                {
                    paddle.position = new Vector2(paddle.position.X, paddle.position.Y + 5);
                    paddle.withinScreen();
                }
                else if (movementKey.IsKeyDown(Keys.S))
                {
                    paddle.position = new Vector2(paddle.position.X, paddle.position.Y - 5);
                    paddle.withinScreen();
                }
            }
            else if (playerType == PongGame.PlayerType.CPU)
            {
                //Add code for difficulty here regarding when the 
                //paddle finds out where the ball is

                paddle.position = new Vector2(gameBall.position.X, gameBall.position.Y);
                paddle.withinScreen();
            }
        }
        public void scorePoint()
        {
            ++score;
        }
    }
}
