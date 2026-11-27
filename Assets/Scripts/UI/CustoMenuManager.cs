using System;
using UnityEngine;
using UnityEngine.UI;

public class CustoMenuManager : MonoBehaviour
{
    [SerializeField] private Animator WeaponAnimator;
    [SerializeField] private Animator ColorAnimator;
    [SerializeField] private Animator TrailAnimator;
    [SerializeField] private Animator ReadyAnimator;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private float SelectedYSpacing = 83;
    [SerializeField] private float UnselectedYSpacing = 3;
    
    private enum CustoState
    {
        Weapon,
        Color,
        Trail,
        Ready
    }

    private CustoState currentState = CustoState.Weapon;


    private void Start()
    {
        WeaponAnimator.SetTrigger("T_ComingBack");
    }

    public void LeftArrow()
    {
        Debug.Log("left");
        switch (currentState)
        {
            case CustoState.Weapon:
                break;
            case CustoState.Color:
                SwitchState(CustoState.Weapon);
                break;
            case CustoState.Trail:
                SwitchState(CustoState.Color);
                break;
            case CustoState.Ready:
                SwitchState(CustoState.Trail);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void RightArrow()
    {
        Debug.Log("right");
        switch (currentState)
        {
            case CustoState.Weapon:
                SwitchState(CustoState.Color);
                break;
            case CustoState.Color:
                SwitchState(CustoState.Trail);
                break;
            case CustoState.Trail:
                SwitchState(CustoState.Ready);
                
                break;
            case CustoState.Ready:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void SwitchState(CustoState state)
    {
        
        switch (state)
        {
            case CustoState.Weapon:
                
                WeaponAnimator.SetTrigger("T_ComingBack");
                ColorAnimator.SetTrigger("T_OutMenu");
                
                break;
            case CustoState.Color:

                if (currentState == CustoState.Weapon)
                {
                    WeaponAnimator.SetTrigger("T_Leaving");
                    ColorAnimator.SetTrigger("T_OpeningMenu");
                }
                else
                {
                    ColorAnimator.SetTrigger("T_ComingBack");
                    TrailAnimator.SetTrigger("T_OutMenu");
                }
                
                break;
            case CustoState.Trail:
                
                if (currentState == CustoState.Color)
                {
                    ColorAnimator.SetTrigger("T_Leaving");
                    TrailAnimator.SetTrigger("T_OpeningMenu");
                }

                if (currentState == CustoState.Ready)
                {
                    
                    TrailAnimator.SetTrigger("T_ComingBack");
                    ReadyAnimator.SetTrigger("T_BackFromReady");
                    
                }
                break;
            case CustoState.Ready:
                
                ReadyAnimator.SetTrigger("T_Ready");
                TrailAnimator.SetTrigger("T_Leaving");
                
                break;
                
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentState = state;

    }
    
}
