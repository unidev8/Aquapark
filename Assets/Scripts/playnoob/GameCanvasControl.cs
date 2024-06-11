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

    public GameState gameState = GameState.CharaterSelection;

    public GameplayPanelControl gameplayPanelControl;
    public GameOverPanelControl gameOverPanelControl;
    public GameObject panCharacterSelection;
    public GameObject characterCam;
    public RawImage img_Character;

    //public Button btnPrev, btn_Next;

    public event Action OnNextCharacter;
    public event Action OnPrevCharacter;
    private RenderTexture renderTextrue;
   

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        renderTextrue = new RenderTexture(256, 256, 24);
        characterCam.GetComponent<Camera>().targetTexture = renderTextrue;
        img_Character.texture = renderTextrue;
        characterCam.GetComponent<Camera>().Render();
    }

    private void OnEnable()
    {
        gameOverPanelControl.ResetPanel();
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
        characterCam.GetComponent<Camera>().Render();
    }

    public void OnBtnNextClicked()
    {
        //Debug.Log("Next button Clicked!");
        OnNextCharacter?.Invoke();
        characterCam.GetComponent<Camera>().Render();
    }

    public void OnBtnOk()
    {
        panCharacterSelection.SetActive(false);
        gameState = GameState.None;
        characterCam.SetActive(false);
    }

    //private void Start()
    //{
    //    //TODO: test
    //    gameOverPanelControl.EnablePanel(GameState.LevelFailed, 1.5f);
    //}

}
