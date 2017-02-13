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
        clsSprite settingsSprite;
        //Buttons
        clsButton settingScreenSelection1;
        clsButton settingScreenSelection2;
        clsButton settingScreenSelection3;
        clsButton settingScreenSelection4;
        #endregion

        clsSprite creditsSprite;
        clsSprite gameBoardSprite;
        #region Pause Screen Sprite and Buttons
        //Sprites
        clsSprite pauseScreenSprite;
        //Buttons
        clsButton pauseExitButton;
        clsButton gameExitButton;
        #endregion

        #endregion


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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            inputHelper = new InputHelper();
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

            #endregion
            creditsSprite = new clsSprite(Content.Load<Texture2D>("CreditScreen"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            gameBoardSprite = new clsSprite(Content.Load<Texture2D>("GameScreen"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pauseScreenSprite = new clsSprite(Content.Load<Texture2D>("PauseScreen1"),
                                    new Vector2(0f, 0f), new Vector2(700f, 500f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            #region Pause Buttons
            gameExitButton = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2(110, 19));
            gameExitButton.setPosition(new Vector2(578, 13));
            pauseExitButton = new clsButton(Content.Load<Texture2D>("MenuLineBar"), new Vector2((672-581), 19));
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

            settingsSprite.texture.Dispose();

            creditsSprite.texture.Dispose();

            gameBoardSprite.texture.Dispose();

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
                    }
                    mainMenu1P.Update(mouseState);

                    if (mainMenu2P.isClicked == true)
                    {
                        CurrentGameState = GameState.InGame2P;
                    }
                    mainMenu2P.Update(mouseState);

                    if (mainMenuSettings.isClicked == true)
                    {
                        CurrentGameState = GameState.Settings;
                    }
                    mainMenuSettings.Update(mouseState);

                    if (mainMenuCredits.isClicked == true)
                    {
                        CurrentGameState = GameState.Credits;
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
                        //Update logic for single player game here
                        /*while(Game isn't done)
                         * {
                         *      Play through game
                         *      Update Score
                         *      Etc
                         *  }
                        //*/

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
                        else if (gameExitButton.isClicked)
                        {
                            gamePaused = false;
                            CurrentGameState = GameState.MainMenu;
                        }
                        pauseExitButton.Update(mouseState);
                        gameExitButton.Update(mouseState);
                    }
                    break;
                #endregion
                #region Two Player Code
                case GameState.InGame2P:
                    mouseState = Mouse.GetState();
                    Console.WriteLine(CurrentGameState);
                    if (!gamePaused)
                    {
                        //Code to pause game
                        if (exitInput.IsKeyDown(Keys.P) && !inputHelper.IsNewPress(Keys.P))
                        {
                            inputHelper.Update();
                            gamePaused = true;
                        }
                        //Update logic for single player game here

                        //TODO Implement actual exit condition
                        if (exitInput.IsKeyDown(Keys.Space))
                        {
                            CurrentGameState = GameState.MainMenu;
                        }
                    }
                    else
                    {
                        Console.WriteLine("In Pause Screen");

                        pauseExitButton.Update(mouseState);
                        gameExitButton.Update(mouseState);
                        if (pauseExitButton.isClicked)
                        {
                            gamePaused = false;
                        }
                        else if (gameExitButton.isClicked)
                        {
                            gamePaused = false;
                            CurrentGameState = GameState.MainMenu;
                            Console.WriteLine(CurrentGameState);
                        }
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

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if(CurrentGameState == GameState.InGame1P || CurrentGameState == GameState.InGame2P)
            {
                if(gamePaused)
                {
                    pauseScreenSprite.Draw(spriteBatch);
                    pauseExitButton.Draw(spriteBatch);
                    gameExitButton.Draw(spriteBatch);
                }
                else
                {
                    if (CurrentGameState == GameState.InGame1P)
                    {
                        //print out associated things with one player game
                        //and which settings are active
                        gameBoardSprite.Draw(spriteBatch);
                    }
                    else
                    {
                        //print out associated things with a two player game
                        //and which settings are active
                        gameBoardSprite.Draw(spriteBatch);
                    }
                }
            }
            else if(CurrentGameState == GameState.Settings)
            {
                settingsSprite.Draw(spriteBatch);
            }
            else if (CurrentGameState == GameState.Credits)
            {
                creditsSprite.Draw(spriteBatch);
            }
            else
            {
                if (CurrentGameState == GameState.MainMenu)
                {

                    mainMenu1P.Draw(spriteBatch);
                    mainMenu2P.Draw(spriteBatch);
                    mainMenuSettings.Draw(spriteBatch);
                    mainMenuCredits.Draw(spriteBatch);
                    mainMenuQuit.Draw(spriteBatch);
                    MainMenuSprite.Draw(spriteBatch);
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
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
