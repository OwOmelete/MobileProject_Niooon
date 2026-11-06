using System;
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

    private Button[] buttonsP1;
    private Button[] buttonsP2;

    private UnityAction action;
    
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject InGameUI;
    
    
    

    private void Start()
    {
        //UpdateButtons();
        
        
        
        
        
    }

    public void MainToGame()
    {
        MainMenuUI.SetActive(false);
        InGameUI.SetActive(true);
        
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

    void googoo()
    {
        return;
    }

    void UpdateButtons()
    {
        for (int i = 0; i < weaponDatas.Length; i++)
        {


            GameObject go = Instantiate(ButtonPrefab, P1UiParent);

            Button btn = go.GetComponent<Button>();
            
            Debug.Log(btn);
            
            //btn.onClick.AddListener(delegate { EquipWeapon(i, 1); });
            btn.onClick.AddListener(googoo);
            
            
            
            btn = Instantiate(ButtonPrefab, P2UiParent).GetComponent<Button>();
            btn.onClick.AddListener(googoo);
        }
    }

    

}