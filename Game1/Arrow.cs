using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Arrow : Entity
    {
        int colorChanger = 0;
        float angle;
        private bool isCountingUp = true;
        public bool isBonusClock = false;
        public Arrow(World w, Vector2 Pos, float angle)
        {
            Position = Pos;
            image = Art.arrow;
            this.angle = angle;
        }
        public Arrow(World w, Vector2 Pos)
        {
            Position = Pos;
            image = Art.arrow;
            this.angle = 0;
        }

        public override void Update()
        {
            if(colorChanger > 254)
            {
                isCountingUp = false;
            }
            if(colorChanger <= 0)
            {
                isCountingUp = true;
            }
            if (isCountingUp)
                colorChanger+=3;
            else
                colorChanger-=3;

            if(isBonusClock)
            {
                flipped = SpriteEffects.FlipVertically;
                angle = (LevelManager.GetRemainingTimeInBonusLevel() * 6.2f);
                
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle d = new Rectangle((int) Position.X, (int) Position.Y, image.Width, image.Height);
            Vector2 origin = new Vector2(d.Width / 2, d.Height / 2);
      
            spriteBatch.Draw(image, d, null, Color.White, angle, origin, SpriteEffects.None, 0);
            if (isBonusClock)
            {
                spriteBatch.Draw(Art.ClockBack, d, null, Color.White, 0, new Vector2(origin.X + Art.ClockBack.Width/2 , origin.Y + Art.ClockBack.Height / 2), SpriteEffects.None, 0);
            }
            //spriteBatch.Draw(image, Position, null, new Color(colorChanger, 0,255), angle, new Vector2(1, 1), 1f, flipped, 0);
        }
    }
}
