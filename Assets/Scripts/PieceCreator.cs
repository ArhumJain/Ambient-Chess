using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material whiteMaterial;

    private Dictionary<string, GameObject> nameToPieceDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var i in piecesPrefabs)
        {
            // nameToPieceDict.Add(i.GetComponent<Piece>().GetType().ToString(), i);
            nameToPieceDict.Add(i.name, i);
        }
    }
    public GameObject CreatePiece(string type)
    {
        GameObject prefab = nameToPieceDict[type.ToString()];
        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab);
            return newPiece;
        }
        return null;
    }
    public Material GetTeamMaterial(TeamColor team)
    {
        return team == TeamColor.White ? whiteMaterial : blackMaterial;
    }
}
