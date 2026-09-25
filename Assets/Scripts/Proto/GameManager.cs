using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerControllerProto[] playerList;


    private void Start()
    {
        /*for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].g = Gamepad.all[i];
        }*/
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

}
