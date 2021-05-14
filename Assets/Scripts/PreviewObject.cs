using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //충돌한 오브젝트의 콜라이더를 저장한다.
    private List<Collider> colliderList = new List<Collider>();
    [SerializeField]
    private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
            SetColor(red);//레드
        else
            SetColor(green);//그린
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }

    private void SetColor(Material _mat)
    {
        foreach (Transform tf_child in this.transform)
        {
            var newMaterials = new Material[tf_child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
                newMaterials[i] = _mat;

            tf_child.GetComponent<Renderer>().materials = newMaterials;

            //foreach (Renderer mat in tf_child.transform)
            //{
            //    mat.material = _mat;
            //}

            //for (int i = 0; i < tf_child.GetComponent<Renderer>().materials.Length; i++)
            //{
            //    tf_child.GetComponent<Renderer>().materials[i] = _mat;
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other); 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);
    }

}
