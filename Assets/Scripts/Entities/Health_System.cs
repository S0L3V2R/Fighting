using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Health_System : MonoBehaviour
{
    [SerializeField] float maxHP;
    [SerializeField] float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    
    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0) { Death(); }
    }

    public void Death()
    {
        Debug.Log("YMER");
    }

}
