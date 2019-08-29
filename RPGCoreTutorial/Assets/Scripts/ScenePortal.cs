using UnityEngine;
using System.Collections;
using RPG.SceneManagement;
using UnityEngine.SceneManagement;


public class ScenePortal : MonoBehaviour {

    [SerializeField] int sceneToLoad = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Fader fader;
    [SerializeField] float fadeOutTime = 2f;
    [SerializeField] float fadeInTime = 3f;
    [SerializeField] float fadeWait = 3f;


    void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Player")){
            StartCoroutine("Transition");            
        }
    }

    IEnumerator Transition(){
        if(sceneToLoad < 0){
            Debug.LogError("Scene to load has not been set!");
            yield break;
        }

        DontDestroyOnLoad(gameObject);    //    Only works if gameobject is at root of scene
        Fader fader = FindObjectOfType<Fader>();

        yield return fader.FadeOut(fadeOutTime);        
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        ScenePortal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);

        yield return new WaitForSeconds(fadeWait);
        yield return fader.FadeIn(fadeInTime);
        Destroy(gameObject);
    }

    void UpdatePlayer(ScenePortal _otherPortal)
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        go.transform.position = _otherPortal.transform.GetChild(0).position;
        go.transform.rotation = _otherPortal.transform.GetChild(0).rotation;
    }

    ScenePortal GetOtherPortal()
    {
        foreach (var portal in GameObject.FindObjectsOfType<ScenePortal>())
        {
            if(portal == this) continue;
            return portal;
        } 
        return null;
    }
}