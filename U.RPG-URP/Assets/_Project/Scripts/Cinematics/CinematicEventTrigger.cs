/*
 * CinematicEventTrigger - 
 * Created by : Allan N. Murillo
 * Last Edited : 10/20/2020
 */

using UnityEngine;
using UnityEngine.Playables;

namespace ANM.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicEventTrigger : MonoBehaviour
    {
        private bool _hasPlayed;
        [SerializeField] private int enemyCounter;
        [SerializeField] private GameObject objectToActivate;

        public void EnemyHasDied()
        {
            enemyCounter--;
            if (enemyCounter > 0 || _hasPlayed) return;
            GetComponent<PlayableDirector>().Play();
            _hasPlayed = true;
            if (objectToActivate == null) return;
            objectToActivate.SetActive(true);
        }
    }
}
