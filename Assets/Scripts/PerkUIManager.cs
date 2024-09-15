using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUIManager : MonoBehaviour
{
    [SerializeField] GameObject perkPrefab;

    int perksToChoose = 3;

    [SerializeField] Queue<PerkData> perkQueue = new Queue<PerkData>();

    int randomIndex;

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

            

            /*newPerkPrefab = Instantiate(perkPrefab, transform.position, transform.rotation); //Instantiates the perk UI GameObject
            newPerkPrefab.transform.SetParent(this.transform);

            Perks newPerk = CreatePerkType(newDataList[randomIndex].type); //Generates a new Perk
            newPerk.Data = newDataList[randomIndex]; //Recieves the data of the perk randomized

            
            
            newPerkPrefab.GetComponent<PerkUI>().FeedDataToUI(newPerk); //Sends the generated Perk to the UIManager to fill data
            newDataList.RemoveAt(randomIndex); //Removes the data from the DataList to avoid repeated perks*/

        }
        InstantiatePerkPrefab();
    }

    void InstantiatePerkPrefab()
    {
        if (this.transform.childCount < perksToChoose)
        {
            for (int i = 0; i < perksToChoose; i++)
            {
                GameObject newPerkPrefab = null;

                newPerkPrefab = Instantiate(perkPrefab, transform.position, transform.rotation); //Instantiates the perk UI GameObject
                newPerkPrefab.transform.SetParent(this.transform);

                PerkData dequeuedPerkData = perkQueue.Dequeue();

                Perks newPerk = CreatePerkType(dequeuedPerkData.type); //Generates a new Perk
                newPerk.Data = dequeuedPerkData; //Recieves the data of the perk randomized

                newPerkPrefab.GetComponent<PerkUI>().FeedDataToUI(newPerk); //Sends the generated Perk to the UIManager to fill data
            }
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
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
    }
}
