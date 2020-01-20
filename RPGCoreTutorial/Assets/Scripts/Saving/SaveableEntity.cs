using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


namespace RPG.Saving
{
    [ExecuteAlways] //  Update will execute in runtime & editor
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();


        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
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
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;    //  Empty Gameobject Scene Path means the Object is a prefab

            SerializedObject serializedObject = new SerializedObject(this); //  If you build game without UNITY_EDITOR, this will cause an error
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");    //  SerializedObject is a generic, use SerializedProperty to get string uniqueIdentifier

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))  //  Makes sure each UUID is Unique
            {
                property.stringValue = System.Guid.NewGuid().ToString();    //  Give this object a UUID only during editor
                serializedObject.ApplyModifiedProperties(); //  Tells Unity, you have made a change to the Serialized Object
            }

            globalLookup[property.stringValue] = this;  //  Apply UUID as key to dictionary for this Entity
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;  //  Does UUID key exist already?

            if (globalLookup[candidate] == this) return true;   //  Is the current gameobject unique?

            if (globalLookup[candidate] == null)
            {   //  Has candidate been deleted?
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() == candidate
            ) return false; //  Does this candidate key not match the value in dictionary?
            globalLookup.Remove(candidate);
            return true;

        }
    }
}