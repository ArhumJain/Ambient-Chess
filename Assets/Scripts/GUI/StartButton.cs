using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class StartButton : ButtonBehavior
{
    [SerializeField] private ChessGameController chessController;
    // Start is called before the first frame update
    void Start()
    {
        defaultScale = this.transform.localScale;
    }
    public override void OnClick()
    {
        chessController.StartNewGame();
        Debug.Log("MADE IT HERE ATLEAST!");
        chessController.activePlayer = chessController.whitePlayer;
        chessController.GenerateAllPossiblePlayerMoves(chessController.activePlayer);
        FadeOut();
        gameObject.SetActive(false);
        // Destroy(gameObject);
        
    }
    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     transform.localScale = new Vector3(0.2f,0.2f,0.2f);
    //     hoverSound.Play();
    //     Debug.Log("LOLOLOLOLOLOLOLOL");
    // }
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     this.transform.localScale = defaultScale;
    // }
}
