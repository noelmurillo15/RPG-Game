using ANM.Saving;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace ANM.Framework
{
    public class ScenePortal : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        [SerializeField] private int sceneToLoad = -1;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(PlayerTag))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)  {  yield break;  }
            
            DontDestroyOnLoad(gameObject);
            yield return SavingWrapper.Transition(sceneToLoad, PlayerTag);
            
            ScenePortal otherPortal = GetOtherScenePortal();
            var player = GameManager.Instance.GetPlayerController();
            UpdatePlayerSpawnPosition(otherPortal, player.gameObject);
            
            SavingWrapper.SaveGameState();
            player.enabled = true;
            Destroy(gameObject);    // Each Level should contain its own scene Portal, Destroy this one
        }

        private static void UpdatePlayerSpawnPosition(ScenePortal otherPortal, GameObject player)
        {
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.transform.GetChild(0).position;
            player.transform.rotation = otherPortal.transform.GetChild(0).rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private ScenePortal GetOtherScenePortal()
        {
            return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => portal != this);
        }
    }
}