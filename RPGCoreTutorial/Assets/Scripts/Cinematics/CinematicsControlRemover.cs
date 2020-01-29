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
        private GameObject _player = null;


        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector playDirector)
        {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<CharacterMove>().Cancel();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector playDirector)
        {
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}