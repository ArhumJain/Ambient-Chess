using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;
    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    private Piece selectedPiece;
    private ChessGameController chessController;
    private SquareSelectorCreator squareSelector;
    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }
    public void SetDependencies(ChessGameController chessController)
    {
        this.chessController = chessController;
    }
    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }
    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }
    public Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(inputPosition.x / squareSize) + BOARD_SIZE / 2;
        int y = Mathf.FloorToInt(inputPosition.z / squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x, y);
    }
    public void OnSquareSelected(Vector3 inputPosition)
    {
        if (!chessController.IsGameInProgress())
        {
            return;
        }
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Debug.Log(coords);
        Piece piece = GetPieceOnSquare(coords);
        Debug.Log(piece);
        if (selectedPiece)
        {
            if(piece != null && selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && selectedPiece != piece && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
            else if(selectedPiece.CanMoveTo(coords))
            {
                OnSelectedPieceMoved(coords, selectedPiece);
            }
        }
        else
        {
            if(piece!=null && chessController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
        }
    }
    private void SelectPiece(Piece piece)
    {
        chessController.RemoveMovesEnablingAttackOnPieceOfType<King>(piece);
        selectedPiece = piece;
        List<Vector2Int> selection = selectedPiece.availableMoves;
        ShowSelectionSquares(selection);

    }
    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squareData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squareData.Add(position, isSquareFree);   
        }
        squareSelector.ShowSelection(squareData);
    }
    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }
    private void OnSelectedPieceMoved(Vector2Int coords, Piece piece)
    {
        TryToTakeOppositePiece(coords);   
        UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
        selectedPiece.MovePiece(coords);
        DeselectPiece();
        EndTurn();
    }
    private void TryToTakeOppositePiece(Vector2Int coords)
    {
        Piece piece = GetPieceOnSquare(coords);
        if(piece != null && !selectedPiece.isFromSameTeam(piece))
        {
            TakePiece(piece);
        }
    }
    private void TakePiece(Piece piece)
    {
        if(piece)
        {
            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            chessController.OnPieceRemoved(piece);
        }
    }
    private void EndTurn()
    {
        chessController.EndTurn();
    }
    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        // if(GetPieceOnSquare(newCoords) != null && !newPiece.isFromSameTeam(GetPieceOnSquare(newCoords)))
        // {
        //     // FadeCoroutine(GetPieceOnSquare(newCoords));
        //     Piece.Destroy(GetPieceOnSquare(newCoords).gameObject);
        // }
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }
    // Some fade animation stuff I was trying to get to work
    // private void FadeCoroutine(Piece piece)
    // {
    //     StartCoroutine(DestroyFade(true, piece));
    // }
    // IEnumerator DestroyFade(bool fadeAway, Piece piece)
    // {
    //     Renderer rend = piece.GetComponent<Renderer>();
    //     rend.material = piece.team == TeamColor.White ? PieceCreator.blackFadeMaterial : PieceCreator.whiteFadeMaterial;
    //     Color objectColor = piece.GetComponent<Renderer>().material.color;

    //     for (float i = 1; i >= 0; i -= Time.deltaTime)
    //     {
            
    //         piece.GetComponent<Renderer>().material.color = new Color(objectColor.r, objectColor.g, objectColor.b, i);
    //         yield return null;
    //     }
    //     Piece.Destroy(piece.gameObject);
    // }
    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if(CheckIfCoordinatesAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }
    public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
    {
        if(coords.x < 0 || coords.y < 0  || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }
    public bool HasPiece(Piece piece)
    {
        for (int i =0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if(grid[i,j] == piece)
                    return true;
            }
        }
        return false;
    }
    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if(CheckIfCoordinatesAreOnBoard(coords))
        {
            grid[coords.x, coords.y] = piece;
        }
    }
}
