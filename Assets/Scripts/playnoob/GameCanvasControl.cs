using PathCreation.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    None,
    LevelStarted,
    LevelFailed,
    LevelCompleted,
    CharaterSelection
}


public class GameCanvasControl : MonoBehaviour
{
    public static GameCanvasControl Instance;

    public GameState gameState;// = GameState.CharaterSelection;

    public GameplayPanelControl gameplayPanelControl;
    public GameOverPanelControl gameOverPanelControl;
    public GameObject panCharacterSelection;

    public Button btnPrev, btn_Next;

    public event Action OnNextCharacter;
    public event Action OnPrevCharacter;    
    

    private void Awake()
    {
        Instance = this;
        //gameState = GameState.CharaterSelection;
    }

    private void OnEnable()
    {
        gameOverPanelControl.ResetPanel();
        //gameState = GameState.CharaterSelection;
    }

    private void OnValidate()
    {
        //gameState = GameState.CharaterSelection;
    }
    public void OnTouched()
    {
        gameplayPanelControl.GameStarted();
        //Debug.Log("OnTouched!");
    }

    public void OnTouchedWithdraw()
    {

    }

    private void Update()
    {
        /*
        btnPrev.onClick.AddListener(btnPrevClicked);
        btn_Next.onClick.AddListener(() =>
        {
            Debug.Log("Next Button clicked!");
            OnNextCharacter?.Invoke();
        });
        */
    }

   public void OnBtnPrevClicked()
    {
        //Debug.Log("Prev Button Cliked!");
        OnPrevCharacter?.Invoke();
    }

    public void OnBtnNextClicked()
    {
        //Debug.Log("Next button Clicked!");
        OnNextCharacter?.Invoke();
    }

    public void OnBtnOk()
    {
        panCharacterSelection.SetActive(false);
        gameState = GameState.None;
    }

    //private void Start()
    //{
    //    //TODO: test
    //    gameOverPanelControl.EnablePanel(GameState.LevelFailed, 1.5f);
    //}

}
