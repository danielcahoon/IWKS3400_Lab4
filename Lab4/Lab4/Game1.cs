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
       
        clsSprite gymSprite;
        
        //Font Stuff
        SpriteFont Font1;
        public string victory; //used to hold congratulations/blnt message

        //Sound and Music Stuff
        SoundEffect menuEffect, ballHit, gameWin, score;
        AudioEngine audioEngine;
        SoundBank soundsBank;
        WaveBank sounds;

        WaveBank mainMenu, settings, credits, hardAI, medAI, easyAI, twoPlayer;
        public Cue mainMenuCue, settingsCue, creditsCue, hardAICue, medAICue, easyAICue, twoPlayerCue;


        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
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
        #region Credit Screen Sprite and Button
        clsSprite creditScreen;
        #endregion
        #region Pause Screen Sprite and Buttons
        //Sprites
        //Single Player
        clsSprite pauseScreen1P;
        clsSprite pauseScreen1P_Ball;
        clsSprite pauseScreen1P_Barrier;
        clsSprite pauseScreen1P_All;
        //Two Player
        clsSprite pauseScreen2P;
        clsSprite pauseScreen2P_Ball;
        clsSprite pauseScreen2P_Barrier;
        clsSprite pauseScreen2P_All;
        //Buttons
        clsButton pauseExitButton;
        clsButton gameExitButton;
        #endregion

        #endregion

        PongGame P1Game;
        PongGame P2Game;
        public static pongSettings gameSettings;
        bool gamePaused;
        enum GameState
        {
            MainMenu,
            InGame1P,
            Pause1P,
            GameEnd1P,
            Instructions1P,
            InGame2P,
            Pause2P,
            GameEnd2P,
            Instructions2P,
            Settings,
            Credits
        }
        GameState CurrentGameState = GameState.MainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;

            //Set window size
            graphics.PreferredBackBufferWidth = 1248;
            graphics.PreferredBackBufferHeight = 623;
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

            #region Music Stuff
            // Load the SoundEffect resource
            menuEffect = Content.Load<SoundEffect>("MenuSoundEffect");
            ballHit = Content.Load<SoundEffect>("BallHit");
            score = Content.Load<SoundEffect>("Score");
            gameWin = Content.Load<SoundEffect>("GameWin");

            // Load file built from XACT project
            audioEngine = new AudioEngine("Content\\Lab4Sounds.xgs");
            sounds = new WaveBank(audioEngine, "Content\\Sounds.xwb");
            soundsBank = new SoundBank(audioEngine, "Content\\SoundsBank.xsb");

            // Load streaming wave banks
            mainMenu = new WaveBank(audioEngine, "Content\\MainMenu.xwb", 0, 4);
            settings = new WaveBank(audioEngine, "Content\\Settings.xwb", 0, 4);
            credits = new WaveBank(audioEngine, "Content\\Credits.xwb", 0, 4);
            easyAI = new WaveBank(audioEngine, "Content\\EasyAI.xwb", 0, 4);
            medAI = new WaveBank(audioEngine, "Content\\MediumAI.xwb", 0, 4);
            hardAI = new WaveBank(audioEngine, "Content\\HardAI.xwb", 0, 4);
            twoPlayer = new WaveBank(audioEngine, "Content\\2Player.xwb", 0, 4);

            // The audio engine must be updated before the streaming cues are ready
            audioEngine.Update();

            // Get cues for streaming music
            mainMenuCue = soundsBank.GetCue("MainMenuMusic");
            settingsCue = soundsBank.GetCue("SettingsMusic");
            creditsCue = soundsBank.GetCue("Credits");
            easyAICue = soundsBank.GetCue("EasyAI");
            medAICue = soundsBank.GetCue("MediumAI");
            hardAICue = soundsBank.GetCue("HardAI");
            twoPlayerCue = soundsBank.GetCue("2Player");

            audioEngine.Update();
            settingsCue.Play();
            creditsCue.Play();
            easyAICue.Play();
            medAICue.Play();
            hardAICue.Play();
            twoPlayerCue.Play();
            mainMenuCue.Play();
            #endregion

            gameSettings = new pongSettings();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            P1Game = new Lab4.PongGame(Content.Load<Texture2D>("paddle-ash"), Content.Load<Texture2D>("paddle-gary"), Content.Load<Texture2D>("barrier"), Content.Load<Texture2D>("pokeball"), 
                Content.Load<Texture2D>(/*"ballSpeedUp"*/"power-ball-fast"), Content.Load<Texture2D>(/*"ballSpeedDown"*/"power-ball-slow"), Content.Load<Texture2D>(/*"barrierSpeedUp"*/"power-barrier-speed"), Content.Load<Texture2D>(/*"barrierSpeedDown"*/"power-barrier-slow"), graphics, 1);

            P2Game = new Lab4.PongGame(Content.Load<Texture2D>("paddle-ash"), Content.Load<Texture2D>("paddle-gary"), Content.Load<Texture2D>("barrier"), Content.Load<Texture2D>("pokeball"),
                Content.Load<Texture2D>(/*"ballSpeedUp"*/"power-ball-fast"), Content.Load<Texture2D>(/*"ballSpeedDown"*/"power-ball-slow"), Content.Load<Texture2D>(/*"barrierSpeedUp"*/"power-barrier-speed"), Content.Load<Texture2D>(/*"barrierSpeedDown"*/"power-barrier-slow"), graphics, 2);

            //Load 2D Content into the Sprites
            MainMenuSprite = new clsSprite(Content.Load<Texture2D>("menu-main"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            #region Menu Buttons
            //Load 2D content into my MainMenuButton
            mainMenu1P = new clsButton(Content.Load<Texture2D>("arrow-right"), new Vector2(420, 30), false, false);
            mainMenu1P.setPosition(new Vector2(400, 152));
            mainMenu2P = new clsButton(Content.Load<Texture2D>("arrow-right"), new Vector2(420, 30), false, false);
            mainMenu2P.setPosition(new Vector2(400, 216));
            mainMenuSettings = new clsButton(Content.Load<Texture2D>("arrow-right"), new Vector2(420, 30), false, false);
            mainMenuSettings.setPosition(new Vector2(400, 280));
            mainMenuCredits = new clsButton(Content.Load<Texture2D>("arrow-right"), new Vector2(420, 30), false, false);
            mainMenuCredits.setPosition(new Vector2(400, 344));
            mainMenuQuit = new clsButton(Content.Load<Texture2D>("arrow-right"), new Vector2(420, 30), false, false);
            mainMenuQuit.setPosition(new Vector2(400, 442));
            #endregion

            gymSprite = new clsSprite(Content.Load<Texture2D>("gym2"),
                                new Vector2(0f, 0f), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            creditScreen = new clsSprite(Content.Load<Texture2D>("menu-credits"),
                                new Vector2(0f, 0f), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            settingsSprite = new clsSprite(Content.Load<Texture2D>("menu-settings"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            #region Setting Buttons
            #region Barrier
            settingBarrierOn = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(61, 40), true, false);
            settingBarrierOn.setPosition(new Vector2(707, 151));
            settingBarrierOn.addColor();
            settingBarrierOff = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(92, 40), true, false);
            settingBarrierOff.setPosition(new Vector2(803, 151));
            #endregion
            #region PowerUps
            settingPowerUpsOn = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(61, 40), true, false);
            settingPowerUpsOn.setPosition(new Vector2(707, 215));
            settingPowerUpsOn.addColor();
            settingPowerUpsOff = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(92, 40), true, false);
            settingPowerUpsOff.setPosition(new Vector2(803, 215));
            #endregion
            #region Difficulty
            settingDifficulty1 = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(25, 30), true, false);
            settingDifficulty1.setPosition(new Vector2(711, 284));
            settingDifficulty1.addColor();
            settingDifficulty2 = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(30, 30), true, false);
            settingDifficulty2.setPosition(new Vector2(803, 284));
            settingDifficulty3 = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(30, 30), true, false);
            settingDifficulty3.setPosition(new Vector2(899, 284));

            #endregion
            #region Music
            settingMusicOn = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(61, 40), true, false);
            settingMusicOn.setPosition(new Vector2(707, 343));
            settingMusicOn.addColor();
            settingMusicOff = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(92, 40), true, false);
            settingMusicOff.setPosition(new Vector2(803, 343));
            #endregion
            settingExitButton = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(125, 40), false, false);
            settingExitButton.setPosition(new Vector2(319, 441));
            #endregion
            #region Pause Buttons and Sprites
            //Sprites
            pauseScreen1P = new clsSprite(Content.Load<Texture2D>("pause-1P"),
                                   new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pauseScreen1P_Ball = new clsSprite(Content.Load<Texture2D>("pause-1P-ball"),
                                   new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pauseScreen1P_Barrier = new clsSprite(Content.Load<Texture2D>("pause-1P-barrier"),
                                   new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pauseScreen1P_All = new clsSprite(Content.Load<Texture2D>("pause-1P-ball-barrier"),
                                   new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            pauseScreen2P = new clsSprite(Content.Load<Texture2D>("pause-2P"),
                                   new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pauseScreen2P_Ball = new clsSprite(Content.Load<Texture2D>("pause-2P-ball"),
                                               new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pauseScreen2P_Barrier = new clsSprite(Content.Load<Texture2D>("pause-2P-barrier"),
                                               new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pauseScreen2P_All = new clsSprite(Content.Load<Texture2D>("pause-2P-ball-barrier"),
                                               new Vector2(0f, 0f), new Vector2(1248f, 623f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);


            //Buttons
            gameExitButton = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(110, 19), false, false);
            gameExitButton.setPosition(new Vector2(578, 13));
            pauseExitButton = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2((672 - 581), 19), false, false);
            pauseExitButton.setPosition(new Vector2(581, 45));
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

            creditScreen.texture.Dispose();

            settingsSprite.texture.Dispose();


            //Pause Screen Disposal
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
            #region Music Stuff
            if (gameSettings.music == true)
            {
                switch (CurrentGameState)
                {
                    #region Main Menu Music
                    case GameState.MainMenu:
                        audioEngine.Update();
                        mainMenuCue.Resume();
                        settingsCue.Pause();
                        creditsCue.Pause();
                        #region Difficulty Music
                        easyAICue.Pause();
                        medAICue.Pause();
                        hardAICue.Pause();
                        #endregion
                        twoPlayerCue.Pause();
                        break;
                    #endregion
                    #region Settings Music
                    case GameState.Settings:
                        audioEngine.Update();
                        mainMenuCue.Pause();
                        settingsCue.Resume();
                        creditsCue.Pause();
                        #region Difficulty Music
                        easyAICue.Pause();
                        medAICue.Pause();
                        hardAICue.Pause();
                        #endregion
                        twoPlayerCue.Pause();
                        break;
                    #endregion
                    #region Credits Music
                    case GameState.Credits:
                        audioEngine.Update();
                        mainMenuCue.Pause();
                        settingsCue.Pause();
                        creditsCue.Resume();
                        #region Difficulty Music
                        easyAICue.Pause();
                        medAICue.Pause();
                        hardAICue.Pause();
                        #endregion
                        twoPlayerCue.Pause();
                        break;
                    #endregion
                    #region 1 Player Music
                    case GameState.InGame1P:
                        audioEngine.Update();
                        mainMenuCue.Pause();
                        settingsCue.Pause();
                        creditsCue.Pause();
                        #region Difficulty Music
                        switch(gameSettings.difficulty)
                        {
                            case pongSettings.Difficulty.Easy:
                                easyAICue.Resume();
                                medAICue.Pause();
                                hardAICue.Pause();
                                break;
                            case pongSettings.Difficulty.Medium:
                                easyAICue.Pause();
                                medAICue.Resume();
                                hardAICue.Pause();
                                break;
                            case pongSettings.Difficulty.Hard:
                                easyAICue.Pause();
                                medAICue.Pause();
                                hardAICue.Resume();
                                break;
                        }
                        #endregion
                        twoPlayerCue.Pause();
                        break;
                    #endregion
                    #region 2 Player Music
                    case GameState.InGame2P:
                        audioEngine.Update();
                        mainMenuCue.Pause();
                        settingsCue.Pause();
                        creditsCue.Pause();
                        #region Difficulty Music
                        easyAICue.Pause();
                        medAICue.Pause();
                        hardAICue.Pause();
                        #endregion
                        twoPlayerCue.Resume();
                        break;
                        #endregion
                }
            }
            else
            {
                //Pause any music if setting says 
                audioEngine.Update();
                mainMenuCue.Pause();
                settingsCue.Pause();
                creditsCue.Pause();
                easyAICue.Pause();
                twoPlayerCue.Pause();
            }
            #endregion
            #region Game Code
            switch (CurrentGameState)
            {
                #region MainMenu Code
                case GameState.MainMenu:
                    mouseState = Mouse.GetState();
                    if (mainMenu1P.isClicked == true)
                    {
                        mainMenu1P.isClicked = false;
                        menuEffect.Play();
                        CurrentGameState = GameState.Instructions1P;
                        P1Game.Reset();
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    mainMenu1P.Update(mouseState);

                    if (mainMenu2P.isClicked == true)
                    {
                        mainMenu2P.isClicked = false;
                        menuEffect.Play();
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
                        mainMenuSettings.isClicked = false;
                        menuEffect.Play();
                        CurrentGameState = GameState.Settings;
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    mainMenuSettings.Update(mouseState);

                    if (mainMenuCredits.isClicked == true)
                    {
                        mainMenuCredits.isClicked = false;
                        menuEffect.Play();
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
                case GameState.Instructions1P:
                    if (exitInput.IsKeyDown(Keys.Up))
                    {
                        CurrentGameState = GameState.InGame1P;
                    }
                    else if (exitInput.IsKeyDown(Keys.Down))
                    {
                        CurrentGameState = GameState.InGame1P;
                    }
                    break;
                case GameState.InGame1P:
                    mouseState = Mouse.GetState();

                    if (exitInput.IsKeyDown(Keys.P))
                    {
                        gamePaused = true;
                        CurrentGameState = GameState.Pause1P;
                    }
                    if (!gamePaused)
                    {
                        if (P1Game.gameActive)
                        {
                            P1Game.Update(exitInput, gameTime);
                        }
                        else
                        {
                            P1Game.Reset();
                            CurrentGameState = GameState.GameEnd1P;
                        }

                        //TODO Implement actual exit condition
                        if (exitInput.IsKeyDown(Keys.Space))
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    break;
                #endregion
                #region Single Player Pause Code
                case (GameState.Pause1P):
                    
                    //Ensures that P is not pressed before going out of the pause
                    
                    if (exitInput.IsKeyDown(Keys.P))
                    {
                        if (exitInput.IsKeyUp(Keys.P))
                        {
                            gamePaused = false;
                            CurrentGameState = GameState.InGame1P;
                        }
                    }
                    else
                    {
                        //Exits to main menu
                        if (exitInput.IsKeyDown(Keys.Enter))
                        {
                            if (exitInput.IsKeyUp(Keys.Enter))
                            {
                                gamePaused = false;
                                CurrentGameState = GameState.MainMenu;
                            }
                        }
                    }

                    //Code for using buttons to exit 

                    //if (pauseExitButton.isClicked)
                    //{
                    //    gamePaused = false;
                    //    if (mouseState.LeftButton == ButtonState.Released)
                    //    {
                    //        CurrentGameState = GameState.InGame1P;
                    //        gamePaused = false;
                    //    }
                    //}
                    ////Exiting Game needs debugging from Pause Screen
                    //else if (gameExitButton.isClicked)
                    //{
                    //    if (mouseState.LeftButton == ButtonState.Released && exitInput.IsKeyUp(Keys.P))
                    //    {
                    //        gamePaused = false;
                    //        CurrentGameState = GameState.MainMenu;
                    //    }
                    //}
                    //pauseExitButton.Update(mouseState);
                    //gameExitButton.Update(mouseState);

                    break;
                #endregion
                #region Single Player Game End Code
                case GameState.GameEnd1P:
                    if (exitInput.IsKeyDown(Keys.Enter))
                    {
                        CurrentGameState = GameState.MainMenu;
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
                            P2Game.Update(exitInput, gameTime);
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
                #region Two Player Pause Code

                #endregion
                #region Settings Code
                case GameState.Settings:
                    mouseState = Mouse.GetState();
                    KeyboardState settingsExit = Keyboard.GetState();
                    
                    #region Barrier Settings
                    if(settingBarrierOn.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingBarrierOff.removeColor();
                            settingBarrierOn.addColor();
                            gameSettings.barriers = true;
                        }
                        
                    }
                    if (settingBarrierOff.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingBarrierOn.removeColor();
                            settingBarrierOff.addColor();
                            gameSettings.barriers = false;
                        }
                        
                    }
                    settingBarrierOn.Update(mouseState);
                    settingBarrierOff.Update(mouseState);
                    #endregion
                    #region PowerUp Settings
                    if (settingPowerUpsOn.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingPowerUpsOff.removeColor();
                            settingPowerUpsOn.addColor();
                            gameSettings.powerUps = true;
                        }

                    }
                    if (settingPowerUpsOff.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingPowerUpsOn.removeColor();
                            settingPowerUpsOff.addColor();
                            gameSettings.powerUps = false;
                        }

                    }
                    settingPowerUpsOn.Update(mouseState);
                    settingPowerUpsOff.Update(mouseState);
                    #endregion
                    #region Difficulty Settings
                    if (settingDifficulty1.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingDifficulty1.addColor();
                            settingDifficulty2.removeColor();
                            settingDifficulty3.removeColor();
                            gameSettings.difficulty = pongSettings.Difficulty.Easy;
                        }

                    }
                    if (settingDifficulty2.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingDifficulty1.removeColor();
                            settingDifficulty2.addColor();
                            settingDifficulty3.removeColor();
                            gameSettings.difficulty = pongSettings.Difficulty.Medium;
                        }

                    }
                    if (settingDifficulty3.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingDifficulty1.removeColor();
                            settingDifficulty2.removeColor();
                            settingDifficulty3.addColor();
                            gameSettings.difficulty = pongSettings.Difficulty.Hard;
                        }

                    }
                    settingDifficulty1.Update(mouseState);
                    settingDifficulty2.Update(mouseState);
                    settingDifficulty3.Update(mouseState);
                    #endregion
                    #region Music Settings
                    if (settingMusicOn.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingMusicOff.removeColor();
                            settingMusicOn.addColor();
                            gameSettings.music = true;
                        }

                    }
                    if (settingMusicOff.isClicked)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            settingMusicOn.removeColor();
                            settingMusicOff.addColor();
                            gameSettings.music = false;
                        }

                    }
                    settingMusicOn.Update(mouseState);
                    settingMusicOff.Update(mouseState);
                    #endregion
                    #region Exit Settings
                    if (settingExitButton.isClicked == true)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    settingExitButton.Update(mouseState);
                    #endregion//Exit Condition for Settings Screen
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
            #endregion
            // Update the audio engine
            audioEngine.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    MainMenuSprite.Draw(spriteBatch);
                    mainMenu1P.Draw(spriteBatch);
                    mainMenu2P.Draw(spriteBatch);
                    mainMenuSettings.Draw(spriteBatch);
                    mainMenuCredits.Draw(spriteBatch);
                    mainMenuQuit.Draw(spriteBatch);
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
                case GameState.Instructions1P:
                    spriteBatch.Begin();
                    if (gameSettings.barriers && gameSettings.powerUps)
                    {
                        pauseScreen1P_All.Draw(spriteBatch);
                    }
                    else if (gameSettings.powerUps)
                    {
                        pauseScreen1P_Ball.Draw(spriteBatch);
                    }
                    else
                    {
                        pauseScreen1P.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    break;
                case GameState.InGame1P:
                    spriteBatch.Begin();
                    gymSprite.Draw(spriteBatch);
                    P1Game.Draw(spriteBatch);
                    spriteBatch.DrawString(Font1, "Gary: " + P1Game.P2.score, new Vector2(130, 60), Color.Black);
                    spriteBatch.DrawString(Font1, "Ash: " + P1Game.P1.score, new Vector2(997, 60), Color.Black);
                        
                    spriteBatch.End();
                    break;
                case GameState.Pause1P:
                    spriteBatch.Begin();
                    if(gameSettings.barriers && gameSettings.powerUps)
                    {
                        pauseScreen1P_All.Draw(spriteBatch);
                    }
                    else if (gameSettings.barriers)
                    {
                        pauseScreen1P_Barrier.Draw(spriteBatch);
                    }
                    else if(gameSettings.powerUps)
                    {
                        pauseScreen1P_Ball.Draw(spriteBatch);
                    }
                    else
                    {
                        pauseScreen1P.Draw(spriteBatch);
                    }

//                    pauseExitButton.Draw(spriteBatch);
  //                  gameExitButton.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.GameEnd1P:
                    spriteBatch.Begin();
                    if(P1Game.P1.score > P1Game.P2.score)
                    {
                        //Draw the winning screen for Ash (Player 1) with points
                    }
                    else
                    {
                        //Draw the winning screen for Gary (Player 2) with points
                    }
                    spriteBatch.End();
                    break;
                case GameState.InGame2P:
                    spriteBatch.Begin();
                    gymSprite.Draw(spriteBatch);
                    P2Game.Draw(spriteBatch);
                    spriteBatch.DrawString(Font1, "Gary: " + P2Game.P2.score, new Vector2(130, 60), Color.Black);
                    spriteBatch.DrawString(Font1, "Ash: " + P2Game.P1.score, new Vector2(997, 60), Color.Black);
                    spriteBatch.End();
                    break;
                case GameState.Pause2P:
                    if (gameSettings.barriers && gameSettings.powerUps)
                    {
                        pauseScreen2P_All.Draw(spriteBatch);
                    }
                    else if (gameSettings.barriers)
                    {
                        pauseScreen2P_Barrier.Draw(spriteBatch);
                    }
                    else if (gameSettings.powerUps)
                    {
                        pauseScreen2P_Ball.Draw(spriteBatch);
                    }
                    else
                    {
                        pauseScreen2P.Draw(spriteBatch);
                    }
                    pauseExitButton.Draw(spriteBatch);
                    gameExitButton.Draw(spriteBatch);
                    break;
                case GameState.GameEnd2P:
                    spriteBatch.Begin();
                    if (P2Game.P1.score > P2Game.P2.score)
                    {
                        //Draw the winning screen for Ash (Player 1) with points
                    }
                    else
                    {
                        //Draw the winning screen for Gary (Player 2) with points
                    }
                    spriteBatch.End();
                    break;

                case GameState.Settings:
                    spriteBatch.Begin();
                    settingsSprite.Draw(spriteBatch);
                    settingBarrierOn.Draw(spriteBatch);
                    settingBarrierOff.Draw(spriteBatch);
                    settingPowerUpsOn.Draw(spriteBatch);
                    settingPowerUpsOff.Draw(spriteBatch);
                    settingDifficulty1.Draw(spriteBatch);
                    settingDifficulty2.Draw(spriteBatch);
                    settingDifficulty3.Draw(spriteBatch);
                    settingMusicOn.Draw(spriteBatch);
                    settingMusicOff.Draw(spriteBatch);
                    settingExitButton.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Credits:
                    spriteBatch.Begin();
                    //spriteBatch.DrawString(Font1, "Credits\n     Danny Cahoon\n\n     Jacob Jolly\n\n     Hugh Ohlin\n\n     Robbie Fikes\n\n", new Vector2(((graphics.GraphicsDevice.Viewport.Width / 2) - Font1.MeasureString("Credits").X - 10), 10), Color.LimeGreen);
                    //spriteBatch.DrawString(Font1, "Go Back. . . ", new Vector2(5, graphics.GraphicsDevice.Viewport.Height - Font1.MeasureString("Go Back. . .").Y), Color.LimeGreen);
                    creditScreen.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
