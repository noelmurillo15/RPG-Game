using System.IO;
using UnityEngine;
using ANM.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace ANM.Saving
{
    public class SavingSystem : MonoBehaviour
    {

        public static void SaveGameStateToFile(string saveFile)
        {
            var state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public static void LoadGameStateFromFile(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public static void DeleteSaveFile(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }
        
        public static IEnumerator LoadLastGameState(string saveFile)
        {
            var state = LoadFile(saveFile);
            if (state.Count <= 0) yield break;
            
            var sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            var buildIndex = sceneTransitionManager.GetCurrentScene().buildIndex;
            
            if (state.ContainsKey("lastSceneBuildIndex"))
                buildIndex = (int)state["lastSceneBuildIndex"];
            
            yield return sceneTransitionManager.BeginLoadScene(buildIndex);
            RestoreState(state);
            yield return sceneTransitionManager.EndLoadScene();
        }

        public static IEnumerator Transition(int buildIndex)
        {
            yield return SceneTransitionManager.BeginLoadNextLevel(buildIndex);
        }

        private static Dictionary<string, object> LoadFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (var stream = File.Open(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private static void SaveFile(string saveFile, object state)
        {
            var path = GetPathFromSaveFile(saveFile);
            using (var stream = File.Open(path, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private static void CaptureState(IDictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            state["lastSceneBuildIndex"] = SceneTransitionManager.GetCurrentSceneBuildIndex();
        }

        private static void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                var id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        private static string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}