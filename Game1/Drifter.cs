using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using System.Diagnostics;


namespace Game1
{
    class Drifter : Interactable
    {
        public bool isStationary; 
        public Vector2 driftDirection = new Vector2(30,0);
        private Vector2 startPos;
        Random rn = new Random();
        Color r; 
        public Drifter(World w, Vector2 Pos) : base(w, Pos)
        {
           // world.Remove(body);
            //body = world.CreateRectangle(image.Width, image.Height / 2, 1, bodyPos);
            body.IgnoreGravity = true;
            if(driftDirection != new Vector2(0,0))
                driftDirection.Normalize();
            body.LinearVelocity = driftDirection * 30f;
            controlRemoveTimer = CONTROLREMOVETIME;
            startPos = Pos;
            r = new Color(rn.Next(255), rn.Next(255), rn.Next(255));
        }


        public override void Update()
        {

            controlRemoveTimer++;
            if(driftDirection != new Vector2(0,0))
                driftDirection.Normalize();
            else
            {
                body.Position = startPos;
            }
            if(controlRemoveTimer > CONTROLREMOVETIME)
                body.LinearVelocity = driftDirection * 30f;
            base.Update();
            if(!isBeingDestroyed)
            {
                PlayFloat();
            }
            Position = body.Position;
        }
        protected override void SetUpArtAssets()
        {
            image = Art.Baloon1;
            baseImage = Art.Baloon1;
            baseOffset = new Vector2(8,-10);

            pics[0] = image;
            pics[1] = Art.Baloon2;
            pics[2] = Art.Baloon3;
            pics[3] = Art.Baloon4;
            pics[4] = Art.Baloon5;
            pics[5] = Art.Baloon6;
            pics[6] = Art.Baloon7;
            pics[7] = Art.Baloon8;



        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            texCylcer = 0;
        }

        protected virtual void PlayFloat()
        {
            if (animationTimer > destroyStepRate)
            {
                texCylcer = rn.Next(4);
                animationTimer = 0;
                if (texCylcer == 0)
                {
                    currTex = 0;
                }
                else if (texCylcer == 1)
                {
                    currTex = 1;
                }
                else if (texCylcer == 2)
                {
                    currTex = 2;
                }
                else if (texCylcer == 3)
                {
                    currTex = 3;
                }



            }
        }


        protected override void PlayDestroy()
        {
            if (animationTimer > destroyStepRate)
            {
                animationTimer = 0;
                if (texCylcer == 0)
                {
                    texCylcer++;
                    currTex = 5;
                }
                else if (texCylcer == 1)
                {
                    texCylcer++;
                    currTex = 6;
                }
                else if (texCylcer == 1)
                {
                    texCylcer++;
                    currTex = 7;
                }
                else if (texCylcer == 2)
                {
                    texCylcer = 0;
                    currTex = 0;
                    isDefeated = true;
                }
            }
        }

        protected override void AddHitAndHurtBoxes(World w)
        {
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width,baseImage.Height*.4f), new Vector2(0, 20)));
            body.SetFriction(10000);
            body.Mass = 1000000f;
        }

        protected override void SetUpBody()
        {
            body = world.CreateRectangle(1, baseImage.Height * .1f, 1, Position);

            body.FixedRotation = true;
            body.BodyType = BodyType.Dynamic;
            AddCollisionAndSeprationToBody();
            body.SetCollisionGroup(5);
            body.OnCollision += Body_OnCollision;
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            if(other.CollisionGroup == 0)
            {
                isBeingDestroyed = true; 
            }


                return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (image != null && !isInvisible)
                spriteBatch.Draw(image, imagePos + offset, null, r, Orientation, new Vector2(0, 0), 1f, flipped, 0);

        }
    }
}
