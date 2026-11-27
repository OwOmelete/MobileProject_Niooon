using System;
using UnityEngine;

public class CustoButton : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public static event Action OnBtnSelected;
    public static event Action OnBtnEquip;

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
        switch (state)
        {
            case BtnState.idle:
                animator.SetTrigger("Deselec");
                break;
            case BtnState.selected:
                OnBtnSelected?.Invoke();
                animator.SetTrigger("Pressed");
                break;
            case BtnState.equiped:
                OnBtnEquip?.Invoke();
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
    
}
