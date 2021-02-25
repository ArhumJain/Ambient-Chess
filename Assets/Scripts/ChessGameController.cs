using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]
[RequireComponent(typeof(AudioOutputController))]
// [RequireComponent(typeof(AudioOutputController))]
public class ChessGameController : MonoBehaviour
{
    private enum GameState {Init, Play, Finished}
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    [SerializeField] private AudioOutputController audioOutput;
    private PieceCreator pieceCreator;
    private ChessPlayer whitePlayer;
    private ChessPlayer blackPlayer;
    private ChessPlayer activePlayer;
    private GameState state;
    

    private void Awake()
    {
        SetDependencies();
        CreatePlayers();
    }
    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }
    private void CreatePlayers()
    {
        whitePlayer = new ChessPlayer(TeamColor.White, board);
        blackPlayer = new ChessPlayer(TeamColor.Black, board);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
        activePlayer = whitePlayer;
        GenerateAllPossiblePlayerMoves(activePlayer);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void StartNewGame(){
        SetGameState(GameState.Init);
        board.SetDependencies(this);
        CreatePiecesFromLayout(startingBoardLayout);
        activePlayer = whitePlayer;
        GenerateAllPossiblePlayerMoves(activePlayer);
        SetGameState(GameState.Play);
    }
    private void SetGameState(GameState state)
    {
        this.state = state;
    }
    public bool IsGameInProgress()
    {
        return state == GameState.Play;
    }
    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoordsAtindex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);
            CreatePieceAndInitialize(squareCoords, team, typeName);

        }
    }
    public void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, string type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, team, board);

        Material teamMaterial = pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);
        board.SetPieceOnBoard(squareCoords, newPiece);

        ChessPlayer currentPlayer = team == TeamColor.White ? whitePlayer : blackPlayer;
        currentPlayer.AddPiece(newPiece);
    }
    private void GenerateAllPossiblePlayerMoves(ChessPlayer player)
    {
        player.GenerateAllPossibleMoves();
    }
    public bool IsTeamTurnActive(TeamColor team)
    {
        return activePlayer.team == team;
    }
    public void EndTurn()
    {
        Debug.Log("ENDING TURN!");
        GenerateAllPossiblePlayerMoves(activePlayer);
        GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(activePlayer));
        if (CheckIfGameIsFinished())
        {
            EndGame();
        }
        else
            ChangeActiveTeam();
        Debug.Log(activePlayer.team);
    }
    private bool CheckIfGameIsFinished()
    {
        Piece[] kingAttackingPieces = activePlayer.GetPiecesAttackingOppositePieceOfType<King>();
        if(kingAttackingPieces.Length > 0)
        {
            ChessPlayer oppositePlayer = GetOpponentToPlayer(activePlayer);
            Piece attackedKing = oppositePlayer.GetPiecesOfType<King>().FirstOrDefault();
            oppositePlayer.RemoveMovesEnablingAttackOnPiece<King>(activePlayer, attackedKing);

            int availableKingMoves = attackedKing.availableMoves.Count;
            if(availableKingMoves==0)
            {
                bool canCoverKing = oppositePlayer.CanHidePieceFromAttack<King>(activePlayer);
                if(!canCoverKing)
                    return true;
            }
        }
        return false;
    }
    public void OnPieceRemoved(Piece piece)
    {
        ChessPlayer pieceOwner = (piece.team == TeamColor.White) ? whitePlayer : blackPlayer;
        pieceOwner.RemovePiece(piece);
        Destroy(piece.gameObject);
        audioOutput.PlayCaptureSound();
    }
    private void EndGame()
    {
        Debug.Log("Game Ended!");
        SetGameState(GameState.Finished);
    }
    private void ChangeActiveTeam()
    {
        activePlayer = activePlayer == whitePlayer ? blackPlayer : whitePlayer;
    }
    private ChessPlayer GetOpponentToPlayer(ChessPlayer player)
    {
        return player == whitePlayer ? blackPlayer : whitePlayer;
    }
    public void RemoveMovesEnablingAttackOnPieceOfType<T>(Piece piece) where T : Piece
	{
        activePlayer.RemoveMovesEnablingAttackOnPiece<T>(GetOpponentToPlayer(activePlayer), piece);
    }
}
