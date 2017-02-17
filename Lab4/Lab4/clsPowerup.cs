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
    class clsPowerup
    {
        public static enum PowerUpType
        {
            BallSpeedUp,
            BallSpeedDown,
            BarrierSpeedUp,
            BarrierSpeedDown
        }

        PowerUpType puType;



        public Texture2D texture { get; set; } //sprite texture, read-only property
        public Vector2 position { get; set; } //sprite position on screen
        public Vector2 size { get; set; } //sprite size in pixels

        public Vector2 velocity { get; set; } //sprite velocity
        private Vector2 screenSize { get; set; } // screen size
        public Vector2 center { get { return position + (size / 2); } set { } } //sprite center
        public float radius { get { return size.X / 2; } } //sprite radius

        public Vector2 startingPosition { get; set; } //original location for the sprite
        //clsSprite Constructor

        public clsPowerup(Texture2D newTexture, Vector2 newPosition, Vector2 newSize,
                         int ScreenWidth, int ScreenHeight, PowerUpType newType)
        {
            texture = newTexture;
            position = newPosition;
            size = newSize;
            startingPosition = newPosition;

            puType = newType;

            screenSize = new Vector2(ScreenWidth, ScreenHeight);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
        public bool CircleCollides(clsSprite otherSprite)
        {
            //check if two circles sprite collided
            if (Vector2.Distance(this.center, otherSprite.center) <= this.radius + otherSprite.radius)
            {
                this.position = new Vector2(otherSprite.position.X + (2 * otherSprite.radius), otherSprite.position.Y + (2 * otherSprite.radius));
            }
            return (Vector2.Distance(this.center, otherSprite.center) < this.radius + otherSprite.radius);
        }
        public void Reset()
        {
            position = startingPosition;
        }
    }
}
