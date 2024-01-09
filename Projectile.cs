using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA
{
    public class Projectile : Game1
    {
        private float _speed; //speed is the distance of the hypotenuse of a right angle triange where the other 2 sides are x and y
        private Texture2D _sprite;
        private float _direction; //angle in radians
        private Vector2 _coords;
        public Projectile()
        {
            _speed = 0;
            _sprite = null;
            _direction = 0f;
            _coords = new Vector2(0, 0);
        }
        public Projectile(float speed, Texture2D sprite, float direction, Vector2 coords)
        {
            _speed = speed;
            _sprite = sprite;
            _direction = direction;
            _coords = coords;
        }
        public void moveProjectile(GameTime gameTime)
        {
            _coords.Y += (float)Math.Sin(_direction) * _speed;
            _coords.X += (float)Math.Cos(_speed) * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;    <- movement reference

        }
        public Vector2 getCoords()
        {
            return _coords;
        }
        public float getDirection()
        {
            return _direction;
        }
        public float speed()
        {
            return _speed;
        }
    }
}
