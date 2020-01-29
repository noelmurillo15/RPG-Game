using RPG.Saving;
using UnityEngine;
using System.Collections;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeInTime = 0.33f;

        private const string DefaultSaveFile = "save";


        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(DefaultSaveFile);   //  Scene loads and calls Awake
            Fader fade = FindObjectOfType<Fader>();    //  This happens after Awake since the line above is Async
            fade.FadeOutImmediate();
            yield return fade.FadeIn(fadeInTime);
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
            GetComponent<SavingSystem>().Load(DefaultSaveFile);
        }

        public void Save()
        {
            print("Saving progress to File");
            GetComponent<SavingSystem>().Save(DefaultSaveFile);
        }

        public void Delete()
        {
            print("Deleting Save File");
            GetComponent<SavingSystem>().Delete(DefaultSaveFile);
        }
    }
}