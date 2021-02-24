using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    private PieceCreator pieceCreator;
    private ChessPlayer whitePlayer;
    private ChessPlayer blackPlayer;
    private ChessPlayer activePlayer;
    

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
        board.SetDependencies(this);
        CreatePiecesFromLayout(startingBoardLayout);
        activePlayer = whitePlayer;
        GenerateAllPossiblePlayerMoves(activePlayer);
    }
    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoordsAtindex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);
            // Type type = Type.GetType("RookWhite");
            // Debug.Log(type);
            CreatePieceAndInitialize(squareCoords, team, typeName);

        }
    }
    private void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, string type)
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
        ChangeActiveTeam();
        Debug.Log(activePlayer.team);
    }
    private void ChangeActiveTeam()
    {
        activePlayer = activePlayer == whitePlayer ? blackPlayer : whitePlayer;
    }
    private ChessPlayer GetOpponentToPlayer(ChessPlayer player)
    {
        return player == whitePlayer ? blackPlayer : whitePlayer;
    }
}
