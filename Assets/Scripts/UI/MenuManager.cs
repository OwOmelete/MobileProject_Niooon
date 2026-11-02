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
    [SerializeField] private Transform P1Parent;
    [SerializeField] private Transform P2Parent;

    private Button[] buttonsP1;
    private Button[] buttonsP2;

    private UnityAction action;

    private void Start()
    {
        UpdateButtons();
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


            GameObject go = Instantiate(ButtonPrefab, P1Parent);

            Button btn = go.GetComponent<Button>();
            
            Debug.Log(btn);
            
            //btn.onClick.AddListener(delegate { EquipWeapon(i, 1); });
            btn.onClick.AddListener(googoo);
            
            
            
            btn = Instantiate(ButtonPrefab, P2Parent).GetComponent<Button>();
            btn.onClick.AddListener(googoo);
        }
    }

    

}