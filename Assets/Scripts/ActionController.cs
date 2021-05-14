using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //습득 가능한 최대거리.

    private bool pickupActivated = false; //습득이 가능 할 시 True;

    private RaycastHit hitInfo; //충돌체 정보 저장;

    [SerializeField]
    private LayerMask layerMask; //아이템 레이어에 대해서만 반응해야 하기에 필요

    //필요한 컴포넌트.
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;
    
    // Update is called once per frame
    void Update()
    {
        CheckItem(); //크로스헤어에 닿을 경우 아이템이 있는지를 매 프레임 체크한다. 
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKey(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            
            if (hitInfo.transform != null)
            {
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "아이템을 주웠습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
                ItemInfoAppear();
        }
        else
            InfoDisappear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 <color=yellow>(E키)</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
