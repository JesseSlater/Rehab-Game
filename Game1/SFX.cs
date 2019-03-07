using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    static class SFX
    {
        public static SoundEffect Step { get; private set; }
        public static SoundEffect Flap { get; private set; }
        public static SoundEffect Hurt { get; private set; }
        public static SoundEffect NonHurt { get; private set; }
        public static SoundEffect BoxDestroy { get; private set; }
        public static Song BGM { get; private set; }



        public static void Load(ContentManager content)
        {
            Step = content.Load<SoundEffect>("Step");
            Flap = content.Load<SoundEffect>("Flap");
            Hurt = content.Load<SoundEffect>("Hurt");
            NonHurt = content.Load<SoundEffect>("NonHurt");
            BoxDestroy = content.Load<SoundEffect>("BoxDestroy");
            BGM = content.Load<Song>("BGM");
                     

        }

        public static void Update()
        {
            if(MediaPlayer.PlayPosition > BGM.Duration )
            {
                MediaPlayer.Play(BGM, new TimeSpan(0,0,0,11,240));
            }
        }
    }






}


