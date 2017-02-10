using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    /*Credit for the button class before modifications:
     * 
     *https://www.youtube.com/watch?v=54L_w0PiRa8
     *
    //*/

    class clsButton
    {
        InputHelper inputHelper = new InputHelper();
        Vector2 position;
        public Texture2D texture;
        Rectangle rectangle;

        Color color = new Color(0, 0, 0, 0);

        public Vector2 size;


        public clsButton(Texture2D newTexture, Vector2 size)//, GraphicsDevice graphics)
        {
            texture = newTexture;

            //ScreenWidth = 800, ScreenHeight = 600
            //ImageWidth  = 270, ImageHeight  = 40

            this.size = size;
        }

        bool down;
        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if(mouseRectangle.Intersects(rectangle))
            {
                if(color.A == 255)
                {
                    down = true; 
                }
                if (color.A == 0)
                {
                    down = false;
                }
                if(down)
                {
                    color = new Color(185, 0, 255, 255);
                }
                else
                {
                    color = new Color(185, 0, 255, 255);

                    //color.A += 5;
                }

                if (mouse.LeftButton == ButtonState.Pressed && inputHelper.CurrentMouseState != inputHelper.LastMouseState)
                {
                    isClicked = true;
                }
            }
            else if(color.A > 0)
            {
                color = new Color(0, 0, 0, 0);
                isClicked = false;
            }
            inputHelper.Update();
        }
        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
