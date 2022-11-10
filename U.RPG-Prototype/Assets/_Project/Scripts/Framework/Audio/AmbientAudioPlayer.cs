using UnityEngine;
using System.Collections;

namespace ANM.Framework.Audio
{
    public class AmbientAudioPlayer : MonoBehaviour
    {
        private int _index;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _clips;
        [SerializeField] private AudioClip _buttonSFX;


        private void OnEnable()
        {
            Debug.Log("[AmbientAudioPlayer]: OnEnable");
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = _clips[_index];
            _audioSource.loop = true;
            StartCoroutine(LoopThroughSoundtracks());
            
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            Debug.Log("[AmbientAudioPlayer]: OnDisable");
            _audioSource.Stop();
            _audioSource.enabled = false;
            _audioSource = null;
        }

        private IEnumerator LoopThroughSoundtracks()
        {
            yield return new WaitForSeconds(1f);
            if(!_audioSource.isPlaying) _audioSource.Play();
            while (true)
            {
                _index++;
                yield return new WaitForSeconds(_audioSource.clip.length);
                if (_clips[_index] == null) _index = 0;
                _audioSource.Stop();
                _audioSource.clip = _clips[_index];
                _audioSource.Play();
            }
        }

        public void PlaySoundEffect(AudioClip audioClip = null)
        {
            Debug.Log("[AmbientAudioPlayer]: PlaySoundEffect");
            _audioSource.PlayOneShot(audioClip == null ? _buttonSFX : audioClip);
        }
    }
}