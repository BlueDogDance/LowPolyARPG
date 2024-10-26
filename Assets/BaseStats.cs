using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    public bool leftHandHasWeapon;
    public bool rightHandHasWeapon;
    public Transform leftHand;
    public Transform rightHand;

    public bool isBlocking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isBlocking = true;
        } else {
            isBlocking = false;
        }


        if(rightHandHasWeapon || leftHandHasWeapon)
        {
            GetComponent<PunchingScript>().enabled = false;
            GetComponent<OneHandedWeaponScript>().enabled = true;
        } else
        {
            GetComponent<PunchingScript>().enabled = true;

            GetComponent<OneHandedWeaponScript>().enabled = false;
        }
    }
}
