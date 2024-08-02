using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject InventorySlots;
    bool inventoryIsActive;
    public Vector2 SlotNumber;
    public GameObject defaultSlot;
    public List<GameObject> inventorySlots;
    public List<GameObject> itemPrefabs;
    public List<Image> itemUILooks;
    public List<string> itemDescriptions;



    // Start is called before the first frame update
    void Start()
    {
        InventorySlots.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Mathf.Round((InventorySlots.GetComponent<RectTransform>().rect.width - 10 * SlotNumber.x) / SlotNumber.x), Mathf.Round((InventorySlots.GetComponent<RectTransform>().rect.height - 10 * SlotNumber.y) / SlotNumber.y));
        for (int i = 0; i < SlotNumber.x * SlotNumber.y; i++)
        {
            GameObject newSlot = Instantiate(defaultSlot, InventorySlots.transform);
            inventorySlots.Add(newSlot);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryIsActive = !inventoryIsActive;
            if (inventoryIsActive)
            {
                Inventory.SetActive(true);
            }
            else
            {
                Inventory.SetActive(false);
            }
        }


    }
}
