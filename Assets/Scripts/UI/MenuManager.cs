using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Player player1;
    [SerializeField] private Player player2;

    [SerializeField] private WeaponData[] weaponDatas;

    [SerializeField] private GameObject ButtonPrefab;
    [SerializeField] private Transform P1UiParent;
    [SerializeField] private Transform P2UiParent;
    
    
    [SerializeField] private float RoundTransitionDuration;

    [SerializeField] private Animator RoundTransitionBackground;
    [SerializeField] private Animator RoundTransitionText;

    private Button[] buttonsP1;
    private Button[] buttonsP2;

    private UnityAction action;
    
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private GameObject RoundTransitionUI;

    [SerializeField] private GameObject CustoMenu;

    [SerializeField] private CustoMenuManager CustoPlayer1;
    [SerializeField] private CustoMenuManager CustoPlayer2;

    [SerializeField] private GameObject PlayButton;

    private int readycount;
    
    public static event Action OnRoundTransitionEnd;
    
    private void OnEnable()
    {
        RoundManager.OnRoundEnd += PlayRoundTransition;
        CustoMenuManager.Ready += Ready;
        CustoMenuManager.UnReady += Unready;
    }

    private void OnDisable()
    {
        RoundManager.OnRoundEnd -= PlayRoundTransition;
        CustoMenuManager.Ready -= Ready;
        CustoMenuManager.UnReady -= Unready;
    }

    private void Start()
    {
        //UpdateButtons();
    }

    public void Ready()
    {
        readycount++;
        if (readycount >= 2)
        {
            PlayButton.SetActive(true);
        }
    }

    public void Unready()
    {
        readycount--;
        if (readycount < 2)
        {
            PlayButton.SetActive(false);
        }
    }

    public void MainToGame()
    {
        MainMenuUI.SetActive(false);

        StartCoroutine(RoundTransition());
    }

    public void MainToPrep()
    {
        CustoMenu.SetActive(true);
        
        readycount = 0;
        PlayButton.SetActive(false);
        
        MainMenuUI.SetActive(false);
        
        CustoPlayer1.gameObject.SetActive(true);
        CustoPlayer2.gameObject.SetActive(true);
        CustoPlayer1.ResetMenu();
        CustoPlayer2.ResetMenu();
        
    }

    public void PrepToGame()
    {

        CustoMenu.SetActive(false);
        
        CustoPlayer1.gameObject.SetActive(false);
        CustoPlayer2.gameObject.SetActive(false);
        
        PlayButton.SetActive(false);
        
        StartCoroutine(RoundTransition());
    }
    
    public void GameToMain()
    {
        InGameUI.SetActive(false);
        MainMenuUI.SetActive(true);
        GameManager.INSTANCE.SwitchGameState(GameManager.GameState.MainMenu);
    }
    
    
    public void EquipWeapon(int i, int p)
    {
        if (p == 1)
        {
            player1.GetWeaponStats(weaponDatas[i]);
        }
    }
    
    void UpdateButtons()
    {
        for (int i = 0; i < weaponDatas.Length; i++)
        {
            GameObject go = Instantiate(ButtonPrefab, P1UiParent);

            Button btn = go.GetComponent<Button>();
            
            Debug.Log(btn);
            
            //btn.onClick.AddListener(delegate { EquipWeapon(i, 1); });
            btn.onClick.AddListener(delegate { EquipWeapon(i, 1); });
            
            
            
            btn = Instantiate(ButtonPrefab, P2UiParent).GetComponent<Button>();
            btn.onClick.AddListener(delegate { EquipWeapon(i, 2); });
        }
    }

    private void PlayRoundTransition()
    {
        StartCoroutine(RoundTransition());
    }

    IEnumerator RoundTransition()
    {
        RoundTransitionUI.SetActive(true);
        
        RoundTransitionBackground.SetTrigger("In");
        RoundTransitionText.SetTrigger("In");
        
        yield return new WaitForSeconds(RoundTransitionDuration);
        
        RoundTransitionBackground.SetTrigger("Out");
        RoundTransitionText.SetTrigger("Out");

        yield return new WaitForSeconds(RoundTransitionText.GetCurrentAnimatorStateInfo(0).length);
        
        RoundTransitionUI.SetActive(false);
        
        InGameUI.SetActive(true);
        
        OnRoundTransitionEnd?.Invoke();
    }
}