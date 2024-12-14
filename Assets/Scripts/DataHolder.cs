using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Item> items = new List<Item>();

    private void Start()
    {
        print(items[0].itemDescription);
    }
}
