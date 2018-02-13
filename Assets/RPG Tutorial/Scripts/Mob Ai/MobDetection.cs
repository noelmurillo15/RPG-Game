﻿// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class MobDetection : MonoBehaviour {


        MobMaster mobMaster;
        Transform myTransform;
        RaycastHit hit;

        [SerializeField] Transform head;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] LayerMask sightLayer;
        [SerializeField] float detectRadius = 80f;
        [SerializeField] float detectBehindRadius = 10f;

        private float checkRate;
        private float nextCheck;



        void Initialize()
        {
            mobMaster = GetComponent<MobMaster>();
            myTransform = transform;

            if (head == null)
            {
                head = myTransform;
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

        void CarryOutDetection()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;

                Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, sightLayer);

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

        bool CanPotentialTargetsBeSeen(Transform potentialTarget)
        {

            if (Physics.Linecast(head.position, potentialTarget.position, out hit, sightLayer))
            {
                if (hit.transform == potentialTarget)
                {
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

        void DisableThis()
        {
            this.enabled = false;
        }
    }
}