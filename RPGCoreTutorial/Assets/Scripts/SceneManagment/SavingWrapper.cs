using RPG.Saving;
using UnityEngine;
using System.Collections;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 0.33f;

        const string defaultSaveFile = "save";


        void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
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