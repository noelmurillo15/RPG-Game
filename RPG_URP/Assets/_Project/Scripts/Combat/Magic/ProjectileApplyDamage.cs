/*
 * ProjectileApplyDamage -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

//using ANM.FPS.Npc;
using UnityEngine;

public class ProjectileApplyDamage : MonoBehaviour
{
    [SerializeField] private GameObject splashParticlePrefab = null;
    private Transform _myTransform;
    private float _damage;


    public void SetDamage(float dmg)
    {
        _damage = dmg;
    }

    private void OnEnable()
    {
        _myTransform = transform;
        Invoke(nameof(Explode), 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { return; }

        if (other.transform.root.CompareTag("Npc") || other.transform.root.CompareTag("Enemy"))
        {    //    TODO : if not called by event, GetHit won't be able to depict damage taken to console
            Debug.Log("Npc or Enemy was hit by your Projectile causing : " + _damage + " damage");
            //other.transform.root.GetComponent<NpcMaster>().CallEventNpcDeductHealth((int)_damage);
        }
        //    TODO : if not sent by msg, damage multiplier wont be factored into total damage
        //other.SendMessage("ProcessDamage", _damage, SendMessageOptions.DontRequireReceiver);
        CancelInvoke(nameof(Explode));
        Explode();
    }

    private void Explode()
    {
        if(splashParticlePrefab != null){
            var spawnPos = _myTransform.position;
            var spawnRot = _myTransform.rotation;
            var splashParticleObject = Instantiate(splashParticlePrefab, spawnPos, spawnRot);
            splashParticleObject.transform.parent = null;
            splashParticleObject.GetComponent<ParticleSystem>().Play();
        }
        gameObject.SetActive(false);
    }
}
