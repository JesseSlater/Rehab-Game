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
    class Interactable : Enemy
    {
        protected int destroyStepRate = 8;
        protected bool isBeingDestroyed;
        protected Vector2 baseOffset = new Vector2(20, -6);
        public bool isDebug = false;
        private float moveSpeed = 25f;
        private MouseState lastMouseState;
        private MouseState currentMouseState;
        private int timesClicked;
        public Interactable(World w, Vector2 Pos) : base(w, Pos)
        {
            isPlayer = false;
            currHealth = 1;
        }


        public override void Update()
        {
            isInvisible = isDefeated;
            animationTimer++;
            DrawBehaviors();


            //isInvisible = isDefeated;
            if (isBeingDestroyed)
            {
                PlayDestroy();
            }
            if(isDefeated)
            {
                PerformDefeatBehaviors();
            }

            if(isDebug)
            {
                TestInput();
                body.IgnoreGravity = true;
            }
            
        }


        protected override void SetUpArtAssets()
        {
            image = Art.BoxAnim1;
            baseImage = Art.BoxAnim1;
            offset = new Vector2(0, 0);

            pics[0] = image;
            pics[1] = Art.BoxAnim2;
            pics[2] = Art.BoxAnim3;
            pics[3] = Art.BoxAnim4;
            


        }

        protected override void AddHitAndHurtBoxes(World w)
        {
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 1f), new Vector2(0, 8)));
            body.SetFriction(10000);
            body.Mass = 1000000f;
            
        }

        protected override void ManageOffSet()
        {
            offset = new Vector2(baseOffset.X - (baseImage.Width - pics[currTex].Width)/2, baseOffset.Y + (baseImage.Height - pics[currTex].Height) );
        }

        public override void TakeDamage()
        {
            SFX.BoxDestroy.Play();
            isBeingDestroyed = true;
        }

        protected virtual void PlayDestroy()
        {
            if (animationTimer > destroyStepRate)
            {
                animationTimer = 0;
                if (texCylcer == 0)
                {
                    texCylcer++;
                    currTex = 0;
                }
                else if (texCylcer == 1)
                {
                    texCylcer++;
                    currTex = 1;
                }
                else if (texCylcer == 2)
                {
                    texCylcer++;
                    currTex = 2;
                }
                else if (texCylcer == 3)
                {
                    texCylcer++;
                    currTex = 3;
                }
                else if (texCylcer == 4)
                {
                    texCylcer = 0;
                    currTex = 0;
                    isDefeated = true;
                }
            }
        }

        protected override void SetUpBody()
        {
            body = world.CreateRectangle(baseImage.Width, baseImage.Height * .9f, 1, Position);

            body.FixedRotation = true;
            body.BodyType = BodyType.Dynamic;
            AddCollisionAndSeprationToBody();
            body.SetCollisionGroup(5);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
           
            if (image != null && !isInvisible)
                spriteBatch.Draw(image, imagePos + offset, null, color, Orientation, new Vector2(0 ,0), 1f, flipped, 0);




        }

        protected override void DrawBehaviors()
        {
             image = pics[currTex];
            Position = body.Position;
            bodyPos = body.Position;
            ManageOffSet();
            imagePos = new Vector2(bodyPos.X - image.Width, bodyPos.Y + image.Height);
        }


        protected override bool PerformDefeatBehaviors()
        {
                removeTimer++;
                if (body != null)
                    DestroyBody();
                if (REMOVETIME < removeTimer)
                {
                    FinalBehaviorsBeforeRemoval();
                    return true;
                }
                return false;
            
           
        }


        public virtual void TestInput()
        {



            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                body.LinearVelocity = new Vector2(-moveSpeed, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                body.LinearVelocity = new Vector2(moveSpeed, 0);
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                body.LinearVelocity = new Vector2(0, moveSpeed);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                body.LinearVelocity = new Vector2(0, -moveSpeed);

            }
            else
            {
                body.LinearVelocity = new Vector2(0, 0);
            }





        }

    }
}
