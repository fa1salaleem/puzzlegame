using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleImageButton : MonoBehaviour
{
    public string imagePath, imageName;
    public Image myImage;
    public MainMenuController parent;

    public static void LoadImage(string _path, string _imageName, MainMenuController _parent, Transform _parentTransform)
    {
        GameObject GO = Instantiate(Resources.Load("Prefabs/PuzzleButton"),_parentTransform) as GameObject;
        PuzzleImageButton puzzleImageButton = (PuzzleImageButton)GO.GetComponent<PuzzleImageButton>();
        puzzleImageButton.Initialize(_path, _imageName, _parent);
    } 

    public void Initialize(string _path, string _imageName, MainMenuController _parent)
    {
        imagePath = _path;
        imageName = _imageName;
        parent = _parent;

        myImage.sprite = Resources.Load<Sprite>(imagePath + "/" + imageName);
        gameObject.name = imageName;
    }

    public void ButtonClicked()
    {
        Debug.Log("Image clicked = " + imageName);
    }
}
