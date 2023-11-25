using System;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NEA
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D playerCharacter;
        private Vector2 playerPosition;
        private float playerSpeed;
        private string[,] mapArray = new string[100,100];
        private SpriteFont errorText;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            playerSpeed = 100f;
            // ^^ placing the player in the centre of the screen and setting their speed
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            errorText = Content.Load<SpriteFont>("errorText");
            
            playerCharacter = Content.Load<Texture2D>("red circle");
            // TODO: use this.Content to load your game content here
            //map reading
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up))
            {

            }




            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerCharacter, playerPosition, null, Color.White, 0f, new Vector2(playerCharacter.Width / 2, playerCharacter.Height / 2), Vector2.One, SpriteEffects.None, 0f); 
            _spriteBatch.End();
            base.Draw(gameTime);
            
        
        }
        public void DisplayMessage(SpriteFont messageType, string message)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(messageType, message, new Vector2(0,0),Color.Red);
            _spriteBatch.End();
        }
       
        public void GenerateMapArray(string mapName)
        {
            //makes mapreader scan a doc to get number of lines, then run a for loop for the number of lines to add to maparray[]
            try
            {
                using (StreamReader mapReader = new StreamReader(mapName))
                {
                    string line;
                    int i = 0;
                    while ((line = mapReader.ReadLine()) != null)
                    {
                        i++;
                    }
                    for (int n = 0; i == n; n++)
                    {
                        line = mapReader.ReadLine();
                        string[] processedLine = line.Split();
                        for (int j = 0; j >= processedLine.Length; j++)
                        {
                            mapArray[n, j] = processedLine[j];
                        }
                    }
                }
            }
            catch
            {
                DisplayMessage(errorText, "could not read map.txt");
            }
            
        }

    }
}