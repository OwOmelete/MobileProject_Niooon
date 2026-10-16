using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Animator trailBehaviour;
    [SerializeField] private GameObject dust;
    [SerializeField] private GameObject stars;

    private SpriteRenderer sr;

    public void ToStop()
    {
        trailBehaviour.SetBool("Stop", true);
        dust.SetActive(false);
        stars.SetActive(false);
    }

    public void ToLow()
    {
        trailBehaviour.SetBool("Low", true);
        dust.SetActive(false);
        stars.SetActive(false);
    }

    public void ToMedium()
    {
        trailBehaviour.SetBool("Mid", true);
        dust.SetActive(true);
        stars.SetActive(false);
    }

    public void ToHigh()
    {
        trailBehaviour.SetBool("High", true);
        dust.SetActive(true);
        stars.SetActive(true);
    }

    public void ResetAnimBools()
    {
        foreach (var param in trailBehaviour.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                trailBehaviour.SetBool(param.name, false);
            }
        }
    }

}
