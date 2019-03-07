using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using System.Diagnostics;

namespace Game1
{
    class Floor: Entity
    {
        private bool wallsCreated = false;
        public bool canMove = false;
        public int bodyRotateDelay;
        public float moveSpeed = 75f;
        public float wi;
        public float h;
        private float r;
        public Floor(World w, Texture2D texture, Vector2 pos, float boxVerticalAdjust)
        {
            world = w;
            image = texture;
            Position = pos;
            wi = image.Width;
            h = image.Height;
            body = world.CreateRectangle(image.Width, image.Height + 10, 1,new Vector2(pos.X + image.Width/2, pos.Y + boxVerticalAdjust));
            body.BodyType = BodyType.Static;
            body.SetCollisionGroup(GameRoot.PlatformBodyCollisionGroup);
            body.SetFriction(1);

            body.OnCollision += Body_OnCollision;

            CreateWalls(body.Position, new Vector2(image.Width, image.Height + 10));

        }
        public Floor(World w, Vector2 pos, float width, float height)
        { 
            world = w;
            Position = pos;
            body = world.CreateRectangle(width, height, 1, pos);
            body.BodyType = BodyType.Static;
            body.IgnoreGravity = true; 
            body.SetCollisionGroup(GameRoot.PlatformBodyCollisionGroup);
            body.SetFriction(1);
            wi = width;
            h = height;
            body.OnCollision += Body_OnCollision;
            CreateWalls(body.Position, new Vector2(wi, h));


        }

        public Floor(World w, Vector2 pos, float width, float height, float rotation)
        {
            world = w;
            Position = pos;
            body = world.CreateRectangle(width, height, 1, pos);
            body.BodyType = BodyType.Static;
            body.IgnoreGravity = true;
            body.SetCollisionGroup(GameRoot.PlatformBodyCollisionGroup);
            body.SetFriction(1);
            body.Rotation = rotation;
            wi = width;
            h = height;
            body.OnCollision += Body_OnCollision;




        }

        public Floor(World w, Vector2 pos, float width, float height, bool isDebug)
        {
            world = w;
            Position = pos;
            body = world.CreateRectangle(width, height, 1, pos);
            body.BodyType = BodyType.Kinematic;
            body.IgnoreGravity = true;
            body.SetCollisionGroup(GameRoot.PlatformBodyCollisionGroup);
            body.SetFriction(1);
            wi = width;
            h = height;
            canMove = isDebug;


            body.OnCollision += Body_OnCollision;
            CreateWalls(body.Position, new Vector2(wi, h));

        }

        private bool Body_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            if(sender.CollisionGroup != other.CollisionGroup )
            {
                Vector2 pushForce = other.Body.Position - sender.Body.Position;
                pushForce.Normalize();
                pushForce = pushForce * 100000;

            }
            if(other.CollisionGroup == 5)
            {
                SFX.NonHurt.Play();
            }
            return true;
        }

        public override void Update()
        {
            if(image == null)
            {
                Position = body.Position;
            }
            //Position = body.Position;
            if(canMove)
            {
                TestInput();
            }
            
        }

        public void TestInput()
        {
            bodyRotateDelay++;

            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                body.LinearVelocity = new Vector2(-moveSpeed, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                body.LinearVelocity = new Vector2(moveSpeed, 0);
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.K) )
            {
                body.LinearVelocity = new Vector2(0, moveSpeed);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                body.LinearVelocity = new Vector2(0, -moveSpeed);

            }
            else
            {
                body.LinearVelocity = new Vector2(0,0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                if(bodyRotateDelay > 15)
                {
                    body.Rotation+=.2f;
                    bodyRotateDelay = 0;
                    r = body.Rotation;
                }
            }


            Vector2 x = body.Position; 
            if (Keyboard.GetState().IsKeyDown(Keys.A) && bodyRotateDelay > 2)
            {
                bodyRotateDelay = 0;
                GameRoot.RemoveBody(body);
                wi--;
                body = world.CreateRectangle(wi, h, 1, x);
                body.BodyType = BodyType.Kinematic;
                body.IgnoreGravity = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && bodyRotateDelay > 2)
            {
                bodyRotateDelay = 0;
                GameRoot.RemoveBody(body);
                wi++;
                 body = world.CreateRectangle(wi, h, 1, x);
                body.BodyType = BodyType.Kinematic;
                body.IgnoreGravity = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && bodyRotateDelay > 2)
            {
                bodyRotateDelay = 0;
                GameRoot.RemoveBody(body);
                h++;
                body = world.CreateRectangle(wi, h, 1, x);
                body.BodyType = BodyType.Kinematic;
                body.IgnoreGravity = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && bodyRotateDelay > 2)
            {
                bodyRotateDelay = 0;
                GameRoot.RemoveBody(body);
                h--;
                body = world.CreateRectangle(wi, h, 1, x);
                body.BodyType = BodyType.Kinematic;
                body.IgnoreGravity = true;
                body.Rotation = r;

            }



        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            string curPos = body.Position.ToString() +" " + " W: " +wi + " H: "+h ;
            curPos += "  R:" + body.Rotation;
            //R is 1.6 for horizontal 
            base.Draw(spriteBatch);
            if(canMove && GameRoot.isDebugMode)
            {
            spriteBatch.DrawString(GameRoot.sf, curPos, body.WorldCenter, Color.Green);
            }

        }

        protected virtual void CreateWalls(Vector2 bp, Vector2 scale)
        {
            Wall right = new Wall(world, new Vector2(bp.X + scale.X/2, bp.Y), 1, scale.Y * .9f);
            Wall left = new Wall(world, new Vector2(bp.X - scale.X / 2, bp.Y), 1, scale.Y * .9f);
            EntityManager.Add(right);
            EntityManager.Add(left);
        }
    }
}
