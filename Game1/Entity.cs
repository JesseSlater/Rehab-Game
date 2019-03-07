﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;

namespace Game1
{
    public abstract class Entity
    {
        protected bool isInvisible = false; 
        protected Texture2D image;
        // The tint of the image. This will also allow us to change the transparency.
        protected Color color = Color.White;

        protected World world;
        protected Body body;

        public Vector2 Position, Velocity;
        public float Orientation = 0;
        //public float Radius = 20;   // used for circular collision detection
        public bool IsExpired;      // true if the entity was destroyed and should be deleted.
        public SpriteEffects flipped = SpriteEffects.None;
        public bool isFlipped = false;
        public Vector2 Size
        {
            get
            {
                return image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
            }
        }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(image, Position, null, color, Orientation, new Vector2(0,0), 1f, SpriteEffects.FlipHorizontally, 0);
            if(image != null && !isInvisible)
                spriteBatch.Draw(image, Position, color);
            

        }

        public virtual void SetWorld(World w)
        {
            world = w;
        }

        public virtual void MakeInvisible()
        {
            isInvisible = true;
        }

        public virtual void MakeVisible()
        {
            isInvisible = false;
        }

        public virtual void DestroyBody()
        {
            GameRoot.RemoveBody(body);
        }

        protected virtual void DestroyFromWorld()
        {
            DestroyBody();
            MakeInvisible();
        }
    }
}