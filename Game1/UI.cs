using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
namespace Game1
{
    class UI
    {
        GraphicsDeviceManager graphics;
        Vector2 heartScale = new Vector2(1f, 1f);

        public UI()
        {
            graphics = GameRoot.graphics;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(0, 0);
   
            var m = Matrix.CreateOrthographicOffCenter(0,
                graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                0, 0, 1
            );

            var a = new AlphaTestEffect(graphics.GraphicsDevice)
            {
                Projection = m
            };

            var s1 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            var s2 = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.LessEqual,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };
           // spriteBatch.Draw(Art.HealthBlock, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), null, null, null, 0, new Vector2(1f, 1f));//The Background  
           

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, s1, null, a);
            spriteBatch.Draw(Art.HeartInterior, position, null, null, null, 0, heartScale);//The Interior                                   
            spriteBatch.End();

            if(GameRoot.Player1 == null)
            {
                return;
            }
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, s2, null, a);
            spriteBatch.Draw(Art.HealthBlock, position, null, null, null, 0, new Vector2(heartScale.X * GameRoot.Player1.HealthPercent, heartScale.Y));//The Background  
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null);
            spriteBatch.Draw(Art.Heart, position, null, null, null, 0, heartScale);//The Stencil       
            spriteBatch.End();
        }
    }
}
