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
    class Hitbox : Entity
    {
        Player thisActor;
        Vector2 playerPos;
        Body playerBody;
        float height; 
        Vector2 offset;
        short hurtboxGroup;
        bool isUnatached = false;
        Vector2 hPos;

        public Hitbox(Player actor, Body pBody, World curWorld, Vector2 baseImageSize, Vector2 boxSize, Vector2 offset)
        {
            SetWorld(curWorld);
            thisActor = actor;
            playerPos = actor.Position;
            playerBody = pBody;
            this.height = baseImageSize.Y;
            body = curWorld.CreateRectangle(boxSize.X, boxSize.Y, 1, new Vector2(pBody.Position.X + offset.X, pBody.Position.Y - height / 2 + offset.Y));
            this.offset = offset;
            body.SetFriction(0);
            body.SetCollisionGroup(actor.GetCollisionGroup(false));
            hurtboxGroup = actor.GetCollisionGroup(true);
            body.OnCollision += Body_OnCollision;
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;

        }


        public Hitbox(World curWorld, Vector2 pos, Vector2 size)
        {
            SetWorld(curWorld);
            body = curWorld.CreateRectangle(size.X, size.Y, 1, new Vector2(pos.X, pos.Y));
            body.SetFriction(0);
            body.SetCollisionGroup(4);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            isUnatached = true;
            body.IgnoreGravity = true;
            hPos = pos;
            

        }

        private bool Body_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {
            if (other.CollisionGroup == thisActor.GetOpposedCollisionGroup(true))
            {
                thisActor.DoJump(100000, 100000);
            }
            if (other.CollisionGroup == thisActor.GetCollisionGroup(true) )
            {
                thisActor.DoJump(100000, 100000);
            }
            return true;
        }

        public override void Update()
        {
            if (isUnatached)
            {
                body.Position = hPos;
                return;
            }

            body.Position = new Vector2(playerBody.Position.X + offset.X, playerBody.Position.Y - height / 2 - 10 + offset.Y);
            if(thisActor.isDefeated)
            {
                DestroyBody();
            }
        }
    }
}


