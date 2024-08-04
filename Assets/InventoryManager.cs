using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject InventorySlots;
    public GameObject player;
    bool inventoryIsActive;
    public Vector2 SlotNumber;
    public GameObject defaultSlot;
    public List<GameObject> inventorySlots;
    public List<GameObject> itemPrefabs;
    public List<Sprite> itemUILooks;
    public List<string> itemDescriptions;
    public GameObject eventSystem;

    private bool clickedLastFrame;
    private GameObject selectedSlot;

    // Start is called before the first frame update
    void Start()
    {
        InventorySlots.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Mathf.Round((InventorySlots.GetComponent<RectTransform>().rect.width - 10 * SlotNumber.x) / SlotNumber.x), Mathf.Round((InventorySlots.GetComponent<RectTransform>().rect.height - 10 * SlotNumber.y) / SlotNumber.y));
        for (int i = 0; i < SlotNumber.x * SlotNumber.y; i++)
        {
            GameObject newSlot = Instantiate(defaultSlot, InventorySlots.transform);
            inventorySlots.Add(newSlot);
        }

        inventorySlots[0].GetComponent<inventorySlotScript>().item = itemPrefabs[1];
        inventorySlots[0].GetComponent<inventorySlotScript>().itemTexture = itemUILooks[1];
        inventorySlots[0].GetComponent<inventorySlotScript>().itemID = 1;
        inventorySlots[0].GetComponent<inventorySlotScript>().Refresh();


        Inventory.SetActive(false);
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

        if (!Input.GetMouseButton(0) && clickedLastFrame == true && Inventory.activeInHierarchy == true)
        {
            //print("mouseOff");
            MouseOff();
        }
        if (Input.GetMouseButtonDown(0) && Inventory.activeInHierarchy == true)
        {
            //print("mouseOn");
            MouseDown();
        }

        if (Input.GetMouseButton(0))
        {
            clickedLastFrame = true;
        } else
        {
            clickedLastFrame = false;
        }

    }

    void MouseOff()
    {
        GameObject newSlot = getMouseThing();
        if (newSlot != null)
        {
            GameObject tempItem = selectedSlot.GetComponent<inventorySlotScript>().item;
            Sprite tempSprite = selectedSlot.GetComponent<inventorySlotScript>().itemTexture;
            int tempID = selectedSlot.GetComponent<inventorySlotScript>().itemID;

            inventorySlotScript old = selectedSlot.GetComponent<inventorySlotScript>();
            inventorySlotScript current = newSlot.GetComponent<inventorySlotScript>();

            old.item = current.item;
            old.itemTexture = current.itemTexture;
            old.itemID = current.itemID;

            current.item = tempItem;
            current.itemTexture = tempSprite;
            current.itemID = tempID;

            current.Refresh();
            old.Refresh();
        }
    }

    void MouseDown()
    {
        selectedSlot = getMouseThing();
    }


    
     private GameObject getMouseThing()
    {
        List<RaycastResult> thingsOverlapped = new List<RaycastResult>();
        PointerEventData positionData = new PointerEventData(eventSystem.GetComponent<EventSystem>());
        positionData.position = Input.mousePosition;
        GetComponent<GraphicRaycaster>().Raycast(positionData, thingsOverlapped);
        //inventorySlots.Find()
        foreach (RaycastResult thing in thingsOverlapped)
        {
            if (thing.gameObject.GetComponent<inventorySlotScript>())
            {
                return (thing.gameObject);
            }
        }

        return null;
    }
}
