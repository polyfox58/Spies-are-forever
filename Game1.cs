﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
        private Texture2D stoneBrickTile;
        private Texture2D brickWallTile;
        private Texture2D shallowWaterTile;
        private Texture2D sandTile;
        private Texture2D crosshair;
        private Texture2D guidingLaser;
        private Texture2D bullet;
        private Vector2 playerPosition;
        private Vector2 mousePosition;
        private float playerSpeed;
        public Vector2 relativeMousePosition;
        public float AngleBetweenMouseAndPlayer;
        public Projectile[] projectiles = new Projectile[256];
        private const int mapSize = 100;
        
        private int[,] mapArray = new int[mapSize,mapSize];
        private SpriteFont errorText;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            
            // TODO: Add your initialization logic here
            base.Initialize();
            
            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            playerSpeed = 100f;
            // ^^ placing the player in the centre of the screen and setting their speed
            GenerateMapArray("test map.txt");
            Mouse.SetCursor(MouseCursor.FromTexture2D(crosshair, crosshair.Height/2,crosshair.Width/2));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            errorText = Content.Load<SpriteFont>("Fonts\\errorText");   
            playerCharacter = Content.Load<Texture2D>("Characters\\player character");
            grassTile = Content.Load<Texture2D>("Tiles\\grass tile");
            stoneBrickTile = Content.Load<Texture2D>("Tiles\\stone brick tile");
            brickWallTile = Content.Load<Texture2D>("Tiles\\brick wall tile");
            shallowWaterTile = Content.Load<Texture2D>("Tiles\\shallow water tile");
            sandTile = Content.Load<Texture2D>("Tiles\\sand tile");
            crosshair = Content.Load<Texture2D>("Misc assets\\crosshair");
            guidingLaser = Content.Load<Texture2D>("Misc assets\\guiding laser");
            bullet = Content.Load<Texture2D>("Misc assets\\bullet");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            AngleBetweenMouseAndPlayer = -(float)Math.Atan2(relativeMousePosition.X, relativeMousePosition.Y);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //controls
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState(); //gets coords and clicks
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Space) || mouseState.LeftButton.HasFlag(ButtonState.Pressed))
            {
                projectiles.Append(new Projectile(10, bullet, AngleBetweenMouseAndPlayer,playerPosition));
                DisplayMessage(errorText, "lmb pressed");
            }
           
            mousePosition = new Vector2(mouseState.X, mouseState.Y);
            relativeMousePosition = playerPosition - mousePosition;
            if (projectiles[0] != null)
            {
                foreach (var projectile in projectiles)
                {
                    projectile.moveProjectile();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GenerateMap();
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                playerCharacter,
                playerPosition,
                null,
                Color.White,
                0f,
                new Vector2(playerCharacter.Width / 2, playerCharacter.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);
            _spriteBatch.Draw(
                guidingLaser,
                playerPosition,
                null,
                Color.Blue,
                AngleBetweenMouseAndPlayer,
                new Vector2(guidingLaser.Width / 2, guidingLaser.Height),
                new Vector2(0.1f, 2),
                SpriteEffects.None,
                0f) ;
            _spriteBatch.End();
                
                //DisplayMessage(errorText, playerPosition.ToString());
            base.Draw(gameTime);
       
        }
        //unloads the map from the array and sorts through it, drawing the tiles specified. Maps should always be closed by a "1" on a new line after the final line
        public void GenerateMap()
        {
            int i = 0;
            int j = 0;
            _spriteBatch.Begin();
            while (this.mapArray[i,j] != 1)     //going through first array
            {
                while (this.mapArray[i,j+1] != 0)   //going through second array
                {
                    switch (this.mapArray[i, j])
                    {
                        case 2:
                            _spriteBatch.Draw(grassTile,new Vector2 (j*50, i*50), Color.Green);
                            break;
                        case 3:
                            _spriteBatch.Draw(stoneBrickTile, new Vector2(j * 50, i * 50), Color.White);
                            break;
                        case 4:
                            _spriteBatch.Draw(shallowWaterTile, new Vector2(j * 50, i * 50), Color.White);
                            break;
                        case 5:
                            _spriteBatch.Draw(sandTile, new Vector2(j * 50, i * 50), Color.White);
                            break;
                        case 6:
                            _spriteBatch.Draw(brickWallTile, new Vector2(j * 50, i * 50), Color.White);
                            break;
                        case 7:
                            break;
                        case 99:
                            break;
                    }
                    j++;
                }
                j = 0;
                i++;
            }
            _spriteBatch.End();
        }
        //writes messages to the screen using the font "messageType"
        public Vector2 GetRelativeMousePosition()
        {
            return relativeMousePosition;
        }
        
        public void DisplayMessage(SpriteFont messageType, string message)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(messageType, message, new Vector2(0,0),Color.Red);
            _spriteBatch.End();
        }
       
        //pulls the map out of the file line by line and converts it into an int array to be stored in memory
        public void GenerateMapArray(string mapName)
        {
            try
            {
                    string line;
                    int i = 0;
                using (StreamReader mapReader = new StreamReader(mapName))
                {
                    while ((line = mapReader.ReadLine()) != null)
                    {
                        i++;
                    }
                }
                
                using (StreamReader mapReader = new StreamReader(mapName))
                {
                    for (int n = 0; i > n; n++)
                    {
                        line = mapReader.ReadLine();
                        string[] processedLine = line.Split(",");
                        int[] intProcessedLine = new int[mapSize];
                        int l = 0;
                        foreach (string temp in processedLine)
                        {
                            intProcessedLine[l] = Convert.ToInt16(temp);
                            l++;
                        }

                        for (int j = 0; j <= processedLine.Length; j++)
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

    public class Projectile : Game1
    {
        private float _speed; //speed is the distance of the hypotenuse of a right angle triange where the other 2 sides are x and y
        private Texture2D _sprite;
        private float _direction; //angle in radians
        private Vector2 _coords;
        public Projectile()
        {
            _speed = 10;
            _sprite = null;
            _direction = 0f;
            _coords = new Vector2(0,0);   
        }
        public Projectile(float speed, Texture2D sprite, float direction,Vector2 coords)
        {
            _speed = speed;
            _sprite = sprite;
            _direction = direction;
            _coords = coords;
        }
        public void moveProjectile()
        {
            Vector2 relativeMousePosition = GetRelativeMousePosition();
            _coords.Y += (float)Math.Sin(Math.Atan2(relativeMousePosition.X, relativeMousePosition.Y));
            _coords.X += (float)Math.Sqrt((_coords.Y * _coords.Y) + (_speed * _speed));
            //playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;    <- movement reference
            
        }
    }
}