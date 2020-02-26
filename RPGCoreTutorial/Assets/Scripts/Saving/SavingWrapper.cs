/*
 * SavingWrapper - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/23/2020
 */

using ANM.Control;
using UnityEngine;
using System.Collections;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;

namespace ANM.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFileName = "save";


        private void Start()
        {
            SceneExtension.StartSceneLoadEvent += OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadScene;
        }
        
        private static void OnStartLoadScene(bool b)
        {
            var index = SceneExtension.GetCurrentSceneBuildIndex();
            if (index != 2 && index != 3) return;
            SaveGameState();
        }
        
        private static void OnFinishLoadScene(bool b)
        {
            var index = SceneExtension.GetCurrentSceneBuildIndex();
            if (index != 2 && index != 3) return;
            LoadGameState();
        }
        
        public static bool CanLoadSaveFile()
        {
            return SavingSystem.CanLoadSaveFile(DefaultSaveFileName);
        }

        public static IEnumerator LoadLastGameState()
        {
            yield return SavingSystem.LoadLastGameState(DefaultSaveFileName);
        }

        public static IEnumerator Transition(int buildIndex, string playerTag)
        {
            GameObject.FindWithTag(playerTag).GetComponent<PlayerController>().enabled = false;
            yield return SceneExtension.LoadMultiSceneWithBuildIndexSequence(buildIndex, true);
            GameObject.FindWithTag(playerTag).GetComponent<PlayerController>().enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete)) { DeleteSaveFile(); }
            if (Input.GetKeyDown(KeyCode.S)) { SaveGameState(); }
            if (Input.GetKeyDown(KeyCode.L)) { LoadGameState(); }
        }

        private static void LoadGameState()
        {
            SavingSystem.LoadGameStateFromFile(DefaultSaveFileName);
        }

        private static void SaveGameState()
        {
            SavingSystem.SaveGameStateToFile(DefaultSaveFileName);
        }

        public static void DeleteSaveFile()
        {
            SavingSystem.DeleteSaveFile(DefaultSaveFileName);
        }

        private void OnDestroy()
        {
            if (gameObject.GetComponentInParent<GameManager>() != GameManager.Instance) return;
            SceneExtension.StartSceneLoadEvent -= OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadScene;
        }
    }
}