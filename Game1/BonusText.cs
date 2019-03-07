using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using System.Diagnostics;

namespace Game1
{
    class BonusText : Entity
    {
        private int colorChanger = 0;
        public bool isCountingUp = true;

        public BonusText(World w, Vector2 Pos)
        {
            Position = Pos;
            image = Art.BonusText;
        }


        public override void Update()
        {
            if (colorChanger > 254)
            {
                isCountingUp = false;
            }
            if (colorChanger <= 0)
            {
                isCountingUp = true;
            }
            if (isCountingUp)
                colorChanger += 3;
            else
                colorChanger -= 3;



        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, new Color(colorChanger, 0,255), 0, new Vector2(1, 1), 1f, flipped, 0);
        }
    }
    }
