using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class StartButton : ButtonBehavior
{
    [SerializeField] private ChessGameController chessController;
    [SerializeField] private Animator gameStartAnimation;
    // Start is called before the first frame update
    void Start()
    {
        defaultScale = this.transform.localScale;
    }
    public override void OnClick()
    {
        chessController.StartNewGame();
        chessController.activePlayer = chessController.whitePlayer;
        chessController.GenerateAllPossiblePlayerMoves(chessController.activePlayer);
        gameStartAnimation.SetBool("changeViewToChessBoard", true);
        FadeOut();
        gameObject.SetActive(false);        
    }
}
