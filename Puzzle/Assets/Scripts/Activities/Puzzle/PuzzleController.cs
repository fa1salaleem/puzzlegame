using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    public TextureDivider textureDivider;
    public int puzzlePieces;
    public Sprite puzzleSprite;
    public Image image;
    public Camera _camera;

    public static PuzzleController MakePuzzle()
    {
        GameObject gameObject = Instantiate(Resources.Load("Prefabs/Puzzle")) as GameObject;
        PuzzleController puzzle = gameObject.GetComponent<PuzzleController>();
        puzzle.Initialize();
        return puzzle;
    }


    private void Initialize()
	{
        textureDivider._camera = _camera;
        puzzlePieces = 9;
        string filePath = "Images/Eagle";

        if(filePath != null)
        {
            //StartCoroutine(Download(urlFile));
            LoadPuzzleImage(filePath);
        }
    }

    void LoadPuzzleImage(string filePath)
    {
        Sprite sp = Resources.Load<Sprite>(filePath);
        textureDivider.DivideTexture(sp.texture, puzzlePieces, 1);
    }

    IEnumerator Download(string imageUrl)
    {
        yield return null;
        string downloadURL = imageUrl;
        //downloadURL = DownloadHelper.GenerateURLForFile(downloadURL, DownloadHelper.FileOperation.k_Load);
        WWW www = new WWW(downloadURL);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            yield break;
        }
        else
        {
            puzzleSprite = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = puzzleSprite;
            image.SetNativeSize();
            image.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

            textureDivider.DivideTexture(www.texture, puzzlePieces, 1);

            //save the texture from server
            //DownloadHelper.SaveBytesAsFile(imageUrl, www.bytes);
        }

        www.Dispose();
        www = null;
        yield return null;
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
