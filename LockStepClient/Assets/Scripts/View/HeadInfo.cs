using Battle.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeadInfo 
{
    public Transform root;
    public Slider slider;
    public Image bg;
    private RectTransform rect;
    private int targetValue;
    private float speed;
    public HeadInfo(Transform trans)
    {
        root = trans;
        rect = trans.GetComponent<RectTransform>();
        slider = trans.GetComponent<Slider>();
        bg = slider.fillRect.GetComponent<Image>();
    }
    public void Init(SoldierData data)
    {
        slider.maxValue = TroopHelper.GetMaxHp(data.type);
        targetValue = data.hp;
        slider.value = data.hp;
        if ((StaticDef.playerMode == 0) == data.isAtkTroop)//¼º·½
        {
            bg.color = Color.green;
        }
        else
        {
            bg.color = Color.red;
        }
        root.gameObject.SetActive(true);
    }
    public void UpdatePos(Vector3 pos,int hp)
    {
        rect.position = pos;
        if (targetValue != hp)
        {
            targetValue = hp;
            speed = (targetValue - slider.value) * 0.5f;
        }
        slider.value = Mathf.Max(targetValue, slider.value+speed*Time.deltaTime);
    }
    public void Receyle()
    {
        root.gameObject.SetActive(false);
    }
}
