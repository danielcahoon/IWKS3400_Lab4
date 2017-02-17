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
    class PongGame : pongSettings
    {
        public bool gameActive = false;
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

        public Random RandPowerUp
        {
            get
            {
                return randPowerUp;
            }

            set
            {
                randPowerUp = value;
            }
        }


        public enum PlayerType
        {
            PlayerOne,
            PlayerTwo,
            CPU
        }

        // Sound Effect Stuff
        AudioEngine audioEngine;
        SoundBank soundsBank;
        WaveBank ballHit, gameWin, score;
        Cue ballCue, winCue, scoreCue;



        public PongGame(Texture2D playerOneTexture, Texture2D playerTwoTexture, Texture2D barrierTexture,
            Texture2D gameBallTexture, Texture2D ballSpeedUpTexure, Texture2D ballSpeedDownTexture,
            Texture2D barrierSpeedUpTexture, Texture2D barrierSpeedDownTexture, GraphicsDeviceManager graphics, int numOfUsers)
        {
            // Sound Effects Stuff
            audioEngine = new AudioEngine("Content\\Lab4Sounds.xgs");
            ballHit = new WaveBank(audioEngine, "Content\\Sounds.xwb");
            score = new WaveBank(audioEngine, "Content\\Sounds.xwb");
            gameWin = new WaveBank(audioEngine, "Content\\Sounds.xwb");
            soundsBank = new SoundBank(audioEngine, "Content\\SoundsBank.xsb");
            ballCue = soundsBank.GetCue("BallHit");
            scoreCue = soundsBank.GetCue("Score");
            winCue = soundsBank.GetCue("GameWin");
            audioEngine.Update();

            gameActive = true;
            if (Game1.gameSettings.barriers == true)
            {
                if (numOfUsers == 1)
                {
                    P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 40, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P2 = new clsPlayer(playerOneTexture, PlayerType.CPU, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P1.paddle.velocity = new Vector2(0, 0);
                    P2.paddle.velocity = new Vector2(0, 0);

                    topBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 20, 0), new Vector2(40, 150), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    topBarrier.velocity = new Vector2(0, 2);
                    botBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 20, graphics.PreferredBackBufferHeight - 150), new Vector2(40, 150), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    botBarrier.velocity = new Vector2(0, -2);

                    gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 32, (graphics.PreferredBackBufferHeight / 2) - 32), new Vector2(64, 64), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    gameBall.velocity = new Vector2(-5, -5);

                    ballSpeedUp = new clsPowerUp(ballSpeedUpTexure, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    ballSpeedDown = new clsPowerUp(ballSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedUp = new clsPowerUp(barrierSpeedUpTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedDown = new clsPowerUp(barrierSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);


                }
                else
                {
                    P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 60, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P2 = new clsPlayer(playerOneTexture, PlayerType.PlayerTwo, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P1.paddle.velocity = new Vector2(0, 0);
                    P2.paddle.velocity = new Vector2(0, 5);

                    topBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 20, 0), new Vector2(40, 150), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    topBarrier.velocity = new Vector2(0, 2);
                    botBarrier = new clsSprite(barrierTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 20, graphics.PreferredBackBufferHeight - 150), new Vector2(40, 150), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    botBarrier.velocity = new Vector2(0, -2);

                    gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 32, (graphics.PreferredBackBufferHeight / 2) - 32), new Vector2(64, 64), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    gameBall.velocity = new Vector2(-2, -2);

                    ballSpeedUp = new clsPowerUp(ballSpeedUpTexure, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    ballSpeedDown = new clsPowerUp(ballSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedUp = new clsPowerUp(barrierSpeedUpTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedDown = new clsPowerUp(barrierSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

                }
            }
            else
            {
                if (numOfUsers == 1)
                {
                    P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 40, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P2 = new clsPlayer(playerOneTexture, PlayerType.CPU, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P1.paddle.velocity = new Vector2(0, 0);
                    P2.paddle.velocity = new Vector2(0, 0);
                    gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 32, (graphics.PreferredBackBufferHeight / 2) - 32), new Vector2(64, 64), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    gameBall.velocity = new Vector2(-5, -5);

                    ballSpeedUp = new clsPowerUp(ballSpeedUpTexure, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    ballSpeedDown = new clsPowerUp(ballSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedUp = new clsPowerUp(barrierSpeedUpTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedDown = new clsPowerUp(barrierSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

                }
                else
                {
                    P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 60, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P2 = new clsPlayer(playerOneTexture, PlayerType.PlayerTwo, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                    P1.paddle.velocity = new Vector2(0, 0);
                    P2.paddle.velocity = new Vector2(0, 5);
                    gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 32, (graphics.PreferredBackBufferHeight / 2) - 32), new Vector2(64, 64), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    gameBall.velocity = new Vector2(-2, -2);

                    ballSpeedUp = new clsPowerUp(ballSpeedUpTexure, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    ballSpeedDown = new clsPowerUp(ballSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedUp = new clsPowerUp(barrierSpeedUpTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    barrierSpeedDown = new clsPowerUp(barrierSpeedDownTexture, randPowerUp.Next(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth), new Vector2(32, 32), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

                }
            }
        }

        public bool Update(KeyboardState keyboardState)
        {
            if (Game1.gameSettings.barriers == false)
            {
                botBarrier.position = new Vector2(topBarrier.position.X, -150);
                topBarrier.position = new Vector2(topBarrier.position.X, Game1.graphics.PreferredBackBufferHeight);
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
            P1.move(gameBall, keyboardState);
            P1.paddle.withinScreen();
            P2.move(gameBall, keyboardState);
            P2.paddle.withinScreen();
            gameBall.Move();
            gameBall.withinScreen();
            
            if (Game1.gameSettings.powerUps == false)
            {
                ballSpeedUp.position = new Vector2(Game1.graphics.PreferredBackBufferHeight + 150, Game1.graphics.PreferredBackBufferWidth + 150);
                ballSpeedDown.position = new Vector2(Game1.graphics.PreferredBackBufferHeight + 150, Game1.graphics.PreferredBackBufferWidth + 150);
                barrierSpeedUp.position = new Vector2(Game1.graphics.PreferredBackBufferHeight + 150, Game1.graphics.PreferredBackBufferWidth + 150);
                barrierSpeedDown.position = new Vector2(Game1.graphics.PreferredBackBufferHeight + 150, Game1.graphics.PreferredBackBufferWidth + 150);
            }

            if (gameBall.position.X < P2.paddle.size.X / 2)
            {
                scoreCue = soundsBank.GetCue("Score");
                scoreCue.Play();
                P1.scorePoint();
                Console.WriteLine("P1 Score: {0}", P1.score);
                Console.WriteLine("P2 Score: {0}", P2.score);
                gameBall.Reset();
            }
            if (gameBall.position.X > 642)
            {
                scoreCue = soundsBank.GetCue("Score");
                scoreCue.Play();
                P2.scorePoint();
                Console.WriteLine("P1 Score: {0}", P1.score);
                Console.WriteLine("P2 Score: {0}", P2.score);
                gameBall.Reset();
            }
            if (P1.paddle.Collides(gameBall))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
            }
            if (P2.paddle.Collides(gameBall))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
            }
            if (botBarrier.Collides(gameBall))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
            }
            if (topBarrier.Collides(gameBall))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
            }
            if (P1.score == 10 || P2.score == 10)
            {

                winCue = soundsBank.GetCue("GameWin");
                winCue.Play();
                gameActive = false;
            }
            audioEngine.Update();
            return (P1.score == 10 || P2.score == 10);
        }
        public void Reset()
        {
            P1.Reset();
            P2.Reset();
            gameBall.Reset();
            gameActive = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            P1.Draw(spriteBatch);
            P2.Draw(spriteBatch);
            botBarrier.Draw(spriteBatch);
            topBarrier.Draw(spriteBatch);
            gameBall.Draw(spriteBatch);
            ballSpeedUp.Draw(spriteBatch);
            ballSpeedDown.Draw(spriteBatch);
            barrierSpeedUp.Draw(spriteBatch);
            barrierSpeedDown.Draw(spriteBatch);
        }
    }
}
