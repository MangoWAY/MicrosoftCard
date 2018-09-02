using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler,IPointerClickHandler{
    public event EventHandler EndDragEvent;
    public event EventHandler RightMouseDownEvent;
    public bool isEnableDrag = false;
    private RectTransform m_rt;
    private RectTransform m_heapRt;
    private void Start()
    {   
        m_rt = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerId==-2&&RightMouseDownEvent!=null)
        {
            RightMouseDownEvent(this.gameObject, EventArgs.Empty);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (!isEnableDrag)
            return;
        foreach (RectTransform t in GetComponentsInParent<RectTransform>())
        {
            if(t.tag=="cardheap")
            {
                m_heapRt = t;
                break;
            }
        }
        m_heapRt.SetSiblingIndex(m_heapRt.parent.childCount);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!isEnableDrag)
            return;
        Vector3 globalMousePos = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle
                (GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out globalMousePos);
        m_rt.position = globalMousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isEnableDrag)
            return;
        if (EndDragEvent!=null)
        {
            EndDragEvent(this, EventArgs.Empty);
        }
               
    }

   
}
