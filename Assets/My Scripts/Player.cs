﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float MaxHP = 100f;
    [SerializeField] float currentHP = 100f;

	public float healthAsPercentage
    {
        get
        {
            return currentHP / MaxHP;
        }
    }
}
