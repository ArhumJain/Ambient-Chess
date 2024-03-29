﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IObjectTweener))]
[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(AudioOutputController))]
public abstract class Piece : MonoBehaviour
{
    private MaterialSetter materialSetter;
    public Board board {protected get; set;}
    public Vector2Int occupiedSquare {get; set;}
    public TeamColor team {get; set;}
    public bool hasMoved {get; private set;}
    public List<Vector2Int> availableMoves;
    public bool canDoEnPassant;
    private IObjectTweener tweener;
    private AudioOutputController audioOutput;
    public abstract List<Vector2Int> SelectAvailableSquares();
    public bool isEnPassantable = false;

    private void Awake()
    {
        availableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweener>();
        materialSetter = GetComponent<MaterialSetter>();
        audioOutput = GetComponent<AudioOutputController>();
        hasMoved = false;
    }
    public void SetMaterial(Material material)
    {
        if (materialSetter == null)
        {
            materialSetter = GetComponent<MaterialSetter>();
        }
        materialSetter.SetSingleMaterial(material);
    }
    public bool IsAttackingPieceOfType<T>() where T : Piece
    {
        foreach(var square in availableMoves)
        {
            if(board.GetPieceOnSquare(square) is T)
            {
                return true;
            }
        }
        return false;
    }
    public bool isFromSameTeam(Piece piece)
    {
        return team == piece.team;
    }
    public bool CanMoveTo(Vector2Int coords)
    {
        return availableMoves.Contains(coords);
    }
    public virtual void MovePiece(Vector2Int coords)
    {
        Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
        occupiedSquare = coords;
        hasMoved = true;
        tweener.MoveTo(transform, targetPosition);
        audioOutput.PlayMoveSound();
    }
    protected void TryToAddMove(Vector2Int coords)
    {
        availableMoves.Add(coords);
    } 
    public void SetData(Vector2Int coords, TeamColor team, Board board)
    {
        this.team = team;
        occupiedSquare = coords;
        this.board = board;
        transform.position = board.CalculatePositionFromCoords(coords);
    }
}
