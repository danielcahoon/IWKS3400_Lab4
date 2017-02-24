using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    class PongGame
    {
        public bool gameActive = false;
        public int winningScore;
        public clsPlayer P1;
        public clsPlayer P2;
        public clsSprite topBarrier;
        public clsSprite botBarrier;
        clsSprite gameBall;
        public clsPowerUp ballSpeedUp;
        public clsPowerUp ballSpeedDown;
        public clsPowerUp barrierSpeedUp;
        public clsPowerUp barrierSpeedDown;
        Random randPowerUp = new Random();

        public enum PlayerType
        {
            PlayerOne,
            PlayerTwo,
            CPU
        }

        // Sound Effect Initialization
        AudioEngine audioEngine;
        SoundBank soundsBank;
        WaveBank ballHit, score;
        Cue ballCue, scoreCue;

        //Graphics
        
        public static Rectangle destAsh;
        public static Rectangle destGary;
        
        /// <summary>
        /// Initializes Games based off of Game Type
        /// </summary>
        /// <param name="playerOneTexture">Player One Paddle</param>
        /// <param name="playerTwoTexture">Player Two Paddle</param>
        /// <param name="barrierTexture">Barrier Paddle</param>
        /// <param name="gameBallTexture">Gameball</param>
        /// <param name="ballSpeedUpTexure">PowerUp_BallSpeedInc</param>
        /// <param name="ballSpeedDownTexture">PowerUp_BallSpeedDec</param>
        /// <param name="barrierSpeedUpTexture">PowerUp_BarrierSpeedInc</param>
        /// <param name="barrierSpeedDownTexture">PowerUp_BarrierSpeedDec</param>
        /// <param name="graphics">Used for getting screen width and height</param>
        /// <param name="numOfUsers">Determines game type</param>
        public PongGame(Texture2D playerOneTexture, Texture2D playerTwoTexture, Texture2D barrierTexture,
            Texture2D gameBallTexture, Texture2D ballSpeedUpTexture, Texture2D ballSpeedDownTexture,
            Texture2D barrierSpeedUpTexture, Texture2D barrierSpeedDownTexture, GraphicsDeviceManager graphics, int numOfUsers)
        {
            // Sound Effects Initialization
            #region Sound Effects
            audioEngine = new AudioEngine("Content\\Lab4Sounds.xgs");
            ballHit = new WaveBank(audioEngine, "Content\\Sounds.xwb");
            score = new WaveBank(audioEngine, "Content\\Sounds.xwb");
            soundsBank = new SoundBank(audioEngine, "Content\\SoundsBank.xsb");
            ballCue = soundsBank.GetCue("BallHit");
            scoreCue = soundsBank.GetCue("Score");
            
            #endregion
            gameActive = true;
            winningScore = 5;
            #region Single Player
            if (numOfUsers == 1)
            {
                #region Paddles
                P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 51, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(51, 235), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P2 = new clsPlayer(playerTwoTexture, PlayerType.CPU, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(51, 235), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P1.paddle.velocity = new Vector2(0, 0);
                P2.paddle.velocity = new Vector2(0, -5);
                #endregion
                #region Barriers
                if (Game1.gameSettings.barriers == true)
                {
                    topBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 25.5f, 0), new Vector2(51, 167), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    topBarrier.velocity = new Vector2(0f, 5f);
                    botBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 25.5f, graphics.PreferredBackBufferHeight - 167), new Vector2(51, 167), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    botBarrier.velocity = new Vector2(0f, -5f);
                }
                #endregion
                #region Game Ball
                gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 18, (graphics.PreferredBackBufferHeight / 2) - 18), new Vector2(36, 36), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                gameBall.velocity = new Vector2(randPowerUp.Next(5, 7), randPowerUp.Next(-2, 3));
                if (gameBall.velocity.Y == 0)
                {
                    gameBall.velocity = new Vector2(gameBall.velocity.X, randPowerUp.Next(-3, -1));
                }
                #endregion
                #region PowerUps
                if (Game1.gameSettings.powerUps == true)
                {
                    ballSpeedUp = new clsPowerUp(ballSpeedUpTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(36, 36), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BallSpeedUp);
                    ballSpeedDown = new clsPowerUp(ballSpeedDownTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(36, 36), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BallSpeedDown);
                    barrierSpeedUp = new clsPowerUp(barrierSpeedUpTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(87, 78), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BarrierSpeedUp);
                    barrierSpeedDown = new clsPowerUp(barrierSpeedDownTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(72, 63), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BarrierSpeedDown);
                    if (Game1.gameSettings.barriers)
                    {
                        barrierSpeedDown.active = true;
                        barrierSpeedUp.active = true;
                    }
                    ballSpeedUp.active = true;
                    ballSpeedDown.active = true;
                }
                #endregion
            }
            #endregion
            #region Two Player
            else
            {
                #region Paddles
                P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 51, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(51, 235), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P2 = new clsPlayer(playerTwoTexture, PlayerType.PlayerTwo, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(51, 235), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P1.paddle.velocity = new Vector2(0, 0);
                P2.paddle.velocity = new Vector2(0, 0);
                #endregion
                #region Barriers
                if (Game1.gameSettings.barriers == true)
                {
                    topBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 25.5f, 0), new Vector2(51, 167), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    topBarrier.velocity = new Vector2(0f, 5f);
                    botBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 25.5f, graphics.PreferredBackBufferHeight - 167), new Vector2(51, 167), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    botBarrier.velocity = new Vector2(0f, -5f);
                }
                #endregion
                #region Game Ball
                gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 18, (graphics.PreferredBackBufferHeight / 2) - 18), new Vector2(36, 36), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                gameBall.velocity = new Vector2(randPowerUp.Next(5, 7), randPowerUp.Next(-2, 3));
                if (gameBall.velocity.Y == 0)
                {
                    gameBall.velocity = new Vector2(gameBall.velocity.X, randPowerUp.Next(-3, -1));
                }
                #endregion
                #region PowerUps
                if (Game1.gameSettings.powerUps == true)
                {
                    ballSpeedUp = new clsPowerUp(ballSpeedUpTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(36, 36), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BallSpeedUp);
                    ballSpeedDown = new clsPowerUp(ballSpeedDownTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(36, 36), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BallSpeedUp);
                    barrierSpeedUp = new clsPowerUp(barrierSpeedUpTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(87, 78), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BallSpeedUp);
                    barrierSpeedDown = new clsPowerUp(barrierSpeedDownTexture, new Vector2(graphics.PreferredBackBufferWidth + 200, graphics.PreferredBackBufferHeight + 200), new Vector2(72, 63), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, clsPowerUp.PowerUpType.BallSpeedUp);

                    if (Game1.gameSettings.barriers)
                    {
                        barrierSpeedDown.active = true;
                        barrierSpeedDown.active = true;
                    }
                    ballSpeedUp.active = true;
                    ballSpeedDown.active = true;
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool Update(KeyboardState keyboardState, GameTime gameTime)
        {
            #region Barrier movement
            if (Game1.gameSettings.barriers == false)
            {
                botBarrier.position = new Vector2(topBarrier.position.X, -topBarrier.size.Y);
                topBarrier.position = new Vector2(topBarrier.position.X, Game1.graphics.PreferredBackBufferHeight + 10);
            }
            else
            {
                topBarrier.Move();
                botBarrier.Move();
                if (topBarrier.Collides(botBarrier))
                {
                    topBarrier.velocity *= -1;
                    botBarrier.velocity *= -1;
                }
            }
            #endregion
            #region Paddle and Ball Movement
            P1.move(gameBall, keyboardState, gameTime);
            P1.paddle.withinScreen();
            P2.move(gameBall, keyboardState, gameTime);
            P2.paddle.withinScreen();
            gameBall.Move();
            gameBall.withinScreen();
            #endregion
            #region PowerUps
            if (Game1.gameSettings.powerUps == true)
            {
                //Load location of powerups into game if the power up is active
                if (ballSpeedUp.active)
                {
                    ballSpeedUp.position = new Vector2(312, 178);
                }
                else
                {
                    ballSpeedUp.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);
                }
                if (ballSpeedDown.active)
                {
                    //Make it so that the X coordinates match on right side of gym
                    ballSpeedDown.position = new Vector2(936 - (barrierSpeedUp.size.X / 2), 445);
                }
                else
                {
                    ballSpeedDown.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);
                }
                if (barrierSpeedUp.active)
                {
                    barrierSpeedUp.position = new Vector2(936 - (barrierSpeedUp.size.X / 2), 178);
                }
                else
                {
                    barrierSpeedUp.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);
                }
                if (barrierSpeedDown.active)
                {
                    barrierSpeedDown.position = new Vector2(312, 445);
                }
                else
                {
                    barrierSpeedDown.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);
                }
            }

            //Green Pokeball
            if (ballSpeedUp.CircleCollides(gameBall))
            {
                ballSpeedUp.speedBall(gameBall);
                ballSpeedUp.active = false;
                ballSpeedUp.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);

            }
            //Yellow Pokeball
            if (ballSpeedDown.CircleCollides(gameBall))
            {
                ballSpeedDown.slowBall(gameBall);
                ballSpeedDown.active = false;
                ballSpeedDown.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);

            }
            //Ponyta
            if (barrierSpeedUp.CircleCollides(gameBall))
            {
                barrierSpeedUp.speedBarrier(topBarrier);
                barrierSpeedUp.speedBarrier(botBarrier);
                barrierSpeedUp.active = false;
                barrierSpeedUp.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);
            }
            //Snorlax
            if (barrierSpeedDown.CircleCollides(gameBall))
            {
                barrierSpeedDown.slowBarrier(topBarrier);
                barrierSpeedDown.slowBarrier(botBarrier);
                barrierSpeedDown.active = false;
                barrierSpeedDown.position = new Vector2(Game1.graphics.PreferredBackBufferWidth + 200, Game1.graphics.PreferredBackBufferHeight + 200);
            }

            #endregion
            #region Scoring
            if (gameBall.position.X < P2.paddle.size.X - gameBall.size.X)
            {
                scoreCue = soundsBank.GetCue("Score");
                scoreCue.Play();
                P1.scorePoint();
                //Reset the ball's position to the center after each point
                gameBall.Reset();
                botBarrier.Reset();
                topBarrier.Reset();
                //Ball gets new random velocity each time a point is scored
                gameBall.velocity = new Vector2(randPowerUp.Next(5, 7), randPowerUp.Next(-2, 3));
                if (gameBall.velocity.Y == 0)
                {
                    gameBall.velocity = new Vector2(gameBall.velocity.X, randPowerUp.Next(-3, -1));
                }
            }
            if (gameBall.position.X > Game1.graphics.PreferredBackBufferWidth - P1.paddle.size.X)
            {
                scoreCue = soundsBank.GetCue("Score");
                scoreCue.Play();
                P2.scorePoint();
                //Reset the ball's position to the center after each point
                gameBall.Reset();
                botBarrier.Reset();
                topBarrier.Reset();
                //Ball gets new random velocity each time a point is scored
                gameBall.velocity = new Vector2(randPowerUp.Next(5, 7), randPowerUp.Next(-2, 3));
                if (gameBall.velocity.Y == 0)
                {
                    gameBall.velocity = new Vector2(gameBall.velocity.X, randPowerUp.Next(-3, -1));
                }
            }
            #endregion
            #region Game Ball Collision
            if (gameBall.CircleCollidesPaddle(P1.paddle))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity = new Vector2(-gameBall.velocity.X, randPowerUp.Next(-2, 3));
                if (gameBall.velocity.Y == 0)
                {
                    gameBall.velocity = new Vector2(gameBall.velocity.X, randPowerUp.Next(-3, -1));
                }
                gameBall.Move();
                if (P1.paddle.Collides(gameBall))
                {
                    gameBall.position -= gameBall.velocity;
                    gameBall.velocity = new Vector2(-gameBall.velocity.X, gameBall.velocity.X);
                }
            }
            if (gameBall.CircleCollidesPaddle(P2.paddle))
            {

                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity = new Vector2(-gameBall.velocity.X, randPowerUp.Next(-2, 3));
                if (gameBall.velocity.Y == 0)
                {
                    gameBall.velocity = new Vector2(gameBall.velocity.X, randPowerUp.Next(-3, -1));
                }
                gameBall.Move();
                if(P2.paddle.Collides(gameBall))
                {
                    gameBall.position -= gameBall.velocity;
                    gameBall.velocity = new Vector2(-gameBall.velocity.X, gameBall.velocity.X);
                }
            }
            if (gameBall.CircleCollidesPaddle(botBarrier))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
                topBarrier.velocity *= -1;
                botBarrier.velocity *= -1;
                
                //if (gameBall.center.X <= botBarrier.position.X && gameBall.center.X >= botBarrier.position.X + botBarrier.size.X && gameBall.position.Y >= botBarrier.position.Y + gameBall.size.Y)
                //{
                //    topBarrier.velocity = new Vector2(0, 5f);
                //    botBarrier.velocity = new Vector2(0, -5f);
                //    gameBall.velocity = new Vector2(gameBall.velocity.X, -gameBall.velocity.Y);
                //}
                //else if (gameBall.center.X <= botBarrier.position.X && gameBall.center.X >= botBarrier.position.X + botBarrier.size.X && gameBall.position.Y <= botBarrier.position.Y + botBarrier.size.Y)
                //{
                //    topBarrier.velocity = new Vector2(0, -5f);
                //    botBarrier.velocity = new Vector2(0, 5f);
                //    gameBall.velocity = new Vector2(gameBall.velocity.X, -gameBall.velocity.Y);
                //}
                //else if (gameBall.position.X == botBarrier.position.X)
                //{
                //    gameBall.velocity *= -1;
                //}
                //else if (gameBall.position.X == botBarrier.position.X + botBarrier.size.X)
                //{
                //    gameBall.velocity *= -1;
                //}
            }
            if (gameBall.CircleCollidesPaddle(topBarrier))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
                topBarrier.velocity *= -1;
                botBarrier.velocity *= -1;
                //if (gameBall.center.X <= topBarrier.position.X && gameBall.center.X >= topBarrier.position.X + topBarrier.size.X && gameBall.position.Y <= topBarrier.position.Y + topBarrier.size.Y)
                //{
                //    topBarrier.velocity = new Vector2(0f, 5f);
                //    botBarrier.velocity = new Vector2(0f, -5f);
                //    gameBall.velocity = new Vector2(gameBall.velocity.X, -gameBall.velocity.Y);
                //}
                //else if (gameBall.center.X <= topBarrier.position.X && gameBall.center.X >= topBarrier.position.X + topBarrier.size.X && gameBall.position.Y >= topBarrier.position.Y + gameBall.size.Y)
                //{
                //    topBarrier.velocity = new Vector2(0f, -5f);
                //    botBarrier.velocity = new Vector2(0f, 5f);
                //    gameBall.velocity = new Vector2(gameBall.velocity.X, -gameBall.velocity.Y);
                //}
                //else if (gameBall.position.X >= topBarrier.position.X)
                //{
                //    gameBall.velocity *= -1;
                //}
                //else if (gameBall.position.X <= topBarrier.position.X + topBarrier.size.X)
                //{
                //    gameBall.velocity *= -1;
                //}
            }
            #endregion
            if (P1.score == winningScore || P2.score == winningScore)
            {
                gameActive = false;
            }
            audioEngine.Update();
            destAsh = new Rectangle((int)P1.paddle.position.X, (int)P1.paddle.position.Y, 51, 235); // updates position of Ash
            destGary = new Rectangle((int)P2.paddle.position.X, (int)P2.paddle.position.Y, 51, 235); // updates position of Gary

            return (P1.score == winningScore || P2.score == winningScore);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            P1.Reset();
            P2.Reset();
            botBarrier.Reset();
            botBarrier.velocity = new Vector2(0, -5);
            topBarrier.Reset();
            topBarrier.velocity = new Vector2(0, 5);
            gameBall.Reset();
            gameBall.velocity = new Vector2(-gameBall.velocity.X, randPowerUp.Next(-2, 3));
            if (gameBall.velocity.Y == 0)
            {
                gameBall.velocity = new Vector2(gameBall.velocity.X, randPowerUp.Next(-3, -1));
            }
            ballSpeedUp.Reset();
            ballSpeedDown.Reset();
            if (Game1.gameSettings.barriers)
            {
                barrierSpeedUp.Reset();
                barrierSpeedDown.Reset();
            }
            gameActive = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            #region Paddles
            spriteBatch.Draw(P1.paddle.texture, destAsh, clsPlayer.sourceAsh, Color.White);
            spriteBatch.Draw(P2.paddle.texture, destGary, clsPlayer.sourceGary, Color.White);
            #endregion
            #region Barriers
            if (Game1.gameSettings.barriers == true)
            {
                botBarrier.Draw(spriteBatch);
                topBarrier.Draw(spriteBatch);
            }
            #endregion
            #region Game Ball
            gameBall.Draw(spriteBatch);
            #endregion
            #region PowerUps
            if (Game1.gameSettings.powerUps == true)
            {
                if (ballSpeedUp.active)
                {
                    ballSpeedUp.Draw(spriteBatch);
                }
                if (ballSpeedDown.active)
                {
                    ballSpeedDown.Draw(spriteBatch);
                }
                if (barrierSpeedUp.active && Game1.gameSettings.barriers)
                {
                    barrierSpeedUp.Draw(spriteBatch);
                }
                if (barrierSpeedDown.active && Game1.gameSettings.barriers)
                {
                    barrierSpeedDown.Draw(spriteBatch);
                }
            }
            #endregion
        }
    }
}
