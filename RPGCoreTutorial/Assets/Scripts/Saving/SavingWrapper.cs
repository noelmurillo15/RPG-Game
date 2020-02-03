using ANM.Control;
using UnityEngine;
using System.Collections;

namespace ANM.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFileName = "save";
        

        public static IEnumerator LoadLastGameState()
        {
            yield return SavingSystem.LoadLastGameState(DefaultSaveFileName);
        }

        public static IEnumerator Transition(int buildIndex, string playerTag)
        {
            SaveGameState();    //    Saves the state of the old scene
            GameObject.FindWithTag(playerTag).GetComponent<PlayerController>().enabled = false;
            yield return SavingSystem.Transition(buildIndex);
            GameObject.FindWithTag(playerTag).GetComponent<PlayerController>().enabled = false;
            LoadGameState();    //    Loads the state of the new scene if a save exists
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete)) { DeleteSaveFile(); }
        }

        public static void LoadGameState()
        {
            SavingSystem.LoadGameStateFromFile(DefaultSaveFileName);
        }

        public static void SaveGameState()
        {
            SavingSystem.SaveGameStateToFile(DefaultSaveFileName);
        }

        public static void DeleteSaveFile()
        {
            SavingSystem.DeleteSaveFile(DefaultSaveFileName);
        }
    }
}