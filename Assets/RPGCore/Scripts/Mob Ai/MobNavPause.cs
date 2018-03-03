/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobNavPause.cs
/// </summary>
using UnityEngine;
using System.Collections;


namespace RPG {

    public class MobNavPause : MonoBehaviour {


        Character mobMaster;
        float pauseTime = 1f;



        void Initialize()
        {
            mobMaster = GetComponent<Character>();
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventCharacterDie += DisableThis;
            mobMaster.EventCharacterTakeDamage += PauseNavMeshAgent;
        }

        void OnDisable()
        {
            mobMaster.EventCharacterDie -= DisableThis;
            mobMaster.EventCharacterTakeDamage -= PauseNavMeshAgent;
        }

        #region Navigation Pause
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dummy"></param>
        void PauseNavMeshAgent(float dummy)
        {
            if (mobMaster.MyNavAgent != null)
            {
                if (mobMaster.MyNavAgent.enabled)
                {
                    mobMaster.MyNavAgent.ResetPath();
                    mobMaster.IsNavPaused = true;
                    StartCoroutine(RestartNavMeshAgent());
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator RestartNavMeshAgent()
        {
            yield return new WaitForSeconds(pauseTime);
            mobMaster.IsNavPaused = false;
        }
        /// <summary>
        /// Disables Upon Death
        /// </summary>
        void DisableThis()
        {
            StopAllCoroutines();
        }
        #endregion
    }
}