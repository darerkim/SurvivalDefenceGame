using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; //true인 상태에서는 마우스에 따라 카메라가 움직이지 않고 여타 기능을 꺼서 인벤토리에 집중하게 한다.

    //필요한 컴포넌트
    [SerializeField]
    private GameObject go_inventoryBase;
    [SerializeField]
    private GameObject go_slotsParent;

    //슬롯들.
    private Slot[] slots;

    public Slot[] GetSlots() { return slots; }

    [SerializeField] private Item[] items;

    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = go_slotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

            if (!inventoryActivated)
                OpenInventory();
            else
                CloseInventory();

            inventoryActivated = !inventoryActivated;
        }
    }

    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_inventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (_item.itemType != Item.ItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        //slots[i].itemCount += _count;
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
           

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
