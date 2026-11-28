using System;
using UnityEngine;

public class CustoButton : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public event Action<CustoButton> OnBtnSelected;
    public event Action<CustoButton> OnBtnEquip;

    public enum BtnState
    {
        idle,
        selected,
        equiped,
        idle_equiped
    }


    private BtnState currentState = BtnState.idle;



    public void SwitchState(BtnState state)
    {
        if (state == currentState) return;
        
        switch (state)
        {
            case BtnState.idle:
                animator.SetTrigger("Deselec");
                break;
            case BtnState.selected:
                OnBtnSelected?.Invoke(this);
                animator.SetTrigger("Pressed");
                break;
            case BtnState.equiped:
                OnBtnEquip?.Invoke(this);
                animator.SetTrigger("Equipped");
                break;
            case BtnState.idle_equiped:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentState = state;
    }

    public void ButtonClicked()
    {
        SwitchState(BtnState.selected);
    }

    public void selected()
    {
        SwitchState(BtnState.equiped);
    }

    public void deselec()
    {
        SwitchState(BtnState.idle);
    }
    
}
