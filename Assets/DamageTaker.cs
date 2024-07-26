using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    public bool usesAnimation;
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void takeDamage(int amount)
    {
        health -= amount;

        if (usesAnimation)
        {
            gameObject.GetComponent<Animator>().SetTrigger("hit");
        }
    }
}
