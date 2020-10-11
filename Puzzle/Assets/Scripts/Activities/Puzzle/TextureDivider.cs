using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class PuzzlePosition
{
    public Vector3 position;
    public  bool isOccupied;
    public int myIndexInArray;

    public PuzzlePosition(Vector3 _pos, int _index)
    {
        position = _pos;
        myIndexInArray = _index;
    }
}

public class TextureDivider : MonoBehaviour
{
    public GameObject spritesRoot, positionReference, positionReferenceActual;
    public PuzzlePiece[] allPuzzlePieces;
    public Vector3 originalPositionOfPickedObject;
    public GameObject pickedObject;
    public PuzzlePiece currentPuzzlePiece;
    public List<PuzzlePosition> allPositions = new List<PuzzlePosition>();
    public Texture2D puzzleImageSource;
    public int currTopZ = 0;
    public int noOfPieces;
    float pieceWidth, pieceHeight;
    public Camera _camera;
    public Image fullImage;
    public int maxHintsAllowed = 3, hintsGiven = 0;
    public GameObject scrollContent;

    public void DivideTexture(Texture2D source,int pieces, float scale)
    {
        noOfPieces = pieces;
        puzzleImageSource = source;

        allPuzzlePieces = new PuzzlePiece[noOfPieces];

        int rows = 0;
        int coloumns = 0;

        float textureWidth = source.width;
        float textureHeight = source.height;

        if (noOfPieces == 16)
        {
            rows = 4;
            coloumns = 4;
        }
        else if (noOfPieces == 25)
        {
            rows = 5;
            coloumns = 5;
        }
        else if (noOfPieces == 36)
        {
            rows = 6;
            coloumns = 6;
        }
        else if (noOfPieces == 64)
        {
            rows = 8;
            coloumns = 8;
        }
        else if (noOfPieces == 100)
        {
            rows = 10;
            coloumns = 10;
        }

        pieceWidth = textureWidth / coloumns;
        pieceHeight = textureHeight / rows;

        //starting X & Y space from left & above for first piece
        float pixelToUnitRatio = 80.0f;
        float startXPosition = positionReference.gameObject.transform.position.x + (pieceWidth / (2 * pixelToUnitRatio));
        float startYPosition = positionReference.gameObject.transform.position.y - (pieceHeight / (2 * pixelToUnitRatio));
        float startX = startXPosition;
        float startY = startYPosition;

        int puzzlePosIndex = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {
                Vector3 position = new Vector3(startX, startY, 0);
                PuzzlePosition puzzlePos = new PuzzlePosition(position, puzzlePosIndex);
                allPositions.Add(puzzlePos);
                startX += (pieceWidth / pixelToUnitRatio);
                puzzlePosIndex++;
            }
            startX = startXPosition;
            startY -= (pieceHeight / pixelToUnitRatio);
        }

        for (int i = 0; i < rows; i++)
        {
            int index = noOfPieces - (coloumns * (i + 1));
            for (int j = 0; j < coloumns; j++)
            {
                Sprite newSprite = Sprite.Create(source, new Rect(j * pieceWidth, i * pieceHeight, pieceWidth, pieceHeight), new Vector2(0.5f, 0.5f), pixelToUnitRatio);
                GameObject n = new GameObject();
                n.layer = 8;//Gameplay
                SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;
                sr.sortingOrder = 10;
                n.transform.parent = spritesRoot.transform;
                //if (false)//for rotation
                //{
                //    float[] rotations = { 0, 90, 180, 270 };
                //    n.transform.localEulerAngles = new Vector3(0, 0, rotations[Random.Range(0, 4)]);
                //}
                n.name = ""+index;
                n.AddComponent<BoxCollider2D>().isTrigger = true;
                PuzzlePiece puzzlePiece = n.AddComponent<PuzzlePiece>();
                puzzlePiece.textureDivider = this;
                n.AddComponent<Rigidbody2D>().gravityScale = 0;
                allPuzzlePieces[index] = puzzlePiece;
                index++;
            }
        }

        int positionNo = 0;
        foreach (PuzzlePiece puzzlePiece in allPuzzlePieces)
        {
            PuzzlePosition puzzlePosition = allPositions[positionNo] as PuzzlePosition;
            //puzzlePiece.gameObject.transform.position = /*puzzlePosition.position*/ new Vector3(startXPosition + Random.Range(0.0f,10.0f),
            //    startYPosition - Random.Range(10.0f, 16.0f));
            puzzlePiece.myPositionIndex = positionNo;
            puzzlePiece.myPositionObject = puzzlePosition;
            positionNo++;
        }

        allPuzzlePieces.Shuffle();

        float startXPositionActual = positionReferenceActual.gameObject.transform.position.x + (pieceWidth / (2 * pixelToUnitRatio));
        float startYPositionActual = positionReferenceActual.gameObject.transform.position.y;
        float startXActual = startXPositionActual;
        float startYActual = startYPositionActual;
        float XDiff = 0.5f;

        for (int k = 0; k < allPuzzlePieces.Length; k++)
        {
            Vector3 position = new Vector3(startXActual, startYActual, 0);
            PuzzlePiece puzzlePiece = allPuzzlePieces[k] as PuzzlePiece;
            puzzlePiece.gameObject.transform.position = position;
            startXActual += ((pieceWidth / pixelToUnitRatio) + XDiff);

            RectTransform rect = scrollContent.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(rect.anchorMin.x + 0.275f, rect.anchorMin.y);
        }

        spritesRoot.gameObject.transform.SetParent(scrollContent.gameObject.transform);

        fullImage.sprite = Sprite.Create(source, new Rect(0, 0, source.width, source.height), new Vector2(0.5f, 0.5f), pixelToUnitRatio);
    }

    void OnEnable()
    {
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
        FingerGestures.OnFingerUp += HandleOnFingerUp;
        FingerGestures.OnFingerMoveBegin += HandleOnFingerMoveBegin;
        FingerGestures.OnFingerMove += HandleOnFingerMove;
        FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
    }

    void OnDisable()
    {
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerUp -= HandleOnFingerUp;
        FingerGestures.OnFingerMoveBegin -= HandleOnFingerMoveBegin;
        FingerGestures.OnFingerMove -= HandleOnFingerMove;
        FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
    }

    public bool tapEnabled = true;
    public bool touchEnabled = true;

    void FingerGestures_OnFingerTap(int fingerIndex, Vector2 fingerPos, int tapCount)
    {
        if (!tapEnabled) return;
        if (PlayerPrefs.GetInt("IsRotating") == 1)
        {
            GameObject touchedObj = SI_Helper.GetInstance.PickObject(fingerPos, _camera);
            if (touchedObj != null)
            {
                tapEnabled = false;
                touchedObj.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, touchedObj.transform.localEulerAngles.z + 90.0f), 0.25f);
                Invoke("EnableTapAgain", 0.26f);
            }
        }
    }

    void EnableTapAgain()
    {
        tapEnabled = true;
    }

    void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        if (!touchEnabled) return;
        GameObject touchedObj = SI_Helper.GetInstance.PickObject(fingerPos, _camera);
        if (touchedObj != null)
        {
            scrollContent.GetComponentInParent<ScrollRect>().horizontal = false;

            PuzzlePiece pp = touchedObj.GetComponent<PuzzlePiece>();
            if (pp != null & !pp.placed)
            {
                originalPositionOfPickedObject = touchedObj.transform.position;
                pickedObject = touchedObj;
                currentPuzzlePiece = pickedObject.GetComponent<PuzzlePiece>();
                currTopZ--;

                pickedObject.gameObject.transform.SetParent(null);
            }
        }
    }

    //for test temorary solution
    float maxPositonY = 8.5f;

    void HandleOnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
    {
        if (!touchEnabled) return;
        if (pickedObject != null)
        {
            Vector3 position = SI_Helper.GetInstance.GetWorldPositionForCamera(fingerPos, _camera);
            pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);
            //if((position.y) > maxPositonY)
            //{
            //    pickedObject.transform.position = new Vector3(position.x, maxPositonY, currTopZ);
            //}
            //else
            //{
            //    pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);
            //}
        }
    }

    void HandleOnFingerMove(int fingerIndex, Vector2 fingerPos)
    {
        if (!touchEnabled) return;
        if (pickedObject != null)
        {
            Vector3 position = SI_Helper.GetInstance.GetWorldPositionForCamera(fingerPos, _camera);
            pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);
            //if ((position.y) > maxPositonY)
            //{
            //    pickedObject.transform.position = new Vector3(position.x, maxPositonY, currTopZ);
            //}
            //else
            //{
            //    pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);
            //}
        }
    }

    void HandleOnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
        scrollContent.GetComponentInParent<ScrollRect>().horizontal = true;

        if (!touchEnabled) return;
        if (pickedObject != null)
        {
            pickedObject.transform.position = new Vector3(pickedObject.transform.position.x, pickedObject.transform.position.y, currTopZ);
            currentPuzzlePiece.TouchLifted();
        }

        pickedObject = null;
        currentPuzzlePiece = null;
    }

    public bool CheckPosition(Vector3 snapPosition)
    {
        bool occupied = false;
        foreach (PuzzlePiece puzzlePiece in allPuzzlePieces)
        {
            Vector2 snapPositionVector = new Vector2(snapPosition.x, snapPosition.y);
            Vector2 spritePosition = new Vector2(puzzlePiece.gameObject.transform.position.x, puzzlePiece.gameObject.transform.position.y);

            string xSnapPos = snapPosition.x.ToString("0.0");
            string ySnapPos = snapPosition.y.ToString("0.0");

            string xSpritePos = puzzlePiece.gameObject.transform.position.x.ToString("0.0");
            string ySpritePos = puzzlePiece.gameObject.transform.position.y.ToString("0.0");

            if (xSnapPos == xSpritePos && ySnapPos == ySpritePos)
            {
                occupied = true;
                break;
            }
        }
        return occupied;
    }

    bool isEqual(float a, float b)
    {
        if (a >= b - Mathf.Epsilon && a <= b + Mathf.Epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowFullImage()
    {
        fullImage.gameObject.SetActive(!fullImage.gameObject.activeInHierarchy);
    }

    public void HideFullImage()
    {
        fullImage.gameObject.SetActive(false);
    }

    public void GiveHint()
    {
        if(hintsGiven < maxHintsAllowed)
        {
            PuzzlePiece pp = null;
            do
            {
                int randomIndex = Random.Range(0, allPuzzlePieces.Length);
                pp = allPuzzlePieces[randomIndex] as PuzzlePiece;
            }
            while (pp.placed);            
            pp.MoveForHint();
            hintsGiven++;
        }
        else
        {
            Debug.Log("hints limit reached");
        }
    }
}
     