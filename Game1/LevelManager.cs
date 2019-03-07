using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Game1
{
    static class LevelManager
    {
        static int currLevel = 0;
        public static float LevelTimeLimit = 50000;
        static float LEVEL_NEXT_BUFFER_TIME = 50;
        static int levelTimer;
        public static bool IsBonusLevel = false;
        public static int BonusLevelTime = 0;
        static int BonusLevelTimer = 0;
        public static bool IsEliminationLevel = true;


        public static void LoadLevel(int level)
        {
            IsEliminationLevel = true;
            EntityManager.ClearLevel();
            levelTimer = 0;
            currLevel = level;
            GameRoot.camera.ResetCameraPosAfterPrevLevel();
            switch (level)
            {
                case 0:
                    GameRoot.SetUpLevel1();
                    break;
                case 1:
                    GameRoot.SetUpLevel2();
                    break;
                case 2:
                    GameRoot.SetUpLevel3();
                    break;
                case 3:
                    GameRoot.SetUpLevel4();
                    break;
                case 4:
                    GameRoot.SetUpBonusLevel1();
                    IsEliminationLevel = false;
                    break;
                case 5:
                    GameRoot.SetUpLevel5();
                    break;
                case 6:
                    GameRoot.SetUpLevel6();
                    break;
                case 7:
                    GameRoot.SetUpLevel7();
                    break;
                case 8:
                    GameRoot.SetUpBonusLevel2();
                    break;
                case 9:
                    IsBonusLevel = false;
                    GameRoot.SetUpLevel8();
                    break;
                case 10:
                    GameRoot.SetUpLevel9();
                    break;
                case 11:
                    GameRoot.SetUpLevel10();
                    break;
                default:
                    GameRoot.SetUpLevel1();
                    currLevel = 0;
                    break;

            }

        }

        public static void LoadNextLevel()
        {
            if (levelTimer > LEVEL_NEXT_BUFFER_TIME)
            {
                LoadLevel(currLevel+1);
            }

        }

        public static void ReloadLevel()
        {

            
            LoadLevel(currLevel);
        }


        public static void Update()
        {
            levelTimer++; 
            if(BonusLevelTimer > BonusLevelTime)
            {
                currLevel++;
                IsBonusLevel = false;
                BonusLevelTimer = 0;
                LoadNextLevel(); 
                return;
            }
            if(IsBonusLevel)
            {
                BonusLevelTimer++;
            }
            if(GameRoot.EnemiesInLevel <= 0 && IsEliminationLevel)
            {
                LoadNextLevel();
            }
        }

        public static int GetLevel()
        {
            return currLevel + 1;
        }

        public static bool IsBonusLevelRunning()
        {
            if(BonusLevelTimer < BonusLevelTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static float GetRemainingTimeInBonusLevel()
        {
            int x = BonusLevelTime - BonusLevelTimer;
            if (BonusLevelTimer != 0 && BonusLevelTime != 0)
                return  (float) x / (float) BonusLevelTime;
            else
                return 0f;
        }
    }
}
