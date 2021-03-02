/*
 * WorldPortal - Used to move player to another part of the same scene
 * Created by : Allan N. Murillo
 * Last Edited : 10/21/2020
 */

using ANM.Core;
using ANM.Control;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(BoxCollider))]
    public class WorldPortal : MonoBehaviour
    {
        [SerializeField] private Vector3 teleportTo = Vector3.zero;
        private const string PlayerTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(PlayerTag))
            {
                StartCoroutine(Teleport());
            }
        }

        private IEnumerator Teleport()
        {
            if (teleportTo == Vector3.zero) yield break;
            var player = FindObjectOfType<PlayerController>();
            var agent = player.GetComponent<NavMeshAgent>();
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            yield return SceneExtension.StartLoadWithFade(true);

            agent.Warp(teleportTo);
            agent.ResetPath();

            yield return new WaitForSeconds(1f);    //    waits for camera to fix position
            yield return SceneExtension.FinishLoadWithFade(true);
        }
    }
}
