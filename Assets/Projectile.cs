using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool isMoving;
    public string type;

    public Vector3 maxSize;
    public GameObject origin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.localScale.x < maxSize.x)
        {
            transform.localScale += maxSize / 10;
        }
        if(isMoving == true)
        {
            GetComponent<Rigidbody>().velocity = (speed * -1) * transform.forward;
        }
        
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject thingHit = null; 

        if (other.GetComponent<Redirector>())
        {
            thingHit = other.GetComponent<Redirector>().Direction;
        } else
        {
            thingHit = other.gameObject;
        }

        if(thingHit != origin)
        {
            print("hit");
        }

    }
}
