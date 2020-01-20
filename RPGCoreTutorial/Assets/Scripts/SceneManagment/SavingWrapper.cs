using RPG.Saving;
using UnityEngine;
using System.Collections;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 0.33f;

        const string defaultSaveFile = "save";


        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);   //  Scene loads and calls Awake
            Fader fader = FindObjectOfType<Fader>();    //  This happens after Awake since the line above is Async
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) { Save(); }
            if (Input.GetKeyDown(KeyCode.L)) { Load(); }
            if (Input.GetKeyDown(KeyCode.Delete)) { Delete(); }
        }

        public void Load()
        {
            print("Loading Save File");
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            print("Saving progress to File");
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            print("Deleting Save File");
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}