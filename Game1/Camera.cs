using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;

namespace Game1
{
    public class Camera
    {

        public Matrix transform;
        Viewport view;
        public static Vector2 center;
        Vector2 cacheStartCameraPosition; 

        public Camera(Viewport newView)
        {
            view = newView;
            center = new Vector2(GameRoot.cameraPosition.X, 0);
            cacheStartCameraPosition = center; 
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
            Matrix.CreateTranslation(new Vector3(-center.X + GameRoot.cameraPosition.X, -center.Y, 0));
        }

        public void Update()
        {


            if (GameRoot.canXCameraMove && GameRoot.Player1.Position.X < BackgroundManager.currBG.Bounds.Right * .45 && GameRoot.Player1.Position.X > BackgroundManager.currBG.Bounds.Right * .3)
            {
            center = new Vector2(GameRoot.Player1.bodyPos.X, 0 );
            transform = Matrix.CreateScale(new Vector3(1, 1, 0) )*
                Matrix.CreateTranslation(new Vector3(-center.X + GameRoot.cameraPosition.X, -center.Y, 0));
            }
            if (GameRoot.canYCameraMove)
            {
                center = new Vector2(0, GameRoot.Player1.bodyPos.Y);
                transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                    Matrix.CreateTranslation(new Vector3(-cacheStartCameraPosition.X + GameRoot.cameraPosition.X, -center.Y + GameRoot.cameraPosition.Y, 0));
            }




        }


        public void ResetCameraPosAfterPrevLevel()
        {
            center = cacheStartCameraPosition;
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
            Matrix.CreateTranslation(new Vector3(-center.X + GameRoot.cameraPosition.X, -center.Y, 0));
        }


    }
}
