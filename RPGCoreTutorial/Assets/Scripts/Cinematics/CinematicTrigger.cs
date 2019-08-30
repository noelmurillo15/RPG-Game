using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        bool hasPlayed = false;


        void OnTriggerEnter(Collider other)
        {
            if (!hasPlayed && other.tag.Equals("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                hasPlayed = true;
            }
        }
    }
}