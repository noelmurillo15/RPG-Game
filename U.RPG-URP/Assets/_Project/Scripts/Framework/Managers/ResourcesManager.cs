/*
 * ResourcesManager - Contains important Game Resources such as the Inventory and the Input Controller
 * Created by : Allan N. Murillo
 * Last Edited : 3/11/2022
 */

using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace ANM.Scriptables.Manager
{
    [CreateAssetMenu(menuName = "Single Instance/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        private static readonly Dictionary<string, Object> LoadedResources = new();


        public static void Initialize()
        {
            LoadedResources.Clear();
            PreLoadResource("PlayerControls");
        }

        private static Object PreLoadResource(string resourceFilePath)
        {
            if (LoadedResources.ContainsKey(resourceFilePath)) return LoadedResources[resourceFilePath];
            var resource = Resources.Load(resourceFilePath);
            if(resource != null) LoadedResources.Add(resourceFilePath, resource);
            return resource;
        }

        public static T FindResource<T>(string resourceFilePath) where T : class
        {
            var objectToLoad = PreLoadResource(resourceFilePath);
            return objectToLoad == null ? null : objectToLoad as T;
        }
    }
}
