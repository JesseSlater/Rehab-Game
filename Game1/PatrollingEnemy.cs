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
    class PatrollingEnemy : Enemy
    {
        public Arrow instructionArrow;
        public PatrollingEnemy(World w, Vector2 Pos) : base(w, Pos)
        {
            instructionArrow = new Arrow(w, Pos, 0f);
            EntityManager.Add(instructionArrow);
        }


        public override void Update()
        {
            base.Update();
            instructionArrow.Position = new Vector2(this.Position.X + baseImage.Width/2, this.Position.Y - baseImage.Height - 6f);
        }

        protected override void AIBehaviors()
        {
            MoveLeft(maxHorizVelocity);
        }
    }
}
