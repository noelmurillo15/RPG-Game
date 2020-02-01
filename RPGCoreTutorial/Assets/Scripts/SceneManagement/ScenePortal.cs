using System.Collections;
using System.Linq;
using ANM.Control;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace ANM.SceneManagement
{
    public class ScenePortal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private float fadeOutTime = 2f;
        [SerializeField] private float fadeWaitTime = 3f;
        [SerializeField] private float fadeInTime = 3f;


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load has not been set!");
                yield break;
            }

            //  Only works if gameobject is at root of scene
            DontDestroyOnLoad(gameObject);

            //  Get Fader && SavingWrapper
            Fader fade = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();

            //  Remove old Player Control
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;

            //  Panel Alpha Fade Out
            yield return fade.FadeOut(fadeOutTime);

            //  Save current State && Load new level
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return SceneManager.LoadSceneAsync("Menu Ui", LoadSceneMode.Additive);
            
            //  Remove Control from new Player
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            //  Load current state
            wrapper.Load(); //  TODO : Currently when using ScenePortal, if you load new scene and load save file, the stored transform is from previous scene instead of new scene

            //  Get & Set Player Spawn
            ScenePortal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            //  Save new state in new Level
            wrapper.Save();

            //  Panel Alpha Fade In
            yield return new WaitForSeconds(fadeWaitTime);

            //  This will finish in background since we are no longer yield returning it- changed fadein/fadeout to Coroutine instead of IEnumerator to achieve this
            fade.FadeIn(fadeInTime);

            //  Restore Player Control
            playerController.enabled = true;

            //  Destroy Scene Portal
            Destroy(gameObject);
        }

        private static void UpdatePlayer(ScenePortal otherPortal)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
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