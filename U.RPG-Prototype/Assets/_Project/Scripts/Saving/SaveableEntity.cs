/*
 * SaveableEntity - 
 * Created by : Allan N. Murillo
 * Last Edited : 5/23/2022
 */

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace ANM.Saving
{
    [ExecuteAlways] //  Update will execute in runtime & editor
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";

        private static readonly Dictionary<string, SaveableEntity> GlobalLookup = new Dictionary<string, SaveableEntity>();


        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<string, object>)state;
            foreach (var saveable in GetComponents<ISaveable>())
            {
                var typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR    //  Exclude if Built Game
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;  //  Make sure update doesn't run during runtime & only in editor
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;    //  Empty Game object Scene Path means the Object is a prefab

            var serializedObject = new SerializedObject(this); //  If you build game without UNITY_EDITOR, this will cause an error
            var property = serializedObject.FindProperty("uniqueIdentifier");    //  SerializedObject is a generic, use SerializedProperty to get string uniqueIdentifier

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))  //  Makes sure each UUID is Unique
            {
                property.stringValue = System.Guid.NewGuid().ToString();    //  Give this object a UUID only during editor
                serializedObject.ApplyModifiedProperties(); //  Tells Unity, you have made a change to the Serialized Object
            }

            GlobalLookup[property.stringValue] = this;  //  Apply UUID as key to dictionary for this Entity
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!GlobalLookup.ContainsKey(candidate)) return true;  //  Does UUID key exist already?

            if (GlobalLookup[candidate] == this) return true;   //  Is the current game object unique?

            if (GlobalLookup[candidate] == null)
            {   //  Has candidate been deleted?
                GlobalLookup.Remove(candidate);
                return true;
            }

            if (GlobalLookup[candidate].GetUniqueIdentifier() == candidate
            ) return false; //  Does this candidate key not match the value in dictionary?
            GlobalLookup.Remove(candidate);
            return true;
        }
    }
}