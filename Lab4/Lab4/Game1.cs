using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lab4
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        //Font Stuff
        SpriteFont Font1;
        public string victory; //used to hold congratulations/blnt message

        //Sound and Music Stuff
        // Create a SoundEffect resource
        SoundEffect menuEffect, ballHit, gameWin, score;
        AudioEngine audioEngine;
        SoundBank soundsBank;
        WaveBank sounds;
        Cue soundsCue;

        WaveBank mainMenu, settings, credits, hardAI, medAI, easyAI, twoPlayer;
        Cue mainMenuCue, settingsCue, creditsCue, hardAICue, medAICue, easyAICue, twoPlayerCue;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public InputHelper inputHelper;
        #region Sprite Definitions

        #region Main Menu Sprites and Buttons
        //Sprites
        clsSprite MainMenuSprite;
        //Buttons
        clsButton mainMenu1P;
        clsButton mainMenu2P;
        clsButton mainMenuSettings;
        clsButton mainMenuCredits;
        clsButton mainMenuQuit;
        #endregion
        #region Settings Screen Sprite and Buttons
        //Sprites
        clsSprite settingsSprite; //Used as the not Changing background to overlay buttons on 
        //Buttons
        clsButton settingBarrierOn;
        clsButton settingBarrierOff;

        clsButton settingPowerUpsOn;
        clsButton settingPowerUpsOff;

        clsButton settingDifficulty1;
        clsButton settingDifficulty2;
        clsButton settingDifficulty3;

        clsButton settingMusicOn;
        clsButton settingMusicOff;

        clsButton settingExitButton;
        #endregion
        
        #region Pause Screen Sprite and Buttons
        //Sprites
        clsSprite pauseScreenSprite;
        //Buttons
        clsButton pauseExitButton;
        clsButton gameExitButton;
        #endregion

        #endregion

        PongGame P1Game;
        PongGame P2Game;
        bool gamePaused;
        enum GameState
        {
            MainMenu,
            InGame1P,
            InGame2P,
            Settings,
            Credits
        }
        GameState CurrentGameState = GameState.MainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;

            //Set window size
            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 500;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
            {
            //Font Stuff
            Font1 = Content.Load<SpriteFont>("Courier New");


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            inputHelper = new InputHelper();
            
            P1Game = new Lab4.PongGame(Content.Load<Texture2D>("Paddles"), Content.Load<Texture2D>("Paddles"), Content.Load<Texture2D>("ball2"), graphics, 1);
            
            P2Game = new Lab4.PongGame(Content.Load<Texture2D>("Paddles"), Content.Load<Texture2D>("Paddles"), Content.Load<Texture2D>("ball2"), graphics, 2);
            
            //Load 2D Content into the Sprites
            MainMenuSprite = new clsSprite(Content.Load<Texture2D>("MainMenu"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            #region Menu Buttons
            //Load 2D content into my MainMenuButton
            mainMenu1P = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(270, 40));
            mainMenu1P.setPosition(new Vector2(224, 116));
            mainMenu2P = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(233, 40));
            mainMenu2P.setPosition(new Vector2(224, 179));
            mainMenuSettings = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(169, 40));
            mainMenuSettings.setPosition(new Vector2(224, 243));
            mainMenuCredits = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(149, 40));
            mainMenuCredits.setPosition(new Vector2(224, 307));
            mainMenuQuit = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(230, 40));
            mainMenuQuit.setPosition(new Vector2(224, 371));
            #endregion

            settingsSprite = new clsSprite(Content.Load<Texture2D>("Settings"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            #region Setting Buttons
            //settingBarrierOn = new clsButton(Content.Load<Texture2D>(""))
            #endregion
           pauseScreenSprite = new clsSprite(Content.Load<Texture2D>("PauseScreen1"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            #region Pause Buttons
            gameExitButton = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(110, 19));
            gameExitButton.setPosition(new Vector2(578, 13));
            pauseExitButton = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2((672-581), 19));
            pauseExitButton.setPosition(new Vector2(581, 45));
            #endregion

            #region Audio Loading
            //Load the SoundEffect Resources
            menuEffect = Content.Load<SoundEffect>("MenuSoundEffect");
            ballHit = Content.Load<SoundEffect>("BallHit");
            score = Content.Load<SoundEffect>("Score");
            gameWin = Content.Load<SoundEffect>("GameWin");

            //Load File built from XACT project
            audioEngine = new AudioEngine("Content\\Lab4Sounds.xgs");
            sounds = new WaveBank(audioEngine, "Content\\Sounds.xwb");
            soundsBank = new SoundBank(audioEngine, "Content\\SoundsBank.xsb");

            //Load Streaming Wave Banks
            mainMenu = new WaveBank(audioEngine, "Content\\MainMenu.xwb", 0, 4);
            /*//Settings to Implement
             * settings = new WaveBank(audioEngine, "Content\\Settings.xwb", 0, 4);
             * 
             * //Credits to Implement
             * credits = new WaveBank(audioEngine, "Content\\Credits.xwb", 0, 4);
             * 
             //*/
            easyAI = new WaveBank(audioEngine, "Content\\EasyAI.xwb", 0, 4);
            /*//Other AI difficulties to implement
             * medAI = new WaveBank(audioEngine, "Content\\MediumAI.xwb", 0, 4);
             * hardAI = new WaveBank(audioEngine, "Content\\HardAI.xwb, 0, 4);
             * 
             * //2P game audio to implement
             * twoPlayer = new WaveBank(audioEngine, "Content\\2Player.xwb, 0, 4);
             * 
             //*/

            //Get Cues for Streaming music
            mainMenuCue = soundsBank.GetCue("MainMenuMusic");
            easyAICue = soundsBank.GetCue("EasyAI");
            /*//Getting cues for the other cues
             *  medAICue = soundsBank.GetCue("MediumAI");
             *  hardAICue = soundsBank.GetCue("HardAI");
             *  twoPlayerCue = soundsBank.GetCue("2Player");
             *  settingsCue = soundsBank.GetCue("SettingsMusic");
             *  creditsCue = soundsBank.GetCue("Credits");
             *  //*/

            /*//playing and pausing the cues for changing of game state
            settingsCue.Play();
            creditsCue.Play();
            twoPlayerCue.Play();
            //playing for the different difficulties
            */easyAICue.Play();
            //medAICue.Play();
            //hardAICue.Play();

            //Pausing all of the above cues for later use in GameState changing
            //Difficulty pasuing
            easyAICue.Pause();
            //medAI.Pause();
            //hardAI.Pause();

            /*
            settingsCue.Pause();
            creditsCue.Pause();
            twoPlayerCue.Pause();
            //*/

            //Play the MainMenu Theme by default
            mainMenuCue.Play();
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Main Menu Disposal
            MainMenuSprite.texture.Dispose();
            mainMenu1P.texture.Dispose();
            mainMenu2P.texture.Dispose();
            mainMenuSettings.texture.Dispose();
            mainMenuCredits.texture.Dispose();
            mainMenuQuit.texture.Dispose();

            settingsSprite.texture.Dispose();

           
            //Pause Screen Disposal
            pauseScreenSprite.texture.Dispose();
            pauseExitButton.texture.Dispose();
            gameExitButton.texture.Dispose();

            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // Allows the game to exit keyboard input
            KeyboardState exitInput = Keyboard.GetState();
            if (exitInput.IsKeyDown(Keys.D0))
            {
                this.Exit();
            }

            MouseState mouseState = Mouse.GetState();
            // TODO: Add your update logic here
            //       Change to a set of Switch statements using Enum GameState

            switch (CurrentGameState)
            {
                #region MainMenu Code
                case GameState.MainMenu:
                    mouseState = Mouse.GetState();
                    if (mainMenu1P.isClicked == true)
                    {
                        CurrentGameState = GameState.InGame1P;
                        P1Game.Reset();
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    mainMenu1P.Update(mouseState);

                    if (mainMenu2P.isClicked == true)
                    {
                        CurrentGameState = GameState.InGame2P;
                        P2Game.Reset();
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    mainMenu2P.Update(mouseState);

                    if (mainMenuSettings.isClicked == true)
                    {
                        CurrentGameState = GameState.Settings;
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    mainMenuSettings.Update(mouseState);

                    if (mainMenuCredits.isClicked == true)
                    {
                        CurrentGameState = GameState.Credits;
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    mainMenuCredits.Update(mouseState);

                    if (mainMenuQuit.isClicked == true)
                    {
                        this.Exit();
                    }
                    mainMenuQuit.Update(mouseState);

                    break;

                #endregion
                #region Single Player Code
                case GameState.InGame1P:
                    mouseState = Mouse.GetState();
                    
                    if (exitInput.IsKeyDown(Keys.P))
                    {
                        gamePaused = true;
                    }
                    if (!gamePaused)
                    {
                        if(P1Game.gameActive)
                        {
                            P1Game.Update(exitInput);
                        }
                        else
                        {
                            P1Game.Reset();
                            CurrentGameState = GameState.MainMenu;
                        }

                        //TODO Implement actual exit condition
                        if (exitInput.IsKeyDown(Keys.Space))
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    else
                    {
                        if (pauseExitButton.isClicked)
                        {
                            gamePaused = false;
                        }
                        //Exiting Game needs debugging from Pause Screen
                        else if (gameExitButton.isClicked)
                        {
                            if (mouseState.LeftButton == ButtonState.Released)
                            {
                                gamePaused = false;
                                CurrentGameState = GameState.MainMenu;
                            }
                        }
                        pauseExitButton.Update(mouseState);
                        gameExitButton.Update(mouseState);
                    }
                    break;
                #endregion
                #region Two Player Code
                case GameState.InGame2P:
                    mouseState = Mouse.GetState();

                    if (exitInput.IsKeyDown(Keys.P))
                    {
                        gamePaused = true;
                    }
                    if (!gamePaused)
                    {
                        if (P2Game.gameActive)
                        {
                            P2Game.Update(exitInput);
                        }
                        else
                        {
                            P2Game.Reset();
                            CurrentGameState = GameState.MainMenu;
                        }

                        //TODO Implement actual exit condition
                        if (exitInput.IsKeyDown(Keys.Space))
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    else
                    {
                        if (pauseExitButton.isClicked)
                        {
                            gamePaused = false;
                        }
                        //Exiting game needs debugging from Pause Screen
                        else if (gameExitButton.isClicked)
                        {
                            if (mouseState.LeftButton == ButtonState.Released)
                            {
                                gamePaused = false;
                                CurrentGameState = GameState.MainMenu;
                            }
                        }
                        pauseExitButton.Update(mouseState);
                        gameExitButton.Update(mouseState);
                    }
                    break;
                #endregion
                #region Settings Code
                case GameState.Settings:
                    mouseState = Mouse.GetState();
                    KeyboardState settingsExit = Keyboard.GetState();

                    //Exit Condition for Settings Screen
                    if (settingsExit.IsKeyDown(Keys.Enter))
                    {
                        CurrentGameState = GameState.MainMenu;
                    }

                    break;
                #endregion
                #region Credits Code
                case GameState.Credits:
                    if (exitInput.IsKeyDown(Keys.Q))
                    {
                        CurrentGameState = GameState.MainMenu;
                    }

                    break;

                    #endregion
            }
            inputHelper.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            switch(CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    mainMenu1P.Draw(spriteBatch);
                    mainMenu2P.Draw(spriteBatch);
                    mainMenuSettings.Draw(spriteBatch);
                    mainMenuCredits.Draw(spriteBatch);
                    mainMenuQuit.Draw(spriteBatch);
                    MainMenuSprite.Draw(spriteBatch);
                    spriteBatch.End();
                    //*/

                    //If I want to make there be a highlighting box
                    //Make sure to change the MenuLineBar.bmp so that
                    //It is just one color

                    /*
                    mainMenu1P.Draw(spriteBatch);
                    mainMenu2P.Draw(spriteBatch);
                    mainMenuSettings.Draw(spriteBatch);
                    mainMenuCredits.Draw(spriteBatch);
                    mainMenuQuit.Draw(spriteBatch);
                    MainMenuSprite.Draw(spriteBatch);
                    //*/

                    break;

                case GameState.InGame1P:
                    spriteBatch.Begin();
                    if (gamePaused)
                    {
                        pauseScreenSprite.Draw(spriteBatch);
                        pauseExitButton.Draw(spriteBatch);
                        gameExitButton.Draw(spriteBatch);
                    }
                    else
                    {
                        //print out associated things with one player game
                        //and which settings are active
                        P1Game.Draw(spriteBatch);
                        spriteBatch.DrawString(Font1, "Computer: " + P1Game.P2.score, new Vector2(5, 10), Color.LimeGreen);
                        spriteBatch.DrawString(Font1, "Player 1: " + P1Game.P1.score,
                            new Vector2(graphics.GraphicsDevice.Viewport.Width - Font1.MeasureString("Player 1: " + P1Game.P1.score).X - 5, 10), Color.LimeGreen);
                        //gameBoardSprite.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    break;
                case GameState.InGame2P:
                    spriteBatch.Begin();
                    if (gamePaused)
                    {
                        pauseScreenSprite.Draw(spriteBatch);
                        pauseExitButton.Draw(spriteBatch);
                        gameExitButton.Draw(spriteBatch);
                    }
                    else
                    {
                        P2Game.Draw(spriteBatch);
                        spriteBatch.DrawString(Font1, "Player 2: " + P1Game.P2.score, new Vector2(5, 10), Color.LimeGreen);
                        spriteBatch.DrawString(Font1, "Player 1: " + P1Game.P1.score,
                            new Vector2(graphics.GraphicsDevice.Viewport.Width - Font1.MeasureString("Player 1: " + P1Game.P1.score).X - 5, 10), Color.LimeGreen);
                    }
                    spriteBatch.End();
                    break;
                case GameState.Settings:
                    spriteBatch.Begin();
                    settingsSprite.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Credits:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(Font1, "Credits\n     Danny Cahoon\n\n     Jacob J\n\n     Hugh Ohlin\n\n     Robbie Fikes\n\n", new Vector2(((graphics.GraphicsDevice.Viewport.Width / 2) - Font1.MeasureString("Credits").X - 10), 10), Color.LimeGreen);
                    spriteBatch.DrawString(Font1, "Go Back. . . ", new Vector2(5, graphics.GraphicsDevice.Viewport.Height - Font1.MeasureString("Go Back. . .").Y), Color.LimeGreen);
                    spriteBatch.End();
                    break;
            }
           base.Draw(gameTime);
        }
    }
}
