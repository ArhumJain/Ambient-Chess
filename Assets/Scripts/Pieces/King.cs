using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private Vector2Int[] directions = new Vector2Int[] {new Vector2Int(1,1), new Vector2Int(-1,1),new Vector2Int(-1,-1),new Vector2Int(1,-1), Vector2Int.left, Vector2Int.right, Vector2Int.up ,Vector2Int.down};
    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();
        foreach(var direction in directions)
        {
            Vector2Int nextCoords = occupiedSquare + direction;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if(!board.CheckIfCoordinatesAreOnBoard(nextCoords))
            {
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
