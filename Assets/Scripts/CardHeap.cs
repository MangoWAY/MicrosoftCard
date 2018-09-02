using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHeap : MonoBehaviour {
    public Card curCard;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void RefreshCurCard(Card card)
    {
        if (!curCard)
            return;
        if (card.transform.parent.tag == "card")
        {
            curCard = card.transform.parent.GetComponent<Card>();
            if (!curCard.isInteractable)
            {
                curCard.isInteractable = true;
                curCard.GetComponent<Drag>().isEnableDrag = true;
                curCard.SetCardImg(curCard.cardInfo.imgName);
            }
        }
        else
        {
            curCard = null;
        }
    }
}
