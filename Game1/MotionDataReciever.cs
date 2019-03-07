using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FitMi_Research_Puck;
using HidSharp;

namespace Game1
{
    class MotionDataReciever
    {
        public static MotionDataReciever instance = null;
        public static HIDPuckDongle puck;
        public static bool isUsingMotionControls = false;


        public MotionDataReciever()
        {
           // AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            puck = new HIDPuckDongle();

            if (isUsingMotionControls && !puck.IsOpen)
            {
                puck.Open();
            }


        }



        public static Vector3 GetPlayerMotionInput(bool isPlayerOne)
        {
            puck.CheckForNewPuckData();
            if (isPlayerOne)
            {
                int x = puck.PuckPack0.Gyrometer[0];
                int y = puck.PuckPack0.Gyrometer[1];
                int z = puck.PuckPack0.Gyrometer[2];
                Vector3 motionVector = new Vector3(x, y, z);
                motionVector = Vector3.Normalize(motionVector);
                return motionVector;
            }
            else
            {
                int x = puck.PuckPack1.Gyrometer[0];
                int y = puck.PuckPack1.Gyrometer[1];
                int z = puck.PuckPack1.Gyrometer[2];
                Vector3 motionVector = new Vector3(x, y, z);
                motionVector = Vector3.Normalize(motionVector);
                return motionVector;

            }


        }



        public static int GetLoadCellDataFromPuck(bool isPlayerOne)
        {
            puck.CheckForNewPuckData();
            int loadData;
            if (isPlayerOne)
            {
                loadData = puck.PuckPack0.Loadcell;
            }
            else
            {
                loadData = puck.PuckPack1.Loadcell;
            }
            return loadData;
        }



        public static void OnApplicationQuit()
        {
            //Resources.UnloadAsset(MotionDataReciever.instance);
            if (isUsingMotionControls && puck.IsOpen)
            {
                puck.ReceivingData = false;
                puck.Stop();
                puck.Close();
                puck.ReceivingData = false;

            }

        }
    }
}
