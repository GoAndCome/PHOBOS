using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;  

public class ButtonMouse : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    public GameObject D_Select;
    public GameObject Select;

    /*    private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("올라왔음");
                D_Select.SetActive(false);
                Select.SetActive(true);
            } else
            {
                D_Select.SetActive(true);
                Select.SetActive(false);
            }
        }*/


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("올라왔음");
        //SelectBe.sprite = SelectAp;
        D_Select.SetActive(false);
        Select.SetActive(true);
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        D_Select.SetActive(true);
        Select.SetActive(false);
        //throw new System.NotImplementedException();
    }
}
