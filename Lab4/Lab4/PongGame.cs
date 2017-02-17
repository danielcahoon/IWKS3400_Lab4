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
        public clsPlayer P1;
        public clsPlayer P2;
        clsSprite gameBall;
        public pongSettings settings;
        public enum PlayerType
        {
            PlayerOne,
            PlayerTwo,
            CPU
        }
        
        // Sound Effect Stuff
        AudioEngine audioEngine;
        SoundBank soundsBank;
        WaveBank ballHit, score, gameWin;
        Cue ballCue, scoreCue, winCue;

        public PongGame(Texture2D playerOneTexture, Texture2D playerTwoTexture, Texture2D gameBallTexture, GraphicsDeviceManager graphics, int numOfUsers)
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
            if (numOfUsers == 1)
            {
                P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 40, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P2 = new clsPlayer(playerOneTexture, PlayerType.CPU, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P1.paddle.velocity = new Vector2(0, 0);
                P2.paddle.velocity = new Vector2(0, 0);
                gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 32, (graphics.PreferredBackBufferHeight / 2) - 32), new Vector2(64, 64), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                gameBall.velocity = new Vector2(-5, -5);
            }
            else
            {
                P1 = new clsPlayer(playerOneTexture, PlayerType.PlayerOne, new Vector2(graphics.PreferredBackBufferWidth - 60, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P2 = new clsPlayer(playerOneTexture, PlayerType.PlayerTwo, new Vector2(0, (graphics.PreferredBackBufferHeight / 2) - 75), new Vector2(40, 150), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                P1.paddle.velocity = new Vector2(0, 0);
                P2.paddle.velocity = new Vector2(0, 5);
                gameBall = new clsSprite(gameBallTexture, new Vector2((graphics.PreferredBackBufferWidth / 2) - 32, (graphics.PreferredBackBufferHeight / 2) - 32), new Vector2(64, 64), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                gameBall.velocity = new Vector2(-2, -2);
            }
        }

        public bool Update(KeyboardState keyboardState)
        {
            P1.move(gameBall, keyboardState);
            P2.move(gameBall, keyboardState);
            gameBall.Move();
            if (gameBall.position.X < P2.paddle.size.X / 2)
            {
                P1.scorePoint();
                Console.WriteLine("P1 Score: {0}", P1.score);
                Console.WriteLine("P2 Score: {0}", P2.score);
                scoreCue = soundsBank.GetCue("Score");
                scoreCue.Play();
                gameBall.Reset();
            }
            if (gameBall.position.X > 642)
            {
                P2.scorePoint();
                Console.WriteLine("P1 Score: {0}", P1.score);
                Console.WriteLine("P2 Score: {0}", P2.score);
                scoreCue = soundsBank.GetCue("Score");
                scoreCue.Play();
                gameBall.Reset();
            }
            if (P1.paddle.Collides(gameBall))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
            }
            if(P2.paddle.Collides(gameBall))
            {
                ballCue = soundsBank.GetCue("BallHit");
                ballCue.Play();
                gameBall.velocity *= -1;
            }
            
            if(P1.score == 10 || P2.score == 10)
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
            gameBall.Draw(spriteBatch);
        }
    }
}
