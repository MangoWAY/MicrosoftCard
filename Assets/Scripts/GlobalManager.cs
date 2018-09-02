using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour {
    public string heapname;
    public RectTransform[] heaps;
    public SpriteAtlas cardsSprites;
    public GameObject orderHeap;

    private Dictionary<string, RectTransform> m_heapDic;
    private GameObject m_unorderCardHeap;
    private GameObject m_newCardHeap;
    private CardInfo[] m_cards;
    private List<int> m_randomList;
    private string[] m_colorList= { "A","B","C","D"};
    private bool[] m_boolList = { false, true, false, true };
 
    // Use this for initialization

    void Init()
    {
        m_heapDic = new Dictionary<string, RectTransform>();
        m_cards = new CardInfo[52];
        m_unorderCardHeap = GameObject.Find("unorderCardHeap");
        m_newCardHeap = GameObject.Find("newCardHeap");
        orderHeap = GameObject.Find("orderCardHeap");

        for (int i = 0; i < heaps.Length; i++)
        {
            m_heapDic.Add(heaps[i].name, heaps[i]);
        }
        for(int i=0;i<52;i++)
        {
            m_cards[i] = new CardInfo();
            m_cards[i].num = (i % 13)+1;
            m_cards[i].color = m_colorList[(i % 4)];
            m_cards[i].isUsed = false;
            m_cards[i].imgName = m_cards[i].num + m_cards[i].color;
            m_cards[i].isRed = m_boolList[(i % 4)];
        }
        InitRandomList();

        int index = 0;
        foreach(Card c in m_unorderCardHeap.GetComponentsInChildren<Card>())
        {
            c.SetCardInfo(m_cards[m_randomList[index]]);
            if (c.isInteractable)
            {
                c.GetComponent<Drag>().isEnableDrag = true;
                c.SetCardImg(c.cardInfo.imgName);
            }               
            index++;
        }
        foreach(Card c in m_newCardHeap.GetComponentsInChildren<Card>())
        {
            c.SetCardInfo(m_cards[m_randomList[index]]);
            if (c.isInteractable)
            {
                c.GetComponent<Drag>().isEnableDrag = true;
                c.SetCardImg(c.cardInfo.imgName);
            }
                
            index++;
        }
        
    }
    void Start () {

        Init();
	}
	public RectTransform GetHeapRcTrans(string heapName)
    {
        if (!m_heapDic.ContainsKey(heapName))
            return null;
        return m_heapDic[heapName];
    }

    private void InitRandomList()
    {
        m_randomList = new List<int>();        
        while(m_randomList.Count<52)
        {
            int temp = Random.Range(0, 52);
            if(!m_randomList.Contains(temp))
            {
                m_randomList.Add(temp);
            }
        }
    }
    public void changeCard()
    {
        if(m_newCardHeap.transform.childCount==0)
        {
            return;
        }
        m_newCardHeap.transform.GetChild(m_newCardHeap.transform.childCount - 1).SetSiblingIndex(0);
    }
    public void resetGame()
    {
        SceneManager.LoadScene("0");
    }
}
