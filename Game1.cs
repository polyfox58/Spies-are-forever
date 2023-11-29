using System;
using System.IO;
using System.Runtime.CompilerServices;
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
        private Texture2D grassTile;
        private Vector2 playerPosition;
        private Vector2 tilePlacePointer;
        private float playerSpeed;
        private const int mapSize = 100;
        private int[,] mapArray = new int[mapSize,mapSize];
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
            GenerateMapArray("test map.txt");
            GenerateMap();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            errorText = Content.Load<SpriteFont>("errorText");   
            playerCharacter = Content.Load<Texture2D>("player character");
            grassTile = Content.Load<Texture2D>("grass tile");
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
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerCharacter, playerPosition, null, Color.White, 0f, new Vector2(playerCharacter.Width / 2, playerCharacter.Height / 2), Vector2.One, SpriteEffects.None, 0f); 
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void GenerateMap()
        {
            tilePlacePointer = new Vector2(0, 0);
            int i = 0;
            int j = 0;
            _spriteBatch.Begin();
            while (this.mapArray[i,j] != 0)
            {
                while (this.mapArray[i,j] != 1)
                {
                    switch (this.mapArray[i, j])
                    {
                        case 2:
                            _spriteBatch.Draw(grassTile,new Vector2 (i*100, j*100), Color.Green);
                            break;
                        case 3:
                            break;
                    }
                    
                
                }
            }
            _spriteBatch.End();
        }
        public void DisplayMessage(SpriteFont messageType, string message)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(messageType, message, new Vector2(0,0),Color.Red);
            _spriteBatch.End();
        }
       
        public void GenerateMapArray(string mapName)
        {
           //pulls the map out of the file line by line and converts it into an int array to be stored in memory
            try
            {
                using (StreamReader mapReader = new StreamReader(Path.GetDirectoryName(mapName)))  
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
                        int[] intProcessedLine = new int[mapSize];
                        int l = 0;
                        foreach (string temp in processedLine)
                        {
                            intProcessedLine[l] = Convert.ToInt16(temp);
                            l++;
                        }
                        
                        for (int j = 0; j >= processedLine.Length; j++)
                        {
                            mapArray[n, j] = intProcessedLine[j];
                        }
                    }
                }
            }
            catch
            {
                DisplayMessage(errorText, "could not read " + mapName);
            }
            //test
        }

    }
}