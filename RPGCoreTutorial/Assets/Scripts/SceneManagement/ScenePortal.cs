using ANM.Control;
using System.Linq;
using UnityEngine;
using ANM.Framework;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ANM.SceneManagement
{
    public class ScenePortal : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private SceneTransitionManager sceneTransitionManager;


        private void Start()
        {
            if (sceneTransitionManager != null) return;
            sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0 || sceneTransitionManager == null)
            {
                Debug.Log("ScenePortal can not Transition!");
                yield break;
            }

            //  Only works if gameobject is at root of scene
            DontDestroyOnLoad(gameObject);

            var wrapper = FindObjectOfType<SavingWrapper>();
            GameObject.FindWithTag(PlayerTag).GetComponent<PlayerController>().enabled = false;

            //  Panel Alpha Fade Out
            yield return sceneTransitionManager.FadeOut();

            //  Save current State && Load new level
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return SceneManager.LoadSceneAsync("Menu Ui", LoadSceneMode.Additive);
            
            //  Remove Control from new Player
            var playerController = GameObject.FindWithTag(PlayerTag).GetComponent<PlayerController>();
            playerController.enabled = false;

            //  Load current state
            wrapper.Load(); //  TODO : Currently when using ScenePortal, if you load new scene and load save file, the stored transform is from previous scene instead of new scene

            //  Get & Set Player Spawn
            ScenePortal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            //  Save new state in new Level
            wrapper.Save();

            //  Panel Alpha Fade In
            yield return sceneTransitionManager.FadeIn();

            //  Restore Player Control
            playerController.enabled = true;

            //  Destroy Scene Portal
            Destroy(gameObject);
        }

        private static void UpdatePlayer(ScenePortal otherPortal)
        {
            var go = GameObject.FindGameObjectWithTag(PlayerTag);
            go.GetComponent<NavMeshAgent>().enabled = false;
            go.transform.position = otherPortal.transform.GetChild(0).position;
            go.transform.rotation = otherPortal.transform.GetChild(0).rotation;
            go.GetComponent<NavMeshAgent>().enabled = true;
        }

        private ScenePortal GetOtherPortal()
        {
            return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => portal != this);
        }
    }
}