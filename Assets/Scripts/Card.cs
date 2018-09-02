using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]

public class CardInfo
{
    public int num;//牌面数字
    public string color;//牌的花色，用A、B、C、D表示
    public string imgName;//牌对应的图片名称
    public bool isRed;//是否是红色
    public bool isUsed;//是否使用
}

public class cardAniArgs:EventArgs
{
    public Vector2 targetPos;
    public float duration;
    public RectTransform cardRt;
    public cardAniArgs(Vector2 pos,float du,RectTransform rt)
    {
        targetPos = pos;
        duration = du;
        cardRt = rt;
    }
}

public class Card : MonoBehaviour {

    
    public CardHeap curHeap;//当前所在牌堆的引用
    public bool isInteractable;//是否可以交互
    public CardInfo cardInfo;//当前这个牌的信息
    public event EventHandler cardAniEvent;
    private Vector2 m_originalPos;//原来的位置
    private RectTransform m_rt;//当前的位置组件
    private GlobalManager m_gloManager;//全局管理器的引用
    private Image m_img;//当前图片组件的引用
    private Drag m_drag;
    private Vector2 m_offset;
    
    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        m_rt = GetComponent<RectTransform>();
        m_gloManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        m_img = GetComponent<Image>();
        m_originalPos = m_rt.localPosition;
        m_drag = GetComponent<Drag>();
        m_drag.EndDragEvent += HanleEndDragEvent;
        //m_drag.RightMouseDownEvent += HandleRightMouseDownEvent;
    }
    private void Awake()
    {
        Init();
    }
   
    public void SetCardInfo(CardInfo info)
    {
        cardInfo = info;    
    }
    public void SetCardImg(string imgName)
    {
        m_img.sprite = m_gloManager.cardsSprites.GetSprite(imgName);
    }

    void HandleRightMouseDownEvent(object sender,EventArgs e)
    {
        GameObject orderheap = m_gloManager.orderHeap;
        foreach(CardHeap h in orderheap.GetComponentsInChildren<CardHeap>())
        {
            if(!h.curCard&&cardInfo.num!=1)
            {
                continue;
            }
            if ((!h.curCard&&cardInfo.num==1)|| 
                (h.curCard.cardInfo.color == cardInfo.color && h.curCard.cardInfo.num == cardInfo.num - 1))
            {

                RectTransform heapRt = m_gloManager.GetHeapRcTrans(h.name);
                CardHeap tempHeap = heapRt.GetComponent<CardHeap>();
                Card tempCard = tempHeap.curCard;

                //将原来牌堆当前牌替换
                curHeap.RefreshCurCard(this);

                //处理新牌堆
                if (!tempCard)
                {
                    m_rt.SetParent(heapRt, false);
                    if (cardAniEvent != null)
                        cardAniEvent(this, new cardAniArgs(new Vector2(0,-68), 0.5f, m_rt));
                }
                else
                {
                    m_rt.SetParent(tempCard.GetComponent<RectTransform>(), false);
                    if (cardAniEvent != null)
                        cardAniEvent(this, new cardAniArgs(Vector2.zero, 0.5f, m_rt));
                }
                Card card = GetComponent<Card>();
                foreach (Transform t in GetComponentsInChildren<Transform>())
                {
                    card = t.GetComponent<Card>();
                }
                curHeap = tempHeap;
                curHeap.curCard = card;
                SetCurHeap(curHeap);   
            }
        }
        if(m_gloManager.orderHeap.transform.childCount==52)
        {
            Debug.Log("you win");
        }
    }


    void HanleEndDragEvent(object sender, EventArgs e)
    {
        if (!m_gloManager.GetHeapRcTrans(m_gloManager.heapname))
        {

            if (cardAniEvent != null)
                cardAniEvent(this, new cardAniArgs(m_originalPos, 0.5f,m_rt));
            return;
        }

        RectTransform heapRt = m_gloManager.GetHeapRcTrans(m_gloManager.heapname);
        CardHeap tempHeap = heapRt.GetComponent<CardHeap>();
        Card tempCard = tempHeap.curCard;

        if (heapRt.tag == "orderheap")
        {
            if ((m_gloManager.heapname == curHeap.name) || (!tempCard && cardInfo.num != 1) ||
                (tempCard && ((tempCard.cardInfo.num != cardInfo.num - 1) || (tempCard.cardInfo.color!=cardInfo.color))))
            {
                if(cardAniEvent!=null)
                    cardAniEvent(this, new cardAniArgs(m_originalPos, 0.5f,m_rt));
                return;
            }
            m_offset = Vector2.zero;
        }
        else if (heapRt.tag == "cardheap")
        {
            if ((m_gloManager.heapname == curHeap.name) || (!tempCard && cardInfo.num != 13) ||
                (tempCard && ((tempCard.cardInfo.num - 1 != cardInfo.num) || (tempCard.cardInfo.isRed == cardInfo.isRed))))
            {
                if (cardAniEvent != null)
                    cardAniEvent(this, new cardAniArgs(m_originalPos, 0.5f,m_rt));
                return;
            }
            m_offset = new Vector2(0, -20);
        }
        //将原来牌堆当前牌替换
        curHeap.RefreshCurCard(this);

        //处理新牌堆
        if (!tempCard)
        {
            m_rt.SetParent(heapRt, false);
            m_rt.localPosition = new Vector2(0, -68);
        }
        else
        {
            m_rt.SetParent(tempCard.GetComponent<RectTransform>(), false);
            m_rt.localPosition = m_offset;
        }
        Card card = GetComponent<Card>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            card = t.GetComponent<Card>();
        }
        curHeap = tempHeap;
        curHeap.curCard = card;
        SetCurHeap(curHeap);
    }
    public void SetOriginalPos(Vector2 pos)
    {
        m_originalPos = pos;
    }
    public void SetCurHeap(CardHeap heap)
    {
        foreach(RectTransform t in GetComponentsInChildren<RectTransform>())
        {
            t.GetComponent<Card>().curHeap = heap;
            t.GetComponent<Card>().SetOriginalPos(t.localPosition);
        }
    }
   
}
