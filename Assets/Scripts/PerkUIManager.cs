using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PerkUIManager : MonoBehaviour
{
    [SerializeField] PerkUI perkPrefab;

    int perksToChoose = 3;

    [SerializeField] Queue<PerkData> perkQueue = new Queue<PerkData>();

    int randomIndex;

    List<PerkUI> perkUIlist = new List<PerkUI>();

    public static PerkUIManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
    }

    private void OnEnable()
    {
        Actions.OnLevelUp += PerkSelection;
        Actions.OnWaveWon += PerkSelection;
    }

    private void OnDisable()
    {
        Actions.OnLevelUp -= PerkSelection;
        Actions.OnWaveWon -= PerkSelection;
    }

    void PerkSelection()
    {
        List<PerkData> newDataList = new List<PerkData>(PerkManager.Instance.PerkDataList); //Declares a List of PerkData to generate one at random

        for (int i = 0; i < perksToChoose; i++)
        {
            //GameObject newPerkPrefab = null;
            randomIndex = Random.Range(0, newDataList.Count); //Generates random number between 0 and the count of total perks available

            perkQueue.Enqueue(newDataList[randomIndex]);

            newDataList.RemoveAt(randomIndex); //Removes the data from the DataList to avoid repeated perks
        }
        InstantiatePerkPrefab();
    }

    void InstantiatePerkPrefab()
    {
        if (this.transform.childCount < perksToChoose)
        {
            for (int i = 0; i < perksToChoose; i++)
            {
                PerkUI newPerkPrefab = null;

                //newPerkPrefab = Instantiate(perkPrefab, transform.position, transform.rotation); //Instantiates the perk UI GameObject

                newPerkPrefab = ObjectPoolManager.Instance.GetFromPool(perkPrefab); //Gets or Creates a new Perk UI GameObject from Object Pool
                newPerkPrefab.transform.SetParent(this.transform);

                PerkData dequeuedPerkData = perkQueue.Dequeue(); //Stores the Perk Data from the Queue and Removes it from Queue

                Perks newPerk = CreatePerkType(dequeuedPerkData.type); //Generates a new Perk

                newPerk.Data = dequeuedPerkData; //Recieves the data of the perk randomized

                newPerkPrefab.FeedDataToUI(newPerk); //Sends the generated Perk to the UIManager to fill data

                perkUIlist.Add(newPerkPrefab);
            }
            PlayPerkInAnimation();
        }
    }

    public bool CheckEmptyQueue()
    {
        if(perkQueue.Count > 0)
        {
            InstantiatePerkPrefab();
            return false;
        }
        return true;
    }

    Perks CreatePerkType(PerkData.Type type)
    {
        Perks newPerk = null;
        switch (type)
        {
            case PerkData.Type.ExtraSummon:
                newPerk = new ExtraSummon();
                break;

            case PerkData.Type.AddHealth:
                newPerk = new PlayerExtraHealth();
                break;

            case PerkData.Type.AddSummonHealth:
                newPerk = new SummonExtraHealth();
                break;

            case PerkData.Type.ExtraMoveSpeed:
                newPerk = new ExtraMoveSpeed();
                break;

            case PerkData.Type.FasterCast:
                newPerk = new FasterCast();
                break;

            case PerkData.Type.CastCoolDown:
                newPerk = new CastCoolDown();
                break;

            default:
                break;
        }

        return newPerk;

    }

    public void DestroyPerkPrefabs()
    {
        for (int i = perksToChoose - 1; i >= 0; i--)
        {
            //DestroyImmediate(this.transform.GetChild(i).gameObject);
            PerkUI perkToDestroy = this.transform.GetChild(i).GetComponent<PerkUI>();
            ObjectPoolManager.Instance.AddToPool(perkToDestroy);
            perkToDestroy.transform.SetParent(null, false);
        }
        perkUIlist.Clear();
    }

    public void PlayPerkInAnimation()
    {
        foreach (var perk in perkUIlist)
        {
            perk.transform.localScale = Vector3.zero;
        }
        for (int i= 0; i < perkUIlist.Count; i++)
        {
            perkUIlist[i].transform.DOScale(1f, 1f).SetEase(Ease.OutBounce).SetUpdate(true).SetDelay(0.1f * i);
        }
    }

    public void PlayPerkOutAnimation()
    {
        for (int i = 0; i < perkUIlist.Count; i++)
        {
            if (i == 2)
            {
                perkUIlist[i].transform.DOScale(0f, 0.5f).OnComplete(() => PerkIsSelected()).SetUpdate(true);
            }
            else
            {
                perkUIlist[i].transform.DOScale(0f, 0.5f).SetUpdate(true);
            }
        }
        
    }

    public void PerkIsSelected()
    {
        DestroyPerkPrefabs();

        if (CheckEmptyQueue() == true)
        {
            GameManager.Instance.ClosePerkSelectionScreen();
        }
    }
}
