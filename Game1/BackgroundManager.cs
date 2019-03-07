using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Game1
{
    static class BackgroundManager
    {
        public static Texture2D currBG = null;
        private static Vector2 pos = new Vector2(0f,0f);
        private static Color currColor;
        private static Vector2 currScale = new Vector2(1f,1f); 
        public static void SetBG(Texture2D t)
        {
            currBG = t;
            pos = new Vector2(0, 0);
            currColor = Color.White;
        }
        public static void SetBG(Texture2D t, Vector2 scale)
        {
            currBG = t;
            currScale = scale;
            currColor = Color.White;
        }
        public static void SetBG(Texture2D t, Vector2 position, Vector2 scale)
        {
            currBG = t;
            pos = position;
            currScale = scale;
            currColor = Color.White;
        }
        public static void SetBG(Texture2D t, Vector2 scale, Color c)
        {
            currBG = t;
            currScale = scale;
            currColor = c;
        }
        public static void SetBG(Texture2D t, Vector2 position, Vector2 scale, Color c)
        {
            currBG = t;
            pos = position;
            currScale = scale;
            currColor = c;
        }

        public static void SetBG(Texture2D t, Vector2 position, Vector2 scale, Color c, Color bc)
        {
            currBG = t;
            pos = position;
            currScale = scale;
            currColor = c;
            GameRoot.SetBGColor(bc);
        }

        public static void Draw(SpriteBatch spritebatch)
        {
            if(currBG != null)
                spritebatch.Draw(currBG, pos, null, null, null, 0f, currScale, currColor, SpriteEffects.None, 0);
        }

    }
}
