/*
 * LoadSceneButton -
 * Created by : Allan N. Murillo
 * Last Edited : 6/14/2021
 */

using UnityEngine;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private string levelName = string.Empty;
        [SerializeField] private bool forceUserAvatarOption;

        public void ButtonPressed(bool multiScene = false)
        {
            if (!SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName)) return;
            if (forceUserAvatarOption)
            {
                if (GameManager.Instance.userChoiceAvatar == PlayerAvatar.None)
                {
                    Debug.LogWarning("[LoadSceneButton]: Please choose an Avatar");
                }
                else
                {
                    StartCoroutine(multiScene
                        ? SceneExtension.LoadMultiSceneSequence(levelName, true)
                        : SceneExtension.LoadSingleSceneSequence(levelName, true));
                }
            }
            else
            {
                StartCoroutine(multiScene
                    ? SceneExtension.LoadMultiSceneSequence(levelName, true)
                    : SceneExtension.LoadSingleSceneSequence(levelName, true));
            }
        }
    }
}
