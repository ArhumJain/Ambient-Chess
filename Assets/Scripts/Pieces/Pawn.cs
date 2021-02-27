using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    // public bool isEnPassantable = false;
    private float rangeForEnPassant;
    public override List<Vector2Int> SelectAvailableSquares()
    {
        availableMoves.Clear();

        Vector2Int direction = team == TeamColor.White ? Vector2Int.up : Vector2Int.down;
        float range = hasMoved ? 1 : 2; 
        rangeForEnPassant = range;
        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = occupiedSquare + direction * i;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
                break;
            if (piece == null)
                TryToAddMove(nextCoords);
            else
                break;
        }

        Vector2Int[] takeDirections = new Vector2Int[] { new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y) };
        for (int i = 0; i < takeDirections.Length; i++)
        {
            Vector2Int nextCoords = occupiedSquare + takeDirections[i];
            Piece piece = board.GetPieceOnSquare(nextCoords);
            System.Type pawnType = typeof(Pawn);
            if (!board.CheckIfCoordinatesAreOnBoard(nextCoords))
            {
                continue;
            }
            Vector2Int enPassantCheck = new Vector2Int(occupiedSquare.x+takeDirections[i].x, occupiedSquare.y);
            try
            {
                if(board.CheckIfCoordinatesAreOnBoard(new Vector2Int(occupiedSquare.x+1, occupiedSquare.y)) && board.GetPieceOnSquare(enPassantCheck).isEnPassantable && !board.GetPieceOnSquare(enPassantCheck).isFromSameTeam(this))
                {
                    TryToAddMove(new Vector2Int(occupiedSquare.x+takeDirections[i].x, occupiedSquare.y + direction.y));
                    canDoEnPassant = true;
                }
            }
            catch {
            }
            if (piece != null && !piece.isFromSameTeam(this))
            {
                TryToAddMove(nextCoords);
            }
        }
        return availableMoves;
    }
    public override void MovePiece(Vector2Int coords)
    {
        if (System.Math.Max(coords.y, occupiedSquare.y) - System.Math.Min(coords.y, occupiedSquare.y) == 2)
        {
            isEnPassantable = true;
        }
        base.MovePiece(coords);
        CheckPromotion();
    }
    private void CheckPromotion()
    {
        int endOfBoardYCoord = team == TeamColor.White ? Board.BOARD_SIZE - 1 : 0;
        if (occupiedSquare.y == endOfBoardYCoord)
        {
            board.PromotePiece(this);
        }
    }
}
