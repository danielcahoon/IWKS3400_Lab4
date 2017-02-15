using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lab4
{
    class clsSprite
    {
        public Texture2D texture { get; set; } //sprite texture, read-only property
        public Vector2 position { get; set; } //sprite position on screen
        public Vector2 size { get; set; } //sprite size in pixels
        
        public Vector2 velocity { get; set; } //sprite velocity
        private Vector2 screenSize { get; set; } // screen size
        public Vector2 center { get { return position + (size / 2); } set { } } //sprite center
        public float radius { get { return size.X / 2; } } //sprite radius
        
        public Vector2 startingPosition { get; set; } //original location for the sprite
        //clsSprite Constructor
        public clsSprite(Texture2D newTexture, Vector2 newPosition, Vector2 newSize,
                         int ScreenWidth, int ScreenHeight)
        { 
            texture = newTexture;
            position = newPosition;
            size = newSize;
            startingPosition = newPosition;

            screenSize = new Vector2(ScreenWidth, ScreenHeight);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
        //method for moving the ball across the screen
        public void Move()
        {
            //if we'll move out of the screen, invert velocity in the X direction
            /*//checking right boundary
            if (position.X + size.X + velocity.X > screenSize.X)
            {
                velocity = new Vector2(-velocity.X, velocity.Y);
            }*/
            //checking bottom boundary
            if (position.Y + size.Y + velocity.Y > screenSize.Y)
            {
                velocity = new Vector2(velocity.X, -velocity.Y);
            }
            /*//checknig left boundary
            if (position.X + velocity.X < 0)
            {
                velocity = new Vector2(-velocity.X, velocity.Y);
            }//*/
            //checking top boundary
            if (position.Y + velocity.Y < 0)
            {
                velocity = new Vector2(velocity.X, -velocity.Y);
            }

            //since we adjusted the velocity, just add it to the current position
            position += velocity;
            #region mySprite1 Velocity Check
            while (velocity.X > 10 || velocity.Y > 10 || velocity.X < -10 || velocity.Y < -10)
            {
                //Console.WriteLine("X = {0}, Y = {1}", mySprite1.velocity.X, mySprite1.velocity.Y);
                if (velocity.X > 10)
                {
                    velocity = new Vector2(velocity.X - 10, velocity.Y);
                }
                if (velocity.Y > 10)
                {
                    velocity = new Vector2(velocity.X, velocity.Y - 10);
                }
                if (velocity.X < -10)
                {
                    velocity = new Vector2(velocity.X + 10, velocity.Y);
                }
                if (velocity.Y < -10)
                {
                    velocity = new Vector2(velocity.X, velocity.Y + 10);
                }
                #endregion


            }
            
        }

        //method for checking if the ball is off the screen from manual movement
        public void withinScreen()
        {
            //checking right boundary
            if (position.X + size.X > screenSize.X)
            {
                position = new Vector2(screenSize.X - size.X, position.Y);
            }
            //checking bottom boundary
            if (position.Y + size.Y > screenSize.Y)
            {
                position = new Vector2(position.X, screenSize.Y - size.Y);
            }
            //checknig left boundary
            if (position.X < 0)
            {
                position = new Vector2(0, position.Y);
            }
            //checking top boundary
            if (position.Y < 0)
            {
                position = new Vector2(position.X, 0);
            }
        }
        public bool CircleCollides(clsSprite otherSprite)
        {
            //check if two circles sprite collided
            //if (Vector2.Distance(this.center, otherSprite.center) <= this.radius + otherSprite.radius)
            //{
            //    this.position = new Vector2(otherSprite.position.X + (2 * otherSprite.radius), otherSprite.position.Y + (2 * otherSprite.radius));
            //}
            return (Vector2.Distance(this.center, otherSprite.center) < this.radius + otherSprite.radius);
        }
        public bool Collides(clsSprite otherSprite)
        {
            //check if two sprites intersect
            return (this.position.X + this.size.X > otherSprite.position.X &&
                    this.position.X < otherSprite.position.X + otherSprite.size.X &&
                    this.position.Y + this.size.Y > otherSprite.position.Y &&
                    this.position.Y < otherSprite.position.Y + otherSprite.size.Y);
        }
        public void blockBall(Vector2 position)
        {

        }
        
        public void Reset()
        {
            position = startingPosition;
        }
    }
}
