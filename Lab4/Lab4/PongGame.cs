using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    class PongGame
    {
        public bool gameActive = false;
        clsPlayer P1;
        clsPlayer P2;
        clsSprite gameBall;
        public pongSettings settings;
        public enum PlayerType
        {
            PlayerOne,
            PlayerTwo,
            CPU
        }
        public PongGame(Texture2D playerOneTexture, Texture2D playerTwoTexture, Texture2D gameBallTexture, GraphicsDeviceManager graphics)
        {
            gameActive = true;
            P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 60, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(60, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            P2 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(60, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 32, (graphics.PreferredBackBufferHeight / 2) - 32), new Vector2(64, 64),graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        public bool Update()
        {
            P1.move(gameBall);
            P2.move(gameBall);
            if(P1.score == 10 || P2.score == 10)
            {
                gameActive = false;
            }
            return (P1.score == 10 || P2.score == 10);
        }
    }
}
