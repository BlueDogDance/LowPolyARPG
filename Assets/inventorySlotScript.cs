using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventorySlotScript : MonoBehaviour
{
    public GameObject item;
    public Sprite itemTexture;
    public int itemID;

    public void Refresh()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = itemTexture;
    }
}
