/*
 * ButtonSounds -
 * Created by : Allan N. Murillo
 * Last Edited : 3/17/2022
 */

using UnityEngine;

namespace ANM.Framework.Audio
{
    public class ButtonSounds : MonoBehaviour
    {
        private float _delay;
        private AmbientAudioPlayer _player;


        private void Start()
        {
            _delay = 0f;
            _player = FindObjectOfType<AmbientAudioPlayer>();
        }

        //  Needs Animator for sound to execute
        public void PlayButtonPressedSound()
        {
            if (!(Time.timeSinceLevelLoad > _delay)) return;
            _player.PlaySoundEffect();
            _delay = Time.timeSinceLevelLoad + 0.1f;
        }
    }
}
