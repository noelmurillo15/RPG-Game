/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobDetection.cs
/// </summary>
using UnityEngine;


namespace RPG {

    public class MobDetection : MonoBehaviour {


        Character mobMaster;
        RaycastHit hit;

        [SerializeField] Transform head;
        [SerializeField] LayerMask sightLayer;
        [SerializeField] float detectRadius = 80f;
        [SerializeField] float detectBehindRadius = 10f;

        private float checkRate;
        private float nextCheck;



        void Initialize()
        {
            mobMaster = GetComponent<Character>();

            if (head == null)
            {
                Debug.Log("Head has not been assigned in Inspector");
                head = mobMaster.MyTransformRef;
            }

            checkRate = Random.Range(.75f, 1.25f);
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventCharacterDie += DisableThis;
        }

        void OnDisable()
        {
            mobMaster.EventCharacterDie -= DisableThis;
        }

        void Update()
        {
            CarryOutDetection();
        }

        #region Detection
        /// <summary>
        /// 
        /// </summary>
        void CarryOutDetection()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;

                Collider[] colliders = Physics.OverlapSphere(mobMaster.MyTransformRef.position, detectRadius, sightLayer);

                if (colliders.Length > 0)
                {
                    foreach (Collider potentialTarget in colliders)
                    {
                        if (potentialTarget.CompareTag("Player"))
                        {
                            if (potentialTarget.transform == mobMaster.transform)
                            {
                                continue;
                            }

                            if (Vector3.Distance(mobMaster.transform.position, potentialTarget.transform.position) < detectBehindRadius)
                            {
                                mobMaster.CallEventSetAttackTarget(potentialTarget.transform);
                                mobMaster.CallEventSetCharacterNavTarget(potentialTarget.transform);
                                break;
                            }

                            if (CanPotentialTargetsBeSeen(potentialTarget.transform))
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    mobMaster.CallEventCharacterLostTarget();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="potentialTarget"></param>
        /// <returns></returns>
        bool CanPotentialTargetsBeSeen(Transform potentialTarget)
        {

            if (Physics.Linecast(head.position, potentialTarget.position, out hit, sightLayer))
            {
                if (hit.transform == potentialTarget)
                {
                    mobMaster.CallEventSetAttackTarget(potentialTarget);
                    mobMaster.CallEventSetCharacterNavTarget(potentialTarget);
                    return true;
                }
                else
                {
                    mobMaster.CallEventCharacterLostTarget();
                    return false;
                }
            }
            else
            {
                mobMaster.CallEventCharacterLostTarget();
                return false;
            }
        }
        /// <summary>
        /// Disables Upon Death
        /// </summary>
        void DisableThis()
        {
            this.enabled = false;
        }
        #endregion
    }
}