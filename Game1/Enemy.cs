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
    class Enemy : Player
    {
        public bool isLessAgressive = false; 
        private bool isTestingKeyboardControls = true;
        private bool isTestingMotionControls = false;

        protected const int UPDATE_POS_TIME = 120;
        protected int updateTimer;

        protected const int FLAP_COOLDOWN = 40;
        protected int flapCooldownTimer = 0;

        protected Vector2 playerPos;


        private const int PATROLE_DISTANCE = 250;

        private bool isMovingRight;
        

        public Enemy(World w, Vector2 Pos) : base(w, Pos)
        {
            pressCoolTime = 10;
            maxHorizVelocity = 100f;
            GameRoot.EnemiesInLevel++;
            isPlayer = false;
            currHealth = 3;
            updateTimer = UPDATE_POS_TIME;
            jumpSpeed = 100f;
        }


        public override void Update()
        {
            UpdateAITimers();
            base.Update();
        }

        protected override void ManageInput()
        {
            //if(isTestingKeyboardControls)
            //    TestWithKeyboard();
            //if(isTestingMotionControls)
            //{
            //    isPlayerOne = false;
            //    ManageMotionInput(maxHorizVelocity);
            //}

            if (controlRemoveTimer < CONTROLREMOVETIME)
            {
                return;
            }
            AIBehaviors();
        }

        private void TestWithKeyboard()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                MoveLeft(maxHorizVelocity);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                MoveRight(maxHorizVelocity);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && buttonPressTime < buttonPressTimer)
            {
                currState = AnimStates.Flapping;
                DoJump(0, maxHorizVelocity);

            }
        }

        protected override void AddHitAndHurtBoxes(World w)
        {
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width -8, 30), new Vector2(0, 0)));
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 5), new Vector2(baseImage.Width, 2)));
            EntityManager.Add(new Hurtbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 5), new Vector2(-baseImage.Width, 2)));
            EntityManager.Add(new Hitbox(this, body, w, new Vector2(baseImage.Width, baseImage.Height), new Vector2(baseImage.Width, 3), new Vector2(0, 80)));
        }


        public override short GetOpposedCollisionGroup(bool isGettingHurtBox)
        {
            if (isGettingHurtBox)
            {
                return GameRoot.PlayerHurtBoxCollisionGroup;
            }
            else
            {
                return GameRoot.PlayerHurtBoxCollisionGroup;
            }
        }

        public override short GetCollisionGroup(bool isGettingHurtBox)
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


        protected override void SetUpArtAssets()
        {
            image = Art.EnemyStand;
            baseImage = Art.EnemyBase;
            offset = new Vector2(0, 0);

            pics[0] = image;
            pics[1] = Art.EnemyWalk1;
            pics[2] = Art.EnemyWalk2;
            pics[3] = Art.EnemyFlap1;
            pics[4] = Art.EnemyFlap2;

        }



        protected override void ManageOffSet()
        {
            int caseSwitch = 0;
            if (flipped == SpriteEffects.FlipHorizontally)
            {
                caseSwitch = 5;
            }
            caseSwitch += currTex;
            switch (caseSwitch)
            {
                case 0:
                    offset = new Vector2(14, -28);
                    break;
                case 1:
                    offset = new Vector2(17, -28);
                    break;
                case 2:
                    offset = new Vector2(17, -28);
                    break;
                case 3:
                    offset = new Vector2(12, -28);
                    break;
                case 4:
                    offset = new Vector2(16, -28);
                    break;
                case 5:
                    offset = new Vector2(28, -28);
                    break;
                case 6:
                    offset = new Vector2(27, -28);
                    break;
                case 7:
                    offset = new Vector2(27, -28);
                    break;
                case 8:
                    offset = new Vector2(12, -28);
                    break;
                case 9:
                    offset = new Vector2(27, -28);
                    break;
            }
        }
        protected override void FinalBehaviorsBeforeRemoval()
        {
            IsExpired = true;
            GameRoot.EnemiesInLevel--;
        }

        protected virtual void UpdateAITimers()
        {
            updateTimer++;
            flapCooldownTimer++;
            


        }

        protected virtual void AIBehaviors()
        {
            UpdatePlayerLocationIfTime();


            if(GetXDistanceFromPlayer() > GameRoot.ScreenSize.X/2)
            {
                DistBehavior();
            }
            else
            {
                ProxBehavior();
            }

            
        }


        //This checks if X amount of time has passed, updates player location accordingly.
        protected virtual void UpdatePlayerLocationIfTime()
        {
            if(updateTimer > UPDATE_POS_TIME)
            {
                updateTimer = 0;
                GetPlayerLocation();
            }
        }

        //This gets the player location
        private void GetPlayerLocation()
        {
            playerPos = GameRoot.Player1.bodyPos;
        }


        protected  float GetXDistanceFromPlayer()
        {
            float dist = playerPos.X - body.Position.X;
            if(dist < 0)
            {
                return -dist;
            }
            else
            {
                return dist;
            }
        }

        protected bool IsPlayerToRight()
        {
            if (playerPos.X < body.Position.X)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        protected virtual void DistBehavior()
        {
            TrackPlayerAtDist();
        }

        protected virtual void ProxBehavior()
        {
            if(isLessAgressive && GetXDistanceFromPlayer() > GameRoot.ScreenSize.X/4)
            {
                DistBehavior();
                return;
            }

            if(GetYDistanceFromPlayer() > GameRoot.ScreenSize.Y/4)
            {
                DistBehavior();
                return;

            }

            if (IsBelowPlayer())
            {
                TryFlap();
            }
            if (GetXDistanceFromPlayer() < 90)
            {
                return;
            }
            TrackPlayer();

        }

        private void TrackPlayer()
        {
            if (IsPlayerToRight())
            {
                isMovingRight = true;
                MoveRight(maxHorizVelocity);
            }
            else
            {
                isMovingRight = false;
                MoveLeft(maxHorizVelocity);
            }
        }

        private void TrackPlayer(bool wasGoingRight)
        {
            if (wasGoingRight)
            {
                isMovingRight = true;
                MoveRight(maxHorizVelocity);
            }
            else
            {
                isMovingRight = false;
                MoveLeft(maxHorizVelocity);
            }
        }

        private void TrackPlayerAtDist()
        {
            TrackPlayer(isMovingRight);
        }

        protected bool IsBelowPlayer()
        {
            if(bodyPos.Y + baseImage.Height/2 > playerPos.Y)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }


        protected float GetYDistanceFromPlayer()
        {
            float dist = playerPos.Y - body.Position.Y;
            if (dist < 0)
            {
                return -dist;
            }
            else
            {
                return dist;
            }

        }

        protected void TryFlap()
        {
            if(flapCooldownTimer > FLAP_COOLDOWN)
            {
                flapCooldownTimer = 0;
                Jump();
            }
        }



    }


}
