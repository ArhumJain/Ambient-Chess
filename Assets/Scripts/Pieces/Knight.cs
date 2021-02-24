using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    private Vector2Int[] moves = new Vector2Int[] {new Vector2Int(1,2), new Vector2Int(-1,2), new Vector2Int(-2,1), new Vector2Int(2,1), new Vector2Int(-2,-1), new Vector2Int(2,-1),new Vector2Int(-1,-2),new Vector2Int(1,-2)};
    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        foreach(var move in moves)
        {
            Vector2Int nextCoords = occupiedSquare + move;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if(!board.CheckIfCoordinatesAreOnBoard(nextCoords)){
                continue;
            }
            if(piece==null||!piece.isFromSameTeam(this))
            {
                TryToAddMove(nextCoords);
            }
        }
        return availableMoves;
    }
}
