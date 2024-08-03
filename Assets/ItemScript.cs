using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class numbers
{
    public List<float> parameters;
}
[System.Serializable]
public class collisionAreas
{
    public List<numbers> amount;
}



public class ItemScript : MonoBehaviour
{
    public int itemID;

    public bool isOnGround;
    public bool isWeapon;
    public int weaponDamage;

    public collisionAreas hitBoxes = new collisionAreas();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < hitBoxes.amount.Count; i++)
        {
            if(hitBoxes.amount[i].parameters.Count >= 6)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(hitBoxes.amount[i].parameters[0], hitBoxes.amount[i].parameters[1], hitBoxes.amount[i].parameters[2]), new Vector3(hitBoxes.amount[i].parameters[3], hitBoxes.amount[i].parameters[4], hitBoxes.amount[i].parameters[5]));
            }
        }

    }
}
