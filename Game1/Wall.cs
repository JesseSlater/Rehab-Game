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
    class Wall: Floor
	{
        public Wall(World w, Vector2 pos, float width, float height): base(w, pos, width, height)
        {
            body.SetCollisionGroup(6);
        }

        protected override void CreateWalls(Vector2 bp, Vector2 scale)
        {
            return;
        }
    }
}
