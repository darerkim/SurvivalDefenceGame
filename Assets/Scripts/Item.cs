using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Monobehaviour로 상속된 스크립트는 게임 오브젝트에 붙여서 사용했지만
//ScriptableObject 를 상속하게 되면 게임 오브젝트에 붙일 필요가 없게된다.
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; //아이템의 이름
    [TextArea]
    public string itemDesc; //아이템의 설명.
    public Sprite itemImage; //아이템의 이미지 이미지는 켄버스에서만 출력 할 수 있지만 스프라이트는 켄버스 밖에서도 출력 가능하다. (Sprite역시 이미지의 일부라고 보면 된다.)
    public GameObject itemPrefab; //아이템의 프리펩.
    public ItemType itemType; //아이템의 유형.

    public string weaponType; //무기 유형.

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
}
