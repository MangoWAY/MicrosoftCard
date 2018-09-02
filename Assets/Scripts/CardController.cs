using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardController : MonoBehaviour {

    public event EventHandler cardAniEvent;

    private GlobalManager m_gloManager;

    private Card m_selectedCard;
    private RectTransform m_selectedCardRt;
    private CardInfo m_selectedCardInfo;

    private CardHeap m_curHeap;
    private RectTransform m_curHeapRt;

    private Card m_newParentCard;
    private RectTransform m_newParentCardRt;
    private GameObject[] m_allCard;


    private void Start()
    {
        m_allCard = GameObject.FindGameObjectsWithTag("card");
        m_gloManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        for(int i=0;i<m_allCard.Length;i++)
        {
            m_allCard[i].GetComponent<Drag>().RightMouseDownEvent += HandleRightMouseDownEvent;
        }
    }


    bool OrderCardRule(Card underCard,Card overCard)
    {
        
        if(underCard==null)
        {
            if (overCard.cardInfo.num == 1)
                return true;
            else
                return false;
        }
        else
        {
            if (overCard.cardInfo.num - overCard.cardInfo.num == 1 && overCard.cardInfo.color == underCard.cardInfo.color)
                return true;
            else
                return false;
        }
    }
    bool UnorderCardRule(Card underCard, Card overCard)
    {
        if (underCard == null)
        {
            if (overCard.cardInfo.num == 1)
                return true;
            else
                return false;
        }
        else
        {
            if (overCard.cardInfo.num - underCard.cardInfo.num == -1 && overCard.cardInfo.isRed != underCard.cardInfo.isRed)
                return true;
            else
                return false;
        }
    }

    void HandleRightMouseDownEvent(object sender, EventArgs e)
    {
        GameObject orderheap = m_gloManager.orderHeap;
        m_selectedCard = (sender as GameObject).GetComponent<Card>();
        m_selectedCardInfo = m_selectedCard.cardInfo;
        m_selectedCardRt = m_selectedCard.GetComponent<RectTransform>();

        foreach (CardHeap h in orderheap.GetComponentsInChildren<CardHeap>())
        {
            if(!OrderCardRule(h.curCard, m_selectedCard))
                continue;

            m_curHeapRt = m_gloManager.GetHeapRcTrans(h.name);
            m_curHeap = m_curHeapRt.GetComponent<CardHeap>();
            m_newParentCard = m_curHeap.curCard;

            //将原来牌堆当前牌替换
            m_selectedCard.curHeap.RefreshCurCard(m_selectedCard);

            //处理新牌堆
            if (!m_newParentCard)
            {
                m_selectedCardRt.SetParent(m_curHeapRt, false);
                if (cardAniEvent != null)
                    cardAniEvent(this, new cardAniArgs(
                        new Vector2(0, -68), 0.5f, m_selectedCardRt));
            }
            else
            {
                m_selectedCardRt.SetParent(m_newParentCard.GetComponent<RectTransform>(), false);
                if (cardAniEvent != null)
                    cardAniEvent(this, new cardAniArgs(
                        Vector2.zero, 0.5f, m_selectedCardRt));
            }
            Card overCard = null;
            foreach (Transform t in m_selectedCardRt.GetComponentsInChildren<Transform>())
            {
                overCard = t.GetComponent<Card>();
            }
            m_selectedCard.curHeap = m_curHeap;
            m_curHeap.curCard = overCard;
            m_selectedCard.SetCurHeap(m_curHeap);
        }
    }
    void HanleEndDragEvent(object sender, EventArgs e)
    {
        if (!m_gloManager.GetHeapRcTrans(m_gloManager.heapname))
        {

            if (cardAniEvent != null)
                cardAniEvent(this, new cardAniArgs(m_originalPos, 0.5f, m_rt));
            return;
        }

        RectTransform heapRt = m_gloManager.GetHeapRcTrans(m_gloManager.heapname);
        CardHeap tempHeap = heapRt.GetComponent<CardHeap>();
        Card tempCard = tempHeap.curCard;

        if (heapRt.tag == "orderheap")
        {
            if ((m_gloManager.heapname == curHeap.name) || (!tempCard && cardInfo.num != 1) ||
                (tempCard && ((tempCard.cardInfo.num != cardInfo.num - 1) || (tempCard.cardInfo.color != cardInfo.color))))
            {
                if (cardAniEvent != null)
                    cardAniEvent(this, new cardAniArgs(m_originalPos, 0.5f, m_rt));
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
                    cardAniEvent(this, new cardAniArgs(m_originalPos, 0.5f, m_rt));
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


}
