using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    //필요한 컴포넌트들
    [SerializeField]
    private GameObject go_Base;
    [SerializeField]
    private Text txt_itemName;
    [SerializeField]
    private Text txt_desc;
    [SerializeField]
    private Text txt_howToUse;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.8f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 1.2f,
                            0f);
        go_Base.SetActive(true);
        go_Base.transform.position = _pos;
        txt_itemName.text = _item.itemName;
        txt_desc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
            txt_howToUse.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            txt_howToUse.text = "좌클릭 - 먹기";
        else
            txt_howToUse.text = " ";
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
