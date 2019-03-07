using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    static class Art
    {
        public static Texture2D PlayerBase { get; private set; }
        public static Texture2D PlayerStand { get; private set; }
        public static Texture2D PlayerFlap1 { get; private set; }
        public static Texture2D PlayerFlap2 { get; private set; }
        public static Texture2D PlayerWalk1 { get; private set; }
        public static Texture2D PlayerWalk2 { get; private set; }
        public static Texture2D EnemyBase { get; private set; }
        public static Texture2D EnemyStand { get; private set; }
        public static Texture2D EnemyFlap1 { get; private set; }
        public static Texture2D EnemyFlap2 { get; private set; }
        public static Texture2D EnemyWalk1 { get; private set; }
        public static Texture2D EnemyWalk2 { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Ground { get; private set; }
        public static Texture2D Water { get; private set; }
        public static Texture2D HealthBlock { get; private set; }
        public static Texture2D Heart { get; private set; }
        public static Texture2D HeartInterior { get; private set; }
        public static Texture2D BG1 { get; private set; }
        public static Texture2D TestBG { get; private set; }
        public static Texture2D BG2 { get; private set; }
        public static Texture2D TestBG2 { get; private set; }
        public static Texture2D TestBG3 { get; private set; }
        public static Texture2D BG4 { get; private set; }
        public static Texture2D BG5 { get; private set; }
        public static Texture2D BG6 { get; private set; }
        public static Texture2D BG7 { get; private set; }
        public static Texture2D BG8 { get; private set; }
        public static Texture2D BG9 { get; private set; }
        public static Texture2D BG10 { get; private set; }
        public static Texture2D BG11 { get; private set; }
        public static Texture2D BoxAnim1 { get; private set; }
        public static Texture2D BoxAnim2 { get; private set; }
        public static Texture2D BoxAnim3 { get; private set; }
        public static Texture2D BoxAnim4 { get; private set; }
        public static Texture2D arrow { get; private set; }
        public static Texture2D ClockBack { get; private set; }
        public static Texture2D BonusText { get; private set; }
        public static Texture2D Baloon1 { get; private set; }
        public static Texture2D Baloon2 { get; private set; }
        public static Texture2D Baloon3 { get; private set; }
        public static Texture2D Baloon4 { get; private set; }
        public static Texture2D Baloon5 { get; private set; }
        public static Texture2D Baloon6 { get; private set; }
        public static Texture2D Baloon7 { get; private set; }
        public static Texture2D Baloon8 { get; private set; }


        public static void Load(ContentManager content)
        {
            LoadBackGrounds(content);
            LoadNonInteractable(content);
            LoadPlayerSprites(content);
            LoadEnemySprites(content);
            LoadPlatforms(content);
            LoadInteractable(content);
            LoadUI(content);


        }

        private static void LoadNonInteractable(ContentManager content)
        {
            BonusText = content.Load<Texture2D>("BonusLevelText");
            ClockBack = content.Load<Texture2D>("clockback");
            arrow = content.Load<Texture2D>("arrow");
        }

        private static void LoadPlatforms(ContentManager content)
        {
            Seeker = content.Load<Texture2D>("boxanimation1");
            Ground = content.Load<Texture2D>("GroundSprite");
            Water = content.Load<Texture2D>("Ground2");
        }

        private static void LoadBackGrounds(ContentManager content)
        {
            BG1 = content.Load<Texture2D>("Level1BG");
            TestBG = content.Load<Texture2D>("BG1");
            BG2 = content.Load<Texture2D>("Level2");
            TestBG2 = content.Load<Texture2D>("Level2");
            TestBG3 = content.Load<Texture2D>("Level3");
            BG4 = content.Load<Texture2D>("Level4");
            BG5 = content.Load<Texture2D>("Level5");
            BG6 = content.Load<Texture2D>("Level6");
            BG7 = content.Load<Texture2D>("Level7");
            BG8 = content.Load<Texture2D>("Level8");
            BG9 = content.Load<Texture2D>("Level9");
            BG10 = content.Load<Texture2D>("Mazebg");
            BG11 = content.Load<Texture2D>("BonusBG");
        }
        private static void LoadPlayerSprites(ContentManager content)
        {
            PlayerBase = content.Load<Texture2D>("bodytest");
            PlayerStand = content.Load<Texture2D>("mothstand");
            PlayerFlap1 = content.Load<Texture2D>("mothflap");
            PlayerFlap2 = content.Load<Texture2D>("mothflap2");
            PlayerWalk1 = content.Load<Texture2D>("mothwalk1");
            PlayerWalk2 = content.Load<Texture2D>("mothwalk2");
        }
        private static void LoadEnemySprites(ContentManager content)
        {
            EnemyBase = content.Load<Texture2D>("jdbase");
            EnemyStand = content.Load<Texture2D>("jdstand");
            EnemyFlap1 = content.Load<Texture2D>("jdflap");
            EnemyFlap2 = content.Load<Texture2D>("jdflap2");
            EnemyWalk1 = content.Load<Texture2D>("jdwalk1");
            EnemyWalk2 = content.Load<Texture2D>("jdwalk2");
        }
        private static void LoadUI(ContentManager content)
        {
            Heart = content.Load<Texture2D>("alphaheart2");
            HealthBlock = content.Load<Texture2D>("healthblock2");
            HeartInterior = content.Load<Texture2D>("heartinterior2");
        }

        private static void LoadInteractable(ContentManager content)
        {
            BoxAnim1 = content.Load<Texture2D>("boxanimation1");
            BoxAnim2 = content.Load<Texture2D>("boxanimation2");
            BoxAnim3 = content.Load<Texture2D>("boxanimation3");
            BoxAnim4 = content.Load<Texture2D>("boxanimation4");
            Baloon1 = content.Load<Texture2D>("b1");
            Baloon2 = content.Load<Texture2D>("b2");
            Baloon3 = content.Load<Texture2D>("b3");
            Baloon4 = content.Load<Texture2D>("b4");
            Baloon5 = content.Load<Texture2D>("b5");
            Baloon6 = content.Load<Texture2D>("b6");
            Baloon7 = content.Load<Texture2D>("b7");
            Baloon8 = content.Load<Texture2D>("b8");
        }


    }
}
