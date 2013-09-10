using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace JTZS
{
    public class SoundsLib
    {
        public SoundEffect menuSound;
        public SoundEffect gunShot;
        public SoundEffect scream;
        public SoundEffect zombieGroans;
        public SoundEffect theme;

        public SoundsLib(ContentManager content)
        {
            menuSound = content.Load<SoundEffect>("menunav");
            gunShot = content.Load<SoundEffect>("shot");
            scream = content.Load<SoundEffect>("oh");
            zombieGroans = content.Load<SoundEffect>("zombie_groans2");
            theme = content.Load<SoundEffect>("horrortheme");
        }
    }
}
