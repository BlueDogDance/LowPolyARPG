using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class Item
{
    public GameObject prefab;
    public Sprite itemUILook;
    public string itemDescription;
    public string itemType;
}


public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject InventorySlots;
    public GameObject player;
    public GameObject dataHolder;
    bool inventoryIsActive;
    public Vector2 SlotNumber;
    public GameObject defaultSlot;
    public List<GameObject> inventorySlots;
    //public List<GameObject> itemPrefabs;
    //public List<Sprite> itemUILooks;
    //public List<string> itemDescriptions;
    public GameObject eventSystem;

    public List<Transform> specialSlots;
    //public List<string> itemType;

    public List<Item> items;

    private bool clickedLastFrame;
    private GameObject selectedSlot;

    // Start is called before the first frame update
    void Start()
    {
        items = dataHolder.GetComponent<DataHolder>().items;

        InventorySlots.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Mathf.Round((InventorySlots.GetComponent<RectTransform>().rect.width - 10 * SlotNumber.x) / SlotNumber.x), Mathf.Round((InventorySlots.GetComponent<RectTransform>().rect.height - 10 * SlotNumber.y) / SlotNumber.y));
        for (int i = 0; i < SlotNumber.x * SlotNumber.y; i++)
        {
            GameObject newSlot = Instantiate(defaultSlot, InventorySlots.transform);
            inventorySlots.Add(newSlot);
        }

        inventorySlots[0].GetComponent<inventorySlotScript>().item = items[1].prefab;
        inventorySlots[0].GetComponent<inventorySlotScript>().itemTexture = items[1].itemUILook;
        inventorySlots[0].GetComponent<inventorySlotScript>().itemID = 1;
        inventorySlots[0].GetComponent<inventorySlotScript>().Refresh();

        Cursor.lockState = CursorLockMode.Locked;
        Inventory.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryIsActive = !inventoryIsActive;
            if (inventoryIsActive)
            {
                Inventory.SetActive(true);
                player.GetComponent<OneHandedWeaponScript>().enabled = false;
                player.GetComponent<BaseStats>().enabled = false;
                player.GetComponent<PunchingScript>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Inventory.SetActive(false);
                player.GetComponent<OneHandedWeaponScript>().enabled = true;
                player.GetComponent<BaseStats>().enabled = true;
                player.GetComponent<PunchingScript>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject dropItem = getMouseThing();
            if(dropItem.GetComponent<inventorySlotScript>().item != null)
            {
                GameObject droppedItem = Instantiate(dropItem.GetComponent<inventorySlotScript>().item, player.transform.position + new Vector3(0,1,0), player.transform.rotation);
                if (droppedItem.GetComponent<BoxCollider>()){
                    droppedItem.GetComponent<BoxCollider>().enabled = true;
                }
                
                droppedItem.GetComponent<Rigidbody>().isKinematic = false;
                droppedItem.GetComponent<ItemScript>().isOnGround = true;
            }
            dropItem.GetComponent<inventorySlotScript>().item = null;
            dropItem.GetComponent<inventorySlotScript>().itemTexture = null;
            dropItem.GetComponent<inventorySlotScript>().itemID = 0;
            dropItem.GetComponent<inventorySlotScript>().Refresh();
            RefreshEquipment();
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] overlap = Physics.OverlapSphere(player.transform.position, 1);
            List<GameObject> items = new List<GameObject>();
            foreach(Collider thing in overlap)
            {
                if (thing.GetComponent<ItemScript>())
                {
                    if (thing.GetComponent<ItemScript>().isOnGround == true)
                    {
                        items.Add(thing.gameObject);
                    }
                }
            }
            foreach(GameObject item in items)
            {
                for (int i = 0; i < inventorySlots.Count; i++)
                {
                    if (inventorySlots[i].GetComponent<inventorySlotScript>().item == null)
                    {
                        inventorySlots[i].GetComponent<inventorySlotScript>().item = this.items[item.GetComponent<ItemScript>().itemID].prefab;
                        inventorySlots[i].GetComponent<inventorySlotScript>().itemTexture = this.items[item.GetComponent<ItemScript>().itemID].itemUILook;
                        inventorySlots[i].GetComponent<inventorySlotScript>().itemID = item.GetComponent<ItemScript>().itemID;
                        inventorySlots[i].GetComponent<inventorySlotScript>().Refresh();
                        Destroy(item);
                        break;
                    }
                }
            }
            
        }
    }

    void MouseOff()
    {
        GameObject newSlot = getMouseThing();
        if (newSlot != null)
        {
            if(specialSlots.Contains(newSlot.transform) || specialSlots.Contains(selectedSlot.transform))
            {
                if (specialSlots.Contains(newSlot.transform))
                {
                    int specialIndex = specialSlots.IndexOf(newSlot.transform);
                } else
                {
                    int specialIndex = specialSlots.IndexOf(selectedSlot.transform);
                }

                bool hasCorrectOld = selectedSlot.GetComponent<inventorySlotScript>().item == null;
                if (hasCorrectOld == false)
                {
                    hasCorrectOld = selectedSlot.GetComponent<inventorySlotScript>().item.GetComponent<ItemScript>().itemType == 1;
                }

                bool hasCorrectNew = newSlot.GetComponent<inventorySlotScript>().item == null;
                if (hasCorrectNew == false)
                {
                    hasCorrectNew = newSlot.GetComponent<inventorySlotScript>().item.GetComponent<ItemScript>().itemType == 1;
                }

                if (hasCorrectOld && hasCorrectNew)
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
                    RefreshEquipment();
                }
                
            } 
            else
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

    public void RefreshEquipment()
    {
        foreach(Transform slot in specialSlots)
        {
            if(specialSlots.IndexOf(slot) == 0)
            {
                if(player.GetComponent<BaseStats>().rightHand.childCount > 0)
                {
                    Destroy(player.GetComponent<BaseStats>().rightHand.GetChild(0).gameObject);
                }
                if(slot.GetComponent<inventorySlotScript>().item != null)
                {
                    GameObject newWeapon = Instantiate(slot.GetComponent<inventorySlotScript>().item);
                    newWeapon.transform.parent = player.GetComponent<BaseStats>().rightHand;
                    newWeapon.transform.localPosition = new Vector3(0, 0, 0) + newWeapon.GetComponent<ItemScript>().restingPosition;
                    newWeapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0) + newWeapon.GetComponent<ItemScript>().restingRotation);

                    player.GetComponent<BaseStats>().rightHandHasWeapon = true;
                } else
                {
                    player.GetComponent<BaseStats>().rightHandHasWeapon = false;
                }
                
            }
            if (specialSlots.IndexOf(slot) == 1)
            {
                if (player.GetComponent<BaseStats>().leftHand.childCount > 0)
                {
                    Destroy(player.GetComponent<BaseStats>().leftHand.GetChild(0).gameObject);
                }
                if (slot.GetComponent<inventorySlotScript>().item != null)
                {
                    GameObject newWeapon = Instantiate(slot.GetComponent<inventorySlotScript>().item);
                    newWeapon.transform.parent = player.GetComponent<BaseStats>().leftHand;
                    newWeapon.transform.localPosition = new Vector3(0, 0, 0) + newWeapon.GetComponent<ItemScript>().restingPosition;
                    newWeapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0) + newWeapon.GetComponent<ItemScript>().restingRotation);

                    player.GetComponent<BaseStats>().leftHandHasWeapon = true;
                }
                else
                {
                    player.GetComponent<BaseStats>().leftHandHasWeapon = false;
                }

            }
        }
    }
}
