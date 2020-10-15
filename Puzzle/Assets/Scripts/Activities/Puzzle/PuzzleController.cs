using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    public TextureDivider textureDivider;
    public Camera _camera;
    public int puzzlePieces;
    public string puzzleImagePath;   

    public static PuzzleController MakePuzzle(int _pieces, string _puzzleImagePath)
    {
        GameObject gameObject = Instantiate(Resources.Load("Prefabs/PuzzleV1")) as GameObject;
        PuzzleController puzzle = gameObject.GetComponent<PuzzleController>();
        puzzle.puzzlePieces = _pieces;
        puzzle.puzzleImagePath = _puzzleImagePath;
        puzzle.Initialize();
        return puzzle;
    }

    private void Initialize()
	{
        textureDivider._camera = _camera;
        if(puzzleImagePath != null)
        {
            LoadPuzzleImage(puzzleImagePath);
        }
    }

    void LoadPuzzleImage(string filePath)
    {
        Sprite sp = Resources.Load<Sprite>(filePath);
        textureDivider.DivideTexture(sp.texture, puzzlePieces, 1);
    }

    public void Submit()
    {
        textureDivider.tapEnabled = false;
        textureDivider.touchEnabled = false;
    }

    public void ShowFullImage()
    {

    }

    public void HideFullImage()
    {

    }
}
