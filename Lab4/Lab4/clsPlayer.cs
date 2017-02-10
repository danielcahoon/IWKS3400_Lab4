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
        int score;

        public clsPlayer(Texture2D texture, PongGame.PlayerType typeOfPlayer, Vector2 position, Vector2 newSize, Vector2 screenSize)
        {
            switch(playerType)
            {
                case PongGame.PlayerType.PlayerOne:
                    //position associated with right side
                    paddle = new clsSprite(texture, position, newSize, (int)screenSize.X, (int)screenSize.Y);
                    break;
            }
        }

        public void move()
        {

            if (movementKey.IsKeyDown(Keys.Up))
            {
                
            }
        }
    }
}
