/*
 * SplashScreen -
 * Created by : Allan N. Murillo
 * Last Edited : 2/28/2022
 */

using UnityEngine;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    public class SplashScreen : MonoBehaviour
    {
        private void OnEnable() => Invoke(nameof(TransitionToMainMenu), 2f);

        private void TransitionToMainMenu()
        {
            StartCoroutine(SceneExtension.ForceMenuSceneSequence(true));
        }
    }
}
