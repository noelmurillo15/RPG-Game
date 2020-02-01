using UnityEngine;
using UnityEngine.Playables;

namespace ANM.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _hasPlayed;


        private void OnTriggerEnter(Collider other)
        {
            if (_hasPlayed || !other.tag.Equals("Player")) return;
            GetComponent<PlayableDirector>().Play();
            _hasPlayed = true;
        }
    }
}