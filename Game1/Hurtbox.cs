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
    class Hurtbox : Entity
    {
        Player thisActor;
        Vector2 playerPos;
        Body playerBody;
        float height;
        Vector2 offset;
        short hitboxGroup;



        public Hurtbox(Player actor, Body pBody, World curWorld, Vector2 baseImageSize, Vector2 boxSize, Vector2 offset)
        {
            SetWorld(curWorld);
            thisActor = actor;
            playerPos = actor.Position;
            playerBody = pBody;
            this.height = baseImageSize.Y;
            body = curWorld.CreateRectangle(boxSize.X, boxSize.Y, 1, new Vector2(pBody.Position.X + offset.X, pBody.Position.Y - height/2 + offset.Y));
            this.offset = offset;
            body.SetFriction(0);
            body.OnCollision += Body_OnCollision;
            body.BodyType = BodyType.Static;
            body.SetCollisionGroup(actor.GetCollisionGroup(true));
            hitboxGroup = actor.GetCollisionGroup(false);
            body.IgnoreGravity = true;
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
        {

            if(other.CollisionGroup == thisActor.GetOpposedCollisionGroup(false))
            {
                thisActor.TakeDamage();               
            }
            return true; 
        }

        public override void Update()
        {
            body.Position = new Vector2(playerBody.Position.X + offset.X, playerBody.Position.Y - height/2 - 10 + offset.Y);
            if (thisActor.isDefeated)
            {
                DestroyBody();
            }
        }
    }
}
