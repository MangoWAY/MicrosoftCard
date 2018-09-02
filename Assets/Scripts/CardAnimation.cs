using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CardAnimation : MonoBehaviour {


    private void Start()
    {
        Init();
    }
    void Init()
    {
        GetComponent<CardController>().cardAniEvent += handleAniEvent;
    }

    void handleAniEvent(object sender, EventArgs args)
    {
        cardAniArgs e = args as cardAniArgs;
        e.cardRt.DOLocalMove(e.targetPos, e.duration);
    }
}
