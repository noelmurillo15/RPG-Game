using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        bool hasPlayed = false;


        private void OnTriggerEnter(Collider other)
        {
            if (hasPlayed || !other.tag.Equals("Player")) return;
            GetComponent<PlayableDirector>().Play();
            hasPlayed = true;
        }
    }
}