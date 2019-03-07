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
    public class Player : Entity
    {
        
        // Health goes in this block
        public float maxHealth = 6f;
        public float currHealth = 6; 
        public float HealthPercent
        {
            get
            {
                return currHealth / maxHealth;
            }
        }
        public bool isDefeated = false; 
        public Color hurtColor = Color.Red; 
        protected const float REMOVETIME = 300;
        protected bool isInvincible = false;
        protected float InvincibilityTime = 160f;
        protected float invincibilityTimer;



        // This Block is for things related to animation

        public bool isGrounded = false;
        protected Texture2D baseImage;
        protected Texture2D[] pics = new Texture2D[8];
        protected int currTex;
        protected int texCylcer = 0;
        protected enum AnimStates { Still, Walking, Flapping};
        protected AnimStates currState;
        protected Vector2 offset;
        protected float animationTimer;
        protected Vector2 imagePos;
        private float stepAnimRate = 8;
        private float flapAnimRate = 4;

    

       // Variables related to controls go here
        protected float controlRemoveTimer;
        protected float removeTimer;
        public enum ActorType { Player, Enemy, Object};
        public ActorType thisActorType;
        public bool canBounce = false;
        public Vector2 bodyPos;
        public bool isPlayerOne = true;
        public int pressForceNumber = 600;
        enum LoadCellState { ButtonHeld, ButtonPressed, ButtonReleased };
        LoadCellState currButtonState = LoadCellState.ButtonReleased;
        protected bool canControl = true;
        protected float pressCoolTime = 30f;
        protected float pressCoolTimer;
        protected float buttonPressTime = 5;
        protected float buttonPressTimer;
        protected const float CONTROLREMOVETIME = 45f;


        
        
        // Everything relating to forces/movement goes in this blcok
        protected float maxHorizVelocity = 125f;
        protected float jumpSpeed = 100f;
        protected float actorCollisionForceMagnitude = 500000;
        Vector2 tForce = new Vector2(6000, -50000);





        private static Player instance;
        public static Player Instance
        {
            get
            {
                if (instance == null)
                    instance = new Player();

                return instance;
            }
        }



        protected bool isPlayer = true; 

        private Player()
        {
            image = Art.PlayerStand;
            baseImage = Art.PlayerBase;
            Position = GameRoot.ScreenSize / 2;
        }


        public Player(World w, Vector2 Pos)
        {
            Position = Pos;
            world = w;
            controlRemoveTimer = CONTROLREMOVETIME;
            SetUpArtAssets();
            SetUpBody();
            AddHitAndHurtBoxes(w);
           
            


        }

        protected virtual void SetUpBody()
        {
            body = world.CreateRectangle(baseImage.Width, baseImage.Height * .75f, 1, Position);

            body.FixedRotation = true;
            body.BodyType = BodyType.Dynamic;
            AddCollisionAndSeprationToBody();
            body.SetCollisionGroup(5);
        }

        protected void AddCollisionAndSeprationToBody()
        {
            body.OnCollision += Player_body_OnCollision;
            body.OnSeparation += Body_OnSeparation; ;
        }

        protected virtual void AddHitAndHurtBoxes(World w)
        {
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 10), new Vector2(0, 0)));
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 5), new Vector2(25, -8)));
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 5), new Vector2(-25, -5)));
            EntityManager.Add(new Hitbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 3), new Vector2(0, 57)));
            if(LevelManager.IsBonusLevel)
            {
                EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 1), new Vector2(0, 55)));
            }
        }


        public override void Update()
        {

            if (PerformDefeatBehaviors() == false)
            {
                RelocateIfOutsideScreen();
                bodyPos = body.Position;

                ManageInput();
                UpdateTimers();
                DetermineGroundStateBasedOnCurrentVelocity();
                AnimationBehaviors();

                DetermineIfInvincibleAndColor();

                DrawBehaviors();
            }
            else
            {
                return;
            }
           

        }

        protected virtual bool PerformDefeatBehaviors()
        {
            if (currHealth <= 0)
            {
                removeTimer++;
                isDefeated = true;
                controlRemoveTimer = 0;
                flipped = SpriteEffects.FlipVertically;
                Position = new Vector2(Position.X, Position.Y +2.5f);
                SynchImageAndRealPos();
                Debug.WriteLine(Position);
                if(body != null)
                    DestroyBody();
                if(REMOVETIME < removeTimer)
                {
                    FinalBehaviorsBeforeRemoval();
                }
                return true;
            }
            return false; 
        }

        protected virtual void FinalBehaviorsBeforeRemoval()
        {
            IsExpired = true;
            if (LevelManager.IsBonusLevel)
            {
                LevelManager.LoadNextLevel();
                return;
            }

            LevelManager.ReloadLevel();
        }

        protected virtual void ManageInput()
        {
            if (controlRemoveTimer < CONTROLREMOVETIME)
            {
                return;
            }

            if (!MotionDataReciever.isUsingMotionControls)
                ManageKeyInput(maxHorizVelocity);
            else
                ManageMotionInput(maxHorizVelocity);
        }

        private void SynchImageAndRealPos()
        {
            imagePos = Position;
        }

        protected void DetermineIfInvincibleAndColor()
        {
            if (invincibilityTimer > InvincibilityTime)
            {
                invincibilityTimer = 0;
                isInvincible = false;
                color = Color.White;
            }
        }

        protected void AnimationBehaviors()
        {
            if (currState == AnimStates.Walking)
            {
                PlayWalk();
            }
            if (currState == AnimStates.Flapping)
            {
                PlayFlap();
            }
            if (currState == AnimStates.Still)
            {
                currTex = 0;
            }
        }

        protected void RelocateIfOutsideScreen()
        {
            if(!GameRoot.willTeleportIfOffscreen)
            {
                return;
            }

            if (body.Position.X > GameRoot.ScreenBoundRight + pics[currTex].Width)
            {
                body.Position = new Vector2(-pics[currTex].Width / 2, body.Position.Y);
            }
            if (body.Position.X < 0 - pics[currTex].Width)
            {
                body.Position = new Vector2(GameRoot.ScreenBoundRight + pics[currTex].Width / 2, body.Position.Y);
            }

        }

        protected void UpdateTimers()
        {
            pressCoolTimer++;
            animationTimer++;
            buttonPressTimer++;
            invincibilityTimer++;
            controlRemoveTimer++;
        }

        protected void DetermineGroundStateBasedOnCurrentVelocity()
        {
            if (isGrounded)
            {
                if (body.LinearVelocity.X > 40 || body.LinearVelocity.X < -40)
                {

                    currState = AnimStates.Walking;
                }
                else
                {
                    currState = AnimStates.Still;
                }
            }
        }

        protected virtual void ManageKeyInput(float speed)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                MoveLeft(speed);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                MoveRight(speed);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && buttonPressTime < buttonPressTimer)
            {
                Jump();

            }

            //Remember to take this out
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                body.LinearVelocity = new Vector2(0, 0);
            }


        }

        protected void Jump()
        {
            currState = AnimStates.Flapping;
            DoJump(0, jumpSpeed);
        }

        protected void MoveLeft(float speed)
        {
            flipped = SpriteEffects.FlipHorizontally;
            isFlipped = true;
            body.LinearVelocity = new Vector2(-speed, body.LinearVelocity.Y);
        }

        protected void MoveRight(float speed)
        {
            flipped = SpriteEffects.None;
            isFlipped = false;
            body.LinearVelocity = new Vector2(speed, body.LinearVelocity.Y);
        }

        protected virtual void ManageMotionInput(float speed)
        {
            body.LinearVelocity = new Vector2(-speed * MotionDataReciever.GetPlayerMotionInput(isPlayerOne).Y, body.LinearVelocity.Y);
            if(body.LinearVelocity.X < 0)
            {
                flipped = SpriteEffects.FlipHorizontally;
                isFlipped = true;
            }
            else if(body.LinearVelocity.X > 0)
            {
                flipped = SpriteEffects.None;
                isFlipped = false;
            }

            DetermineButtonState();
            if (currButtonState == LoadCellState.ButtonPressed)
            {
                currState = AnimStates.Flapping;
                DoJump(0, speed);
            }
        }

        public void DoJump(float overrideXVel, float yVel)
        {
            buttonPressTimer = 0;
            if (overrideXVel != 0)
            {
                if(body.LinearVelocity.X < 0)
                {
                    overrideXVel = -overrideXVel;
                }
                body.LinearVelocity = new Vector2(overrideXVel, yVel);
            }
            body.LinearVelocity = new Vector2(body.LinearVelocity.X, -yVel);
        }


        private bool Player_body_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            if(other.CollisionGroup == sender.CollisionGroup)
            {
                DoHorizantalBounce(sender, other);
            }

            if (other.CollisionGroup == 0 &&  body.Position.Y  < other.Body.Position.Y)
            {
                isGrounded = true;
                DoJump(0, -jumpSpeed * .5f);
            }


            if (other.CollisionGroup == 6)
            {
                DoHorizantalBounce(sender, other);
            }
            if(other.CollisionGroup == GetOpposedCollisionGroup(true))
            {
                //DoHorizantalBounce(sender, other);
            }


            return true;



        }

        protected void DoHorizantalBounce(Fixture sender, Fixture other)
        {
            controlRemoveTimer = 0;
            if (sender.Body.Position.X > other.Body.Position.X)
            {
                body.ApplyLinearImpulse(new Vector2(actorCollisionForceMagnitude, 0));
            }
            else
            {
                body.ApplyLinearImpulse(new Vector2(-actorCollisionForceMagnitude, 0));
            }
        }

        protected void DoHorizantalBounce(Fixture sender, Fixture other, float timeWithoutControl)
        {
            controlRemoveTimer = (REMOVETIME - timeWithoutControl);
            if (sender.Body.Position.X > other.Body.Position.X)
            {
                body.ApplyLinearImpulse(new Vector2(actorCollisionForceMagnitude, 0));
            }
            else
            {
                body.ApplyLinearImpulse(new Vector2(-actorCollisionForceMagnitude, 0));
            }
        }


        private void Body_OnSeparation(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            if (other.CollisionGroup == 0)
            {
                isGrounded = false;
                if(currState == AnimStates.Walking)
                {
                    currState = AnimStates.Still;
                }
            }

           
        }

        public virtual short GetCollisionGroup(bool isGettingHurtbox)
        {
            if(isGettingHurtbox)
            {
                return GameRoot.PlayerHurtBoxCollisionGroup;
            }
            else
            {
                return GameRoot.PlayerHurtBoxCollisionGroup;
            }
        }

        public virtual short GetOpposedCollisionGroup(bool isGettingHurtBox)
        {
            if (isGettingHurtBox)
            {
                return GameRoot.EnemyHurtBoxCollisionGroup;
            }
            else
            {
                return GameRoot.EnemyHitBoxCollisionGroup;
            }
        }


        protected virtual void DrawBehaviors()
        {
            if (isInvisible)
            {
                return;
            }
            image = pics[currTex];
            ManageOffSet();
            Position = new Vector2(body.Position.X - image.Width / 2 + offset.X, body.Position.Y + image.Height / 2 + offset.Y);
            SynchImageAndRealPos();




        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (image != null && !isInvisible)
                spriteBatch.Draw(image, imagePos, null, color, Orientation, new Vector2(baseImage.Width/2, baseImage.Height/2), 1f, flipped, 0);

            //string curPos = body.Position.ToString();
            string curPos = " " + GameRoot.ScreenSize.X + " " + GameRoot.ScreenSize.Y;
            if (GameRoot.isDebugMode)
            {
                spriteBatch.DrawString(GameRoot.sf, curPos, body.WorldCenter, Color.Red);
            }


        }


        protected virtual void PlayWalk()
        {
            
            if(animationTimer > stepAnimRate)
            {
                animationTimer = 0;
                if (texCylcer == 0)
                {
                    texCylcer++;
                    currTex = 0;   
                }
                else if(texCylcer == 1)
                {
                    SFX.Step.Play();
                    texCylcer++;
                    currTex = 1;
                }
                else if (texCylcer == 2)
                {
                    texCylcer++;
                    currTex = 0;
                }
                else if (texCylcer == 3)
                {
                    SFX.Step.Play();
                    texCylcer = 0;
                    currTex = 2;
                }
            }
        }


        protected virtual void PlayFlap()
        {

            if (animationTimer > flapAnimRate)
            {
                animationTimer = 0;
                if (texCylcer == 0)
                {
                    texCylcer++;
                    currTex = 0;
                }
                else if (texCylcer == 1)
                {
                    SFX.Flap.Play();
                    texCylcer++;
                    currTex = 3;
                }
                else if (texCylcer == 2)
                {
                    texCylcer++;
                    currTex = 4;
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
                    currState = AnimStates.Still;
                }
            }
        }



        protected virtual void SetUpArtAssets()
        {
            image = Art.PlayerStand;
            baseImage = Art.PlayerBase;
            offset = new Vector2(0, 0);

            pics[0] = image;
            pics[1] = Art.PlayerWalk1;
            pics[2] = Art.PlayerWalk2;
            pics[3] = Art.PlayerFlap1;
            pics[4] = Art.PlayerFlap2;

        }

        protected virtual void ManageOffSet()
        {
            int caseSwitch = 0;
            if (flipped == SpriteEffects.FlipHorizontally)
            {
                caseSwitch = 5;
            }
            caseSwitch += currTex;
            switch (caseSwitch)
            {
                case 0: //Standing
                    offset = new Vector2(4,-5);
                    break;                   
                case 1: //Walk 1
                    offset = new Vector2(4,-2);
                    break;
                case 2: //Walk 2
                    offset = new Vector2(4,-3);
                    break;
                case 3: //Flap 1
                    offset = new Vector2(8,-6);
                    break;
                case 4: //Flap 2
                    offset = new Vector2(8,-6);
                    break;
                case 5:
                    offset = new Vector2(19, -5);
                    break;
                case 6:
                    offset = new Vector2(19, -2);
                    break;
                case 7:
                    offset = new Vector2(19, -3);
                    break;
                case 8:
                    offset = new Vector2(15, -6);
                    break;
                case 9:
                    offset = new Vector2(15, -6);
                    break;
            }


        }

        public virtual void TakeDamage()
        {
            if (!isInvincible)
            {
                color = hurtColor;
                isInvincible = true;
                currHealth--;
                SFX.Hurt.Play();

            }
            else
            {
                SFX.NonHurt.Play();
            }
        }


        void DetermineButtonState()
        {
            //int press = GetLoadCellDataFromPuck();
            int press = MotionDataReciever.GetLoadCellDataFromPuck(isPlayerOne);

            if (press > pressForceNumber)
            {
                if (currButtonState == LoadCellState.ButtonReleased)
                {
                    currButtonState = LoadCellState.ButtonPressed;
                }
                else
                {
                    currButtonState = LoadCellState.ButtonHeld;
                }
            }
            else
            {
                currButtonState = LoadCellState.ButtonReleased;
            }
        }
    }
}
