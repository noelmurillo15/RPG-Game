using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _hasPlayed = false;


        private void OnTriggerEnter(Collider other)
        {
            if (_hasPlayed || !other.tag.Equals("Player")) return;
            GetComponent<PlayableDirector>().Play();
            _hasPlayed = true;
        }
    }
}