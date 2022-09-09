using UnityEngine;

namespace SwingGame.Media
{
    public class AudioManager
    {
        public static float SOUND_VOLUME = 1.0f;

        private static AudioManager INSTANCE;

        private AudioManager()
        {
        }

        public static AudioManager GetInstance()
        {
            if (INSTANCE is null)
            {
                INSTANCE = new AudioManager();
            }

            return INSTANCE;
        }

        public void PlayLandingBallSound(GameObject gameObject)
        {
            AudioSource.PlayClipAtPoint(Resources.Load("Audio/LandingBall") as AudioClip,gameObject.transform.position,SOUND_VOLUME);
        }

        public void PlayDropBallSound(GameObject gameObject)
        {
            AudioSource.PlayClipAtPoint(Resources.Load("Audio/DropBall") as AudioClip,gameObject.transform.position,SOUND_VOLUME);
        }

        public void PlaySwingSound(GameObject gameObject)
        {
            AudioSource.PlayClipAtPoint(Resources.Load("Audio/Swing") as AudioClip,gameObject.transform.position,SOUND_VOLUME);
        }

        public void PlayStackingBallSound(GameObject gameObject)
        {
            // AudioSource.PlayClipAtPoint(Resources.Load("Audio/...") as AudioClip,gameObject.transform.position,SOUND_VOLUME); //TODO
        }

        public void PlayDestructionBallSound(GameObject gameObject)
        {
            AudioSource.PlayClipAtPoint(Resources.Load("Audio/MagicExplode") as AudioClip,gameObject.transform.position,SOUND_VOLUME);
        }
    }
}