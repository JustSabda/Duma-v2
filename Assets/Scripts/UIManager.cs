using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public int objCount;

    [SerializeField]
    private GameObject objective;

    [SerializeField]
    public Image healthImage;

    [SerializeField]
    private GameObject panelWin;

    [SerializeField]
    public GameObject panelLose;

    private static bool isPaused;
    // Start is called before the first frame update


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        if(panelWin != null)
        {
            panelWin.SetActive(false);
        }
        if (panelLose != null)
        {
            panelLose.SetActive(false);
        }
    }

    

    // Update is called once per frame
    private void Update()
    {
       

        objCount = GameManager.Instance.Objective;

        if(objective != null)
        {
            switch (objCount)
            {
                case 0:
                    objective.transform.GetChild(0).gameObject.SetActive(false);
                    objective.transform.GetChild(1).gameObject.SetActive(false);
                    objective.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case 1:
                    objective.transform.GetChild(0).gameObject.SetActive(true);
                    objective.transform.GetChild(1).gameObject.SetActive(false);
                    objective.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case 2:
                    objective.transform.GetChild(0).gameObject.SetActive(true);
                    objective.transform.GetChild(1).gameObject.SetActive(true);
                    objective.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case 3:
                    objective.transform.GetChild(0).gameObject.SetActive(true);
                    objective.transform.GetChild(1).gameObject.SetActive(true);
                    objective.transform.GetChild(2).gameObject.SetActive(true);

                    panelWin.SetActive(true);
                    break;
            }
        }

        if(GameManager.Instance.isWin == true && panelWin != null)
        {
            panelWin.SetActive(true);
        }

        if (GameManager.Instance.isGameOver == true && panelLose != null)
        {
            panelLose.SetActive(true);
        }

    }

    public void UI_Health(float value, float max)
    {
        healthImage.fillAmount = GameManager.Instance.GetPercent(value, max);

    }

}
