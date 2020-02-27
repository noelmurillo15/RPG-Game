/*
 * SavingWrapper - Wrapper class for the SavingSystem
 * Subscribes to LoadSceneEvents and saves/loads the game when loading a level
 * Created by : Allan N. Murillo
 * Last Edited : 2/26/2020
 */

using ANM.Core;
using ANM.Control;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;

namespace ANM.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFileName = "save";
        private const int FirstLevelBuildIndex = 2;
        private const int LastLevelBuildIndex = 3;

        private void Start()
        {
            SceneExtension.StartSceneLoadEvent += OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadScene;
        }
        
        private static void OnStartLoadScene(bool b)
        {
            if(!b) return;
            var index = SceneExtension.GetCurrentSceneBuildIndex();
            if (index < FirstLevelBuildIndex && index > LastLevelBuildIndex) return;
            SaveGameState();
        }
        
        private static void OnFinishLoadScene(bool b)
        {
            if(!b) return;
            var index = SceneExtension.GetCurrentSceneBuildIndex();
            if (index < FirstLevelBuildIndex && index > LastLevelBuildIndex) return;
            LoadGameState();
        }
        
        private void OnDestroy()
        {
            if (gameObject.GetComponentInParent<GameManager>() != GameManager.Instance) return;
            SceneExtension.StartSceneLoadEvent -= OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadScene;
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

        private static void DeleteSaveFile()
        {
            SavingSystem.DeleteSaveFile(DefaultSaveFileName);
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
            var player = FindObjectOfType<PlayerController>();
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            var agent = player.GetComponent<NavMeshAgent>();
            player.enabled = false;
            agent.enabled = false;
        }
    }
}