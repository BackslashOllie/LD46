using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float curHealth;
    public float maxHealth;

    public void AdjustHealth(float adjustment)
    {
        curHealth += adjustment;
        
        if(adjustment <= 0)
            OnHit();

        if(curHealth <= 0)
        {
            // die
            curHealth = 0;

            OnDeath();
        }

        if(curHealth >= maxHealth)
        {
            curHealth = maxHealth;

        }
    }

    public virtual void OnHit()
    {

    }

    public void OnDeath()
    {
        print("i be dead");
    }
}
