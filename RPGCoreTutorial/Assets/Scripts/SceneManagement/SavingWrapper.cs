using ANM.Saving;
using UnityEngine;
using ANM.Framework;
using System.Collections;

namespace ANM.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFileName = "save";
        
        
        public IEnumerator LoadLastScene()
        {
            Debug.Log("SavingWrapper::LoadLastScene()");
            var sceneTransition = FindObjectOfType<SceneTransitionManager>();    //  This happens after Awake since the line above is Async
            sceneTransition.onLoadScene.Raise();
            sceneTransition.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(DefaultSaveFileName);   //  Scene loads and calls Awake
            sceneTransition.onFinishLoadScene.Raise();
            yield return sceneTransition.FadeIn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) { Save(); }
            if (Input.GetKeyDown(KeyCode.L)) { Load(); }
            if (Input.GetKeyDown(KeyCode.Delete)) { Delete(); }
        }

        public void Load()
        {
            //print("SavingWrapper::Loading Save File");
            GetComponent<SavingSystem>().Load(DefaultSaveFileName);
        }

        public void Save()
        {
            //print("SavingWrapper::Saving progress to File");
            GetComponent<SavingSystem>().Save(DefaultSaveFileName);
        }

        public void Delete()
        {
            //print("SavingWrapper::Deleting Save File");
            GetComponent<SavingSystem>().Delete(DefaultSaveFileName);
        }
    }
}