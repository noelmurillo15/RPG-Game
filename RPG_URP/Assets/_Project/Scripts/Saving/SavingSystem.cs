/*
 * SavingSystem - Uses SaveableEntity to save/load the current game state
 * Reads and writes to save file
 * Created by : Allan N. Murillo
 * Last Edited : 3/3/2020
 */

using System.IO;
using UnityEngine;
using System.Collections;
using ANM.Framework.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace ANM.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public static void SaveGameStateToFile(string saveFile)
        {
            Debug.Log("SavingSystem::SaveGameState ToFile");
            var state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public static void LoadGameStateFromFile(string saveFile)
        {
            Debug.Log("SavingSystem::LoadGameState FromFile");
            RestoreState(LoadFile(saveFile));
        }

        public static void DeleteSaveFile(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public static bool CanLoadSaveFile(string saveFile)
        {
            var state = LoadFile(saveFile);
            return state.Count > 0;
        }
        
        public static IEnumerator LoadLastGameState(string saveFile)
        {
            var state = LoadFile(saveFile);
            if (state.Count <= 0) yield break;
            
            var buildIndex = -1;
            if (state.ContainsKey("lastSceneBuildIndex"))
                buildIndex = (int)state["lastSceneBuildIndex"];
            
            yield return SceneExtension.LoadMultiSceneWithBuildIndexSequence(buildIndex, true, true);
        }
        
        private static string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
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
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            
            var index = SceneExtension.GetCurrentSceneBuildIndex();
            state["lastSceneBuildIndex"] = index;
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
    }
}