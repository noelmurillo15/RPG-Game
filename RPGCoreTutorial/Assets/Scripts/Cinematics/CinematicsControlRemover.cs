using RPG.Core;
using RPG.Control;
using UnityEngine;
using RPG.Movement;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicsControlRemover : MonoBehaviour
    {
        //  Cached Variables
        GameObject player = null;


        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player"); ;
        }

        void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector playDirector)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<CharacterMove>().Cancel();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector playDirector)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}