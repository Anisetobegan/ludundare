
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI summonName1;
    [SerializeField] private TextMeshProUGUI summonName2;
    [SerializeField] private TextMeshProUGUI summonName3;
    [SerializeField] private TextMeshProUGUI summonName4;
    [SerializeField] private TextMeshProUGUI summonName5;

    [SerializeField] private Image summonImage1;
    [SerializeField] private Image summonImage2;
    [SerializeField] private Image summonImage3;
    [SerializeField] private Image summonImage4;
    [SerializeField] private Image summonImage5;

    [SerializeField] private Sprite ghostImage;
    [SerializeField] private Sprite skeletonImage;
    [SerializeField] private Sprite zombieImage;

    [SerializeField] private GameObject summonIndicator1;
    [SerializeField] private GameObject summonIndicator2;
    [SerializeField] private GameObject summonIndicator3;
    [SerializeField] private GameObject summonIndicator4;
    [SerializeField] private GameObject summonIndicator5;

    private int i = 0;

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
    }

    public void UpdateSummon(string summon, int summonCount)
    {
                
        switch (i)
        {
            case 0:
                if (summonIndicator1.activeSelf == false)
                {                    
                    UpdateSummonImage(summon, summonCount);
                    UpdateSummonName(summon, summonCount);
                    summonIndicator1.SetActive(true);
                    i++;
                }
                break;
            case 1:
                if (summonIndicator2.activeSelf == false)
                {
                    UpdateSummonImage(summon, summonCount);
                    UpdateSummonName(summon, summonCount);
                    summonIndicator2.SetActive(true);
                    i++;
                }
                break;
            case 2:
                if (summonIndicator3.activeSelf == false)
                {
                    UpdateSummonImage(summon, summonCount);
                    UpdateSummonName(summon, summonCount);
                    summonIndicator3.SetActive(true);
                    i++;
                }
                break;
            case 3:
                if (summonIndicator4.activeSelf == false)
                {
                    UpdateSummonImage(summon, summonCount);
                    UpdateSummonName(summon, summonCount);
                    summonIndicator4.SetActive(true);
                    i++;
                }
                break;
            case 4:
                if (summonIndicator5.activeSelf == false)
                {
                    UpdateSummonImage(summon, summonCount);
                    UpdateSummonName(summon, summonCount);
                    summonIndicator5.SetActive(true);
                }
                break;
            }
        
    }

    public void UpdateSummonImage(string summon, int summonCount)
    {
        
            switch (i)
            {
                case 0:
                    switch (summon)
                    {
                        case "Cube":
                            summonImage1.sprite = ghostImage;
                            break;

                        case "Sphere":
                            summonImage1.sprite = skeletonImage;
                            break;

                        case "Capsule":
                            summonImage1.sprite = zombieImage;
                            break;

                    }
                    break;
                case 1:
                    switch (summon)
                    {
                        case "Cube":
                            summonImage2.sprite = ghostImage;
                            break;

                        case "Sphere":
                            summonImage2.sprite = skeletonImage;
                            break;

                        case "Capsule":
                            summonImage2.sprite = zombieImage;
                            break;

                    }
                    break;
                case 2:
                    switch (summon)
                    {
                        case "Cube":
                            summonImage3.sprite = ghostImage;
                            break;

                        case "Sphere":
                            summonImage3.sprite = skeletonImage;
                            break;

                        case "Capsule":
                            summonImage3.sprite = zombieImage;
                            break;

                    }
                    break;
                case 3:
                    switch (summon)
                    {
                        case "Cube":
                            summonImage4.sprite = ghostImage;
                            break;

                        case "Sphere":
                            summonImage4.sprite = skeletonImage;
                            break;

                        case "Capsule":
                            summonImage4.sprite = zombieImage;
                            break;

                    }
                    break;
                case 4:
                    switch (summon)
                    {
                        case "Cube":
                            summonImage5.sprite = ghostImage;
                            break;

                        case "Sphere":
                            summonImage5.sprite = skeletonImage;
                            break;

                        case "Capsule":
                            summonImage5.sprite = zombieImage;
                            break;

                    }
                    break;
            

            /*switch (summon)
            {
                case "Ghost":
                    summonImage.sprite = ghostImage;
                    break;

                case "Skeleton":
                    summonImage.sprite = skeletonImage;
                    break;

                case "Zombie":
                    summonImage.sprite = zombieImage;
                    break;

            }*/

        }
    }

    public void UpdateSummonName(string name, int summonCount)
    {
        
            switch (i)
            {
                case 0:
                switch (name)
                {
                    case "Cube":
                        summonName1.text = "Ghost";
                        break;

                    case "Sphere":
                        summonName1.text = "Skeleton";
                        break;

                    case "Capsule":
                        summonName1.text = "Zombie";
                        break;

                }
                    break;
                case 1:
                switch (name)
                {
                    case "Cube":
                        summonName2.text = "Ghost";
                        break;

                    case "Sphere":
                        summonName2.text = "Skeleton";
                        break;

                    case "Capsule":
                        summonName2.text = "Zombie";
                        break;

                }
                break;
                case 2:
                switch (name)
                {
                    case "Cube":
                        summonName3.text = "Ghost";
                        break;

                    case "Sphere":
                        summonName3.text = "Skeleton";
                        break;

                    case "Capsule":
                        summonName3.text = "Zombie";
                        break;

                }
                break;
                case 3:
                switch (name)
                {
                    case "Cube":
                        summonName4.text = "Ghost";
                        break;

                    case "Sphere":
                        summonName4.text = "Skeleton";
                        break;

                    case "Capsule":
                        summonName4.text = "Zombie";
                        break;

                }
                break;
                case 4:
                switch (name)
                {
                    case "Cube":
                        summonName5.text = "Ghost";
                        break;

                    case "Sphere":
                        summonName5.text = "Skeleton";
                        break;

                    case "Capsule":
                        summonName5.text = "Zombie";
                        break;

                }
                break;
            }
                
        
    }

    public void ClearSelectedSummons()
    {
        summonIndicator1.SetActive(false);
        summonIndicator2.SetActive(false);
        summonIndicator3.SetActive(false);
        summonIndicator4.SetActive(false);
        summonIndicator5.SetActive(false);
        i = 0;
    }

    

}
