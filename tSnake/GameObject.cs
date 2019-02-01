using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tSnake
{
    class GameObject
    {
        protected Vector2 position;
        protected Texture2D gfx;

        public GameObject(Texture2D image, float x, float y)
        {
            this.gfx = image;
            this.position.X = x;
            this.position.Y = y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Ritar ut objektet om inte en barnklass har en Draw-metod som körs istället.
            spriteBatch.Draw(gfx, position, Color.White);
        }

        public void MoveTo(Vector2 newPosition)
        {
            // Flyttar objektet till en ny position.
            this.position = newPosition;
        }

        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }
        public float Width { get { return gfx.Width; } }
        public float Height { get { return gfx.Height; } }

    }

    class Player : GameObject
    {
        //En lista för att förvara ormens olika delar
        private List<GameObject> parts;
        //Hastigheten börjar med 64 (samma som bredden på en del) i riktning åt höger.
        Vector2 speed = new Vector2(64, 0);
        //Används för att sinka ned hastigheten till något lämpligt
        int game_speed = 256; //Ändra detta värde när du äter ett äpple för att låta spelet gå fortare.
        int move_time;

        public Player(Texture2D image, float x, float y, int length) : base(image, x, y)
        {
            move_time = game_speed;
            parts = new List<GameObject>();

            for(int i = 1; i<=length; i++)
            { 
                GameObject temp = new GameObject(image, x - i*64, y);
                parts.Add(temp);
            }
        }

        public void Update(GameTime gameTime)
        {

            //Tangentbordsstyrning
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                speed.X = 64;
                speed.Y = 0;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                speed.X = -64;
                speed.Y = 0;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                speed.X = 0;
                speed.Y = -64;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                speed.X = 0;
                speed.Y = 64;
            }

            //Räknar ned tiden tills den ska flyttas
            move_time -= gameTime.ElapsedGameTime.Milliseconds;
            if (move_time<=0)
            {
                //Gör först en imaginär flytt
                Vector2 newPos = position + speed;
                //Spara den gamla positionen
                Vector2 oldPos = position;
                //Flytta fram huvudet
                position = newPos;
                //Använd den gamla positionen som ny position för nästa kroppsdel.
                newPos = oldPos;

                foreach(GameObject p in parts)
                {
                    oldPos = new Vector2(p.X, p.Y);
                    p.MoveTo(newPos);
                    newPos = oldPos;
                }
                move_time = game_speed;
            }

        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gfx, position, Color.White);
            
            foreach (GameObject p in parts)
            {
                p.Draw(spriteBatch);
            }
            
        }
    
    }
}
