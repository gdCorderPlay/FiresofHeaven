using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoilderIcon : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public SoldierType soldierType;
    public System.Action<SoldierType> action;
    private Sprite sprite;
    void Start()
    {
        sprite = GetComponent<Image>().sprite;
    }
   

    public void OnPointerDown(PointerEventData eventData)
    {
       // Cursor.SetCursor(sprite.texture,sprite.rect.size*0.5f,CursorMode.Auto);
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (action != null)
        {
            action(soldierType);
        }
        //Cursor.SetCursor()
    }
}
