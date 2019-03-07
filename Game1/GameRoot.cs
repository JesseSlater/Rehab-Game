using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System;
using System.IO;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Game
    {

        public static bool isDebugMode = false;


        public static Color bgColor = Color.Yellow; 

        public static SpriteFont sf; 
        public static int EnemiesInLevel = 0;
        public static bool canXCameraMove = false;
        public static bool canYCameraMove = false;
        public static bool willTeleportIfOffscreen; 
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        static World world;
        public static short PlatformBodyCollisionGroup = 0;
        public static short PlayerHurtBoxCollisionGroup = 1;
        public static short PlayerHitBoxCollisionGroup = 2;
        public static short EnemyHurtBoxCollisionGroup = 3;
        public static short EnemyHitBoxCollisionGroup = 4;
        public static short ActorBodyCollisionGroup = 5;
        private static float screenBoundLeft;
        public static Player Player1;
        public static Camera camera; 
        public static float ScreenBoundLeft
        {
            get
            {
                return screenBoundLeft; 
            }
        }
        private static float screenBoundRight;
        public static float ScreenBoundRight
        {
            get
            {
                return screenBoundRight;
            }
        }

        public static GraphicsDeviceManager graphics;
        private BasicEffect _spriteBatchEffect;
        SpriteBatch spriteBatch;
        private static Viewport viewport;

        UI ui;

        //The default values for this are 400, 190
        //Seriously, for the love of god don't change them this took me hours to set up
        public static Vector3 cameraPosition = new Vector3(400, 190f,0);
        private DebugView debugView;
        MotionDataReciever mdr;

        private static Floor debugBox;
        private StreamWriter positions;

        private MouseState currentMouseState;
        private MouseState lastMouseState;
        private int timesClicked;

        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            _spriteBatchEffect = new BasicEffect(graphics.GraphicsDevice);
            world = new World();

            SetUpCamera();
            LevelManager.LoadLevel(0);
            MediaPlayer.Play(SFX.BGM, new TimeSpan(0, 0, 0));
            ui = new UI();
            mdr = new MotionDataReciever();

            //C:\Users\Labuser\Documents\MonoGameMothMan\GameRoot\Game1\bin\Windows\x86\Debug
            //positions = File.AppendText(@"Positions.txt");

            cameraPosition = new Vector3(400, 190f, 0);




        }


        public void SetUpCamera()
        {
            debugView = new DebugView(world);
            debugView.AppendFlags(DebugViewFlags.DebugPanel | DebugViewFlags.PolygonPoints);
            debugView.LoadContent(GraphicsDevice, Content);
            viewport = GraphicsDevice.Viewport;
            camera = new Camera(viewport);
        }

        public static void SetUpLevel1()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, Art.Ground, ground_position, 60));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0,0 - Art.Ground.Height), 0));


            //Add BG
            BackgroundManager.SetBG(Art.BG1, new Vector2(1f, 1f));
            Arrow clock = new Arrow(world, GameRoot.ScreenSize * .5f);



            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X * .25f, GameRoot.ScreenSize.X * .1f));
            EntityManager.Add(Player1);
            //This is just how to set up the debug interactable
             //debug = new Interactable(world, GameRoot.ScreenSize * .75f);
            //debug.isDebug = true;
            //EntityManager.Add(debug);


            EntityManager.Add(new Interactable(world, GameRoot.ScreenSize * .75f));

        }



        public static void SetUpLevel2()
        {

            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            BackgroundManager.SetBG(Art.BG2, new Vector2(1f, 1f));


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, Art.Ground, ground_position, 60));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));
            EntityManager.Add(new Floor(world, new Vector2(570, 272), 126, 33));

            ;

            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X * .2f, GameRoot.ScreenSize.Y * .9f) );
            EntityManager.Add(Player1);
            EntityManager.Add(new Interactable(world, new Vector2(570f, 230f)));





        }
        public static void SetUpLevel3()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            BackgroundManager.SetBG(Art.TestBG3, new Vector2(0f,0f), new Vector2(1f, 1f), Color.White, new Color(155f, 173f, 183f));
            bgColor = new Color(155, 173, 183);
            canYCameraMove = true;


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, BackgroundManager.currBG.Height*.87f );


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, Art.Ground, ground_position, 60));
            EntityManager.Add(new Floor(world, new Vector2(0, -60), GameRoot.ScreenSize.X*2, 60));
            //This puts a wall at the left edge of the screen; cool but not right now
            //EntityManager.Add(new Floor(world, new Vector2(-20, 0), 20, BackgroundManager.currBG.Height*.86f));
            //Walls
            EntityManager.Add(new Wall(world, new Vector2(126, 390), 20, BackgroundManager.currBG.Height));
            EntityManager.Add(new Wall(world, new Vector2(670, 390), 20, BackgroundManager.currBG.Height));
            //Platforms
            EntityManager.Add(new Floor(world, new Vector2(398, 146), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(261, 210), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(532, 214), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(398, 354), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(554, 784), 135, 62));
            EntityManager.Add(new Floor(world, new Vector2(435, 795), 95, 33));
            //Rotated Platforms
            EntityManager.Add(new Floor(world, new Vector2(357, 578), 251, 30, 3.2f));
            EntityManager.Add(new Floor(world, new Vector2(401, 706), 244, 30, 3f));





            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X /2, GameRoot.ScreenSize.Y * .1f));
            EntityManager.Add(Player1);
            EntityManager.Add(new Interactable(world, GameRoot.ScreenSize * .75f));



            //Add arrow
            EntityManager.Add(new Arrow(world, new Vector2(GameRoot.ScreenSize.X * .45f, GameRoot.ScreenSize.Y * .2f), 0));
            EntityManager.Add(new Arrow(world, new Vector2(GameRoot.ScreenSize.X * .45f, GameRoot.ScreenSize.Y * .6f), 0));
            EntityManager.Add(new Arrow(world, new Vector2(GameRoot.ScreenSize.X * .75f, GameRoot.ScreenSize.Y), 0));



        }


        public static void SetUpLevel4()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            BackgroundManager.SetBG(Art.TestBG3, new Vector2(0f, 0f), new Vector2(1f, 1f), Color.White, new Color(155f, 173f, 183f));
            bgColor = new Color(155, 173, 183);
            canYCameraMove = true;


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, BackgroundManager.currBG.Height * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, Art.Ground, ground_position, 60));
            EntityManager.Add(new Floor(world, new Vector2(0, -60), GameRoot.ScreenSize.X * 2, 60));
            //This puts a wall at the left edge of the screen; cool but not right now
            //EntityManager.Add(new Floor(world, new Vector2(-20, 0), 20, BackgroundManager.currBG.Height*.86f));
            //Walls
            EntityManager.Add(new Wall(world, new Vector2(126, 390), 20, BackgroundManager.currBG.Height));
            EntityManager.Add(new Wall(world, new Vector2(670, 390), 20, BackgroundManager.currBG.Height));
            //Platforms
            EntityManager.Add(new Floor(world, new Vector2(398, 146), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(261, 210), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(532, 214), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(398, 354), 137, 63));
            EntityManager.Add(new Floor(world, new Vector2(554, 784), 135, 62));
            EntityManager.Add(new Floor(world, new Vector2(435, 795), 95, 33));
            //Rotated Platforms
            EntityManager.Add(new Floor(world, new Vector2(357, 578), 251, 30, 3.2f));
            EntityManager.Add(new Floor(world, new Vector2(401, 706), 244, 30, 3f));





            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X *.75f, GameRoot.ScreenSize.Y * .75f));
            EntityManager.Add(Player1);
            EntityManager.Add(new Interactable(world, new Vector2(GameRoot.ScreenSize.X / 2, GameRoot.ScreenSize.Y * .1f)));



            //Add arrow
            EntityManager.Add(new Arrow(world, new Vector2(GameRoot.ScreenSize.X * .45f, GameRoot.ScreenSize.Y * .2f), 6f));
            EntityManager.Add(new Arrow(world, new Vector2(GameRoot.ScreenSize.X * .45f, GameRoot.ScreenSize.Y * .6f), 2.8f));
            EntityManager.Add(new Arrow(world, new Vector2(GameRoot.ScreenSize.X * .75f, GameRoot.ScreenSize.Y), 2.8f));



        }

        public static void SetUpLevel5()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            canYCameraMove = false;
            BackgroundManager.SetBG(Art.BG4, new Vector2(1f, 1f));

            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, new Vector2(392, 89), 143, 36));
            EntityManager.Add(new Floor(world, new Vector2(106, 235), 143, 36));
            EntityManager.Add(new Floor(world, new Vector2(706, 235), 143, 36));
            EntityManager.Add(new Floor(world, new Vector2(378, 406), Art.Ground.Width, 24));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));

            

            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(108, 380));
            EntityManager.Add(Player1);
            EntityManager.Add(new Interactable(world, new Vector2(108, 205)));
            EntityManager.Add(new Interactable(world, new Vector2(708, 205)));
            EntityManager.Add(new Interactable(world, new Vector2(398, 87)));
            EntityManager.Add(new Interactable(world, new Vector2(398, 396)));
        }

        public static void SetUpLevel6()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            canYCameraMove = false;
            BackgroundManager.SetBG(Art.BG5, new Vector2(1f, 1f));


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, Art.Ground, ground_position, 60));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));
            EntityManager.Add(new Floor(world, new Vector2(381, 199), 179, 60, false));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));

            ;

            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X * .5f, Art.PlayerBase.Height * 2));
            EntityManager.Add(Player1);
            EntityManager.Add(new PatrollingEnemy(world, new Vector2(GameRoot.ScreenSize.X * .5f, GameRoot.ScreenSize.Y - Art.EnemyStand.Height)));
        }


        public static void SetUpLevel7()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            canYCameraMove = false;
            BackgroundManager.SetBG(Art.BG6, new Vector2(1f, 1f));


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, new Vector2(378, 406), Art.Ground.Width, 24));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));
            EntityManager.Add(new Floor(world, new Vector2(520, 174), 179, 60, false));


            

            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(520, 150));
            EntityManager.Add(Player1);
            Enemy x = new Enemy(world, new Vector2(GameRoot.ScreenSize.X * .1f, GameRoot.ScreenSize.Y - Art.EnemyStand.Height));
            x.isLessAgressive = true;
            EntityManager.Add(x);
        }

        public static void SetUpLevel8()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            canYCameraMove = false;
            BackgroundManager.SetBG(Art.BG7, new Vector2(1f, 1f));


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .83f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, Art.Water, ground_position, 90));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));

            ;

            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X * .5f, Art.PlayerBase.Height * 2));
            EntityManager.Add(Player1);
            EntityManager.Add(new Enemy(world, new Vector2(GameRoot.ScreenSize.X * .5f, GameRoot.ScreenSize.Y - Art.EnemyStand.Height)));
        }

        public static void SetUpLevel9()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            canYCameraMove = false;
            BackgroundManager.SetBG(Art.BG8, new Vector2(1f, 1f));


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, new Vector2(378, 412), Art.Ground.Width, 24));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));
            EntityManager.Add(new Floor(world, new Vector2(390, 267), 117, 30, false));
            EntityManager.Add(new Floor(world, new Vector2(82, 91), 117, 30, false));
            EntityManager.Add(new Floor(world, new Vector2(724, 91), 117, 30, false));

            ;

            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X * .5f, 382));
            EntityManager.Add(Player1);
            EntityManager.Add(new Enemy(world, new Vector2(96,58)));
            EntityManager.Add(new Enemy(world, new Vector2(725, 58)));
        }
        public static void SetUpLevel10()
        {
            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;
            canYCameraMove = false;
            BackgroundManager.SetBG(Art.BG9, new Vector2(1f, 1f));


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, new Vector2(378, 425), Art.Ground.Width, 24));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));
            EntityManager.Add(new Floor(world, new Vector2(27f, 206), 270, 19, false));
            EntityManager.Add(new Floor(world, new Vector2(777, 206), 270, 19, false));
            EntityManager.Add(new Floor(world, new Vector2(400, 55), 211, 19, false));

            ;

            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X * .5f, 398));
            EntityManager.Add(Player1);
            EntityManager.Add(new Enemy(world, new Vector2(70, 169)));
            EntityManager.Add(new Enemy(world, new Vector2(725, 169)));
            EntityManager.Add(new Enemy(world, new Vector2(400, 28)));
        }


        public static void SetUpBonusLevel1()
        {
            LevelManager.BonusLevelTime = 2500;
            LevelManager.IsBonusLevel = true;

            //Set Up World
            TurnOffCameraMovement();
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, new Vector2(378, 400), Art.Ground.Width, 24));
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));


            //Add BG
            BackgroundManager.SetBG(Art.BG11, new Vector2(1f, 1f));
            Arrow clock = new Arrow(world, GameRoot.ScreenSize * .5f);
            clock.isBonusClock = true;
            EntityManager.Add(clock);
            EntityManager.Add(new BonusText(world, new Vector2(GameRoot.ScreenSize.X * .25f, GameRoot.ScreenSize.Y * .25f)) );



            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(GameRoot.ScreenSize.X * .25f, GameRoot.ScreenSize.Y * .1f));
            EntityManager.Add(Player1);
            //This is just how to set up the debug interactable
            //debug = new Interactable(world, GameRoot.ScreenSize * .75f);
            //debug.isDebug = true;
            //EntityManager.Add(debug);


            EntityManager.Add(new Interactable(world, GameRoot.ScreenSize * .75f));
            Spawner leftCenter = new Spawner(world, new Vector2(GameRoot.ScreenBoundLeft - 5, GameRoot.ScreenSize.Y * .5f), new Vector2(0, 10), new Vector2(-5, 5), false);
            Spawner rightCenter = new Spawner(world, new Vector2(GameRoot.ScreenBoundRight + 5, GameRoot.ScreenSize.Y * .5f), new Vector2(-10, 0), new Vector2(-5, 5), false);
            EntityManager.Add(leftCenter, world);
            EntityManager.Add(rightCenter, world);
        }

        public static void SetUpBonusLevel2()
        {
            LevelManager.BonusLevelTime = 25000;
            LevelManager.IsBonusLevel = true;

            //Set Up World
            TurnOffCameraMovement();
            canXCameraMove = true;
            world.Gravity = new Vector2(0, 110);
            willTeleportIfOffscreen = !canXCameraMove;


            //Set Ground
            Vector2 ground_position = new Vector2(graphics.PreferredBackBufferHeight * -.1f, graphics.PreferredBackBufferHeight * .87f);


            //Set Screen Bounds;
            screenBoundRight = graphics.PreferredBackBufferWidth;

            //Add Geometry
            EntityManager.Add(new Floor(world, Art.Ground, new Vector2(0, 0 - Art.Ground.Height), 0));
            EntityManager.Add(new Hitbox(world, new Vector2(596.25f, 328.75f), new Vector2(65, 20)));
            EntityManager.Add(new Hitbox(world, new Vector2(525f, 185f), new Vector2(19, 73)));
            EntityManager.Add(new Hitbox(world, new Vector2(648.75f, 130f), new Vector2(19, 73)));
            EntityManager.Add(new Hitbox(world, new Vector2(726.25f, 301.25f), new Vector2(19, 104)));
            EntityManager.Add(new Hitbox(world, new Vector2(817.5f, 297.5f), new Vector2(19, 107)));
            EntityManager.Add(new Hitbox(world, new Vector2(955f, 323.75f), new Vector2(74, 21)));
            EntityManager.Add(new Hitbox(world, new Vector2(1032.5f, 183.75f), new Vector2(20, 73)));
            EntityManager.Add(new Hitbox(world, new Vector2(897.5f, 130f), new Vector2(18, 73)));

            EntityManager.Add(new Hitbox(world, new Vector2(185f, 298.75f), new Vector2(150, 100)));
            EntityManager.Add(new Hitbox(world, new Vector2(315f, 378.75f), new Vector2(78, 118)));
            EntityManager.Add(new Hitbox(world, new Vector2(740f, 403.75f), new Vector2(851, 29)));
            EntityManager.Add(new Hitbox(world, new Vector2(620.25f, -22.50019f), new Vector2(1090, 37)));
            EntityManager.Add(new Hitbox(world, new Vector2(781.25f, 27.49989f), new Vector2(621, 37)));

            //Walls
            EntityManager.Add(new Wall(world, new Vector2(75f, 120f), 34, 242));
            EntityManager.Add(new Wall(world, new Vector2(1200f, 201.25f), 35, 367));




            //Add BG
            BackgroundManager.SetBG(Art.BG10, new Vector2(1f, 1f));
            EntityManager.Add(new BonusText(world, new Vector2(GameRoot.ScreenSize.X * .75f, GameRoot.ScreenSize.Y * .25f)));




            //Add Players And Enemies
            Player1 = new Player(world, new Vector2(728, 190));
            EntityManager.Add(Player1);
            //192 210
            Drifter currDrifter;
            currDrifter = new Drifter(world, new Vector2(192, 210))
            {
                driftDirection = new Vector2(0, 0)
            };
            EntityManager.Add(currDrifter);

            currDrifter = new Drifter(world, new Vector2(420, 310))
            {
                driftDirection = new Vector2(0, 0)
            };
            EntityManager.Add(currDrifter);

            currDrifter = new Drifter(world, new Vector2(765, 220))
            {
                driftDirection = new Vector2(0, 0)
            };
            EntityManager.Add(currDrifter);
            currDrifter = new Drifter(world, new Vector2(955, 275))
            {
                driftDirection = new Vector2(0, 0)
            };
            EntityManager.Add(currDrifter);
            // drifter = new Drifter(world, new Vector2(240, 210));
            // drifter.driftDirection = new Vector2(0, 0);
            //  EntityManager.Add(drifter);

            // EntityManager.Add(debugBox);
        }

        private static void TurnOffCameraMovement()
        {
            canXCameraMove = false;
            canYCameraMove = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Art.Load(Content);
            SFX.Load(Content);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sf = Content.Load<SpriteFont>("sf");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            MotionDataReciever.OnApplicationQuit();
         //   positions.Close();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
                // The active state from the last frame is now old
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame
            currentMouseState = Mouse.GetState();

            // Recognize a single click of the left mouse button
            if (isDebugMode && lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                timesClicked++;
                //positions.WriteLine("EntityManager.Add(new Hitbox(world, new Vector2(" + debugBox.Position.X + ", " + debugBox.Position.Y + "), new Vector2(" + debugBox.wi + ", " + debugBox.h + ")));");
                //positions.WriteLine("EntityManager.Add(new Hitbox(world, new Vector2(" + debugBox.Position.X + "f, " + debugBox.Position.Y + "f), new Vector2(" + debugBox.wi + ", " + debugBox.h +")));");
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                
                Exit();

            }

            // TODO: Add your update logic here

            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.R) && isDebugMode)
            {
                LevelManager.ReloadLevel();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N) && GameRoot.isDebugMode )
            {
                LevelManager.LoadNextLevel();
            }


            // in Update()
            EntityManager.Update();
            LevelManager.Update();
            SFX.Update();
            camera.Update();

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);

            var vp = GraphicsDevice.Viewport;

            //The following two lines worked before 11/8/18, when I decided to start messing with the camera
            //        _spriteBatchEffect.View = Matrix.CreateLookAt(cameraPosition, cameraPosition + Vector3.Forward, Vector3.Up);
            //      _spriteBatchEffect.Projection = Matrix.CreateOrthographic(Viewport.Width, -Viewport.Height , 0f, 1f);
            //_spriteBatchEffect.Projection = Matrix.CreateOrthographic(cameraViewWidth, cameraViewWidth / vp.AspectRatio, 0f, -1f);

            Vector3 newV = new Vector3(Camera.center.X, cameraPosition.Y, 0);
            if(canYCameraMove)
            {
                newV = new Vector3(cameraPosition.X, Camera.center.Y, 0);
            }
            _spriteBatchEffect.View = Matrix.CreateLookAt(newV, newV + Vector3.Forward, Vector3.Up);
             _spriteBatchEffect.Projection = Matrix.CreateOrthographic(Viewport.Width, -Viewport.Height , 0f, 1f);


            // TODO: Add your drawing code here

            base.Draw(gameTime);


            // in Draw()
            GraphicsDevice.Clear(bgColor);
           
            
            //restore this to spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullClockwise, _spriteBatchEffect);

            EntityManager.Draw(spriteBatch);
            BackgroundManager.Draw(spriteBatch);
            spriteBatch.End();
            if(isDebugMode)
                debugView.RenderDebugData(_spriteBatchEffect.Projection, _spriteBatchEffect.View, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, 0.8f);
            ui.Draw(spriteBatch);



        }

        public static void RemoveBody(Body x)
        {
            if(x == null)
            {
                return;
            }

            if(x.World == world)
            world.Remove(x);
        }

        public static void SetBGColor(Color x)
        {
            bgColor = x;
        }



    }
}
