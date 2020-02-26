/*
 * ScenePortal - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Saving;
using ANM.Control;
using UnityEngine;
using System.Linq;
using System.Collections;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(BoxCollider))]
    public class ScenePortal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;
        private const string PlayerTag = "Player";

        
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
            var otherBuildIndex = SceneExtension.GetCurrentSceneBuildIndex();
            yield return SavingWrapper.Transition(sceneToLoad, PlayerTag);
            var player = FindObjectOfType<PlayerController>();
            UpdatePlayerSpawnPosition(GetOtherScenePortal(otherBuildIndex), player.gameObject);
            player.enabled = true;
            Destroy(gameObject);
        }

        
        
        private static void UpdatePlayerSpawnPosition(ScenePortal otherPortal, GameObject player)
        {
            var spawnPosition = otherPortal.transform.GetChild(0).position;
            player.transform.localPosition = spawnPosition;
            player.transform.rotation = Quaternion.identity;
        }

        private ScenePortal GetOtherScenePortal(int otherBuildIndex = -1)
        {
            if(otherBuildIndex == -1)
                return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => portal != this);
            
            otherBuildIndex -= 1;
            return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => 
                portal.name.Contains(otherBuildIndex.ToString()) && portal != this);
        }
    }
}
