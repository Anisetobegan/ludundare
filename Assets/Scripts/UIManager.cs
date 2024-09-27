
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    [SerializeField] private List<Sprite> spriteList;

    private Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    [SerializeField] private GameObject summonIndicator;

    [SerializeField] private List<GameObject> summonIndicatorList;

    [SerializeField] private GameObject SummonPanelUI;

    [SerializeField] GameObject PlayerPanelUI;    
    [SerializeField] TextMeshProUGUI playerLevelTextMeshPro;
    [SerializeField] TextMeshProUGUI playerExpTextMeshPro;

    [SerializeField] Button resumeButton;
    [SerializeField] Button retryButton;
    [SerializeField] Button returnButton;

    public static UIManager Instance
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

        resumeButton.onClick.AddListener(GameManager.Instance.Resume);
        retryButton.onClick.AddListener(GameManager.Instance.Retry);
        returnButton.onClick.AddListener(GameManager.Instance.ReturnToMainManu);
    }

    private void Start()
    {
        spriteDictionary.Add("Ghost", spriteList[0]);
        spriteDictionary.Add("Skeleton", spriteList[1]);
        spriteDictionary.Add("Zombie", spriteList[2]);
    }

    public void UpdateSummon(Summon selectedSummon)
    {

        GameObject newSummonIndicator = Instantiate(summonIndicator, transform.position, transform.rotation);
        newSummonIndicator.transform.SetParent(SummonPanelUI.transform);

        summonIndicatorList.Add(newSummonIndicator);

        UpdateSummonImage(selectedSummon.GetSummonName(), newSummonIndicator.gameObject.GetComponentInChildren<Image>());
        UpdateSummonName(selectedSummon.GetSummonName(), newSummonIndicator.gameObject.GetComponentInChildren<TextMeshProUGUI>());
    }

    private void UpdateSummonImage(string summonName, Image instantiatedImage)
    {
        instantiatedImage.sprite = spriteDictionary[summonName];        
    }

    private void UpdateSummonName(string summonName, TextMeshProUGUI summonTextMesh)
    {
        summonTextMesh.text = summonName;
    }

    public void ClearSelectedSummon(Summon selectedSummon)
    {
        if (summonIndicatorList.Count > 0)
        {
            GameObject newSummonIndicator = GetSummonIndicator(selectedSummon);
            summonIndicatorList.Remove(newSummonIndicator);
            GameObject.Destroy(newSummonIndicator);
        }
    }

    GameObject GetSummonIndicator(Summon selectedSummon)
    {
        GameObject newSummonIndicator = null;

        for (int i = 0; i < summonIndicatorList.Count; ++i) 
        {
            if (selectedSummon.GetSummonName() == summonIndicatorList[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text)
            {
                newSummonIndicator = summonIndicatorList[i];
                return newSummonIndicator;
            }
        }
        return newSummonIndicator;
    }

    public void ClearSelectedSummons()
    {
        if (summonIndicatorList.Count > 0)
        {
            for (int i = 0; i < summonIndicatorList.Count; i++)
            {
                GameObject.Destroy(summonIndicatorList[i]);
            }
            summonIndicatorList.Clear();                    
        }
    }

    public void UpdatePlayerLevel(int playerLvl)
    {
        playerLevelTextMeshPro.text = "Level: " + playerLvl.ToString();
    }

    public void UpdatePlayerExp(float playerExp, float playerLevelUpExp)
    {
        playerExpTextMeshPro.text = "Exp: " + playerExp.ToString() + "/" + playerLevelUpExp.ToString();
    }
}
