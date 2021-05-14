using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage; //아이템의 이미지

    //필요한 컴포넌트
    private ItemEffectDataBase theItemEffectDataBase;
    [SerializeField]
    private Text text_count;
    [SerializeField]
    private GameObject go_count_image;

    void Start()
    {
        //[SerializeField]의 경우 Slot과 같은 프리펩은 내부의 컴포넌트만을 사용 할 수 있기에 FindObjectOfType를 사용 해 준다.
        theItemEffectDataBase = FindObjectOfType<ItemEffectDataBase>();
    }

    //이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color; 
    }

    //아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        //Item 을 작성할때 itemImage를 Sprite로 작성했다.
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_count_image.SetActive(true);
            text_count.text = itemCount.ToString();
        }
        else
        {
            text_count.text = "0";
            go_count_image.SetActive(false);
        }

        SetColor(1);
    }

    //아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // 슬롯 초기
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        text_count.text = "0";
        go_count_image.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                theItemEffectDataBase.UseItem(item);

                if (item.itemType == Item.ItemType.Used )
                    SetSlotCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }

    //마우스가 슬롯에 들어갈때 발동.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            theItemEffectDataBase.ShowToolTip(item, this.transform.position);
        }
    }

    //마우스가 슬롯에서 벗어날때 발동.
    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDataBase.HideToolTip();
    }
}
