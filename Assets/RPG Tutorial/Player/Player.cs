// Allan Murillo : Unity RPG Core Test Project
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {


    [SerializeField] float MaxHP = 100f;
    [SerializeField] float currentHP = 100f;



	public float healthAsPercentage { get { return currentHP / MaxHP; } }

    public void TakeDamage(float dmg)
    {
        currentHP = Mathf.Clamp(currentHP - dmg, 0f, MaxHP);
    }
}
