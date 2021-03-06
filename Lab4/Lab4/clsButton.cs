﻿using Microsoft.Xna.Framework;
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
        Vector2 position;
        public Texture2D texture;
        Rectangle rectangle;
        
        //On/Off kind of button
        bool toggleColorType;
        bool buttonOn;

        Color color = new Color(0, 0, 0, 0);

        public Vector2 size;


        public clsButton(Texture2D newTexture, Vector2 size, bool toggle, bool colored)
        {
            texture = newTexture;
            toggleColorType = toggle;
            buttonOn = colored;
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
                if (toggleColorType)
                {
                    if (mouse.LeftButton == ButtonState.Pressed && buttonOn == false)
                    {
                        isClicked = true;
                        color = new Color(185, 0, 255, 255);
                        buttonOn = true;
                    }
                }
                else
                {

                    if (color.A == 255)
                    {
                        down = true;
                    }
                    if (color.A == 0)
                    {
                        down = false;
                    }
                    if (down)
                    {
                        color = new Color(185, 0, 255, 255);
                    }
                    else
                    {
                        color = new Color(185, 0, 255, 255);

                        //color.A += 5;
                    }

                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        isClicked = true;
                    }
                }
            }
            else if(color.A > 0 && toggleColorType == false)
            {
                color = new Color(0, 0, 0, 0);
                isClicked = false;
            }
            else
            {
                isClicked = false;
            }
        }
        public void removeColor()
        {
            color = new Color(0, 0, 0, 0);
            buttonOn = false;
        }
        public void addColor()
        {
            color = new Color(185, 0, 255, 255);
            buttonOn = true;
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
