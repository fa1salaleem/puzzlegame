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

public class PuzzlePositionInScroll
{
    public Vector3 position;
    public int myIndexInArray;
    public PuzzlePiece myPuzzlePiece;

    public PuzzlePositionInScroll(Vector3 _pos, int _index, PuzzlePiece _puzzlePiece)
    {
        position = _pos;
        myIndexInArray = _index;
        myPuzzlePiece = _puzzlePiece;
    }
}

public class TextureDivider : MonoBehaviour
{
    public GameObject spritesRoot, positionReference, positionReferenceActual;
    public PuzzlePiece[] allPuzzlePieces;
    public PuzzlePositionInScroll[] allPuzzlePiecesPositionsInScroll; 
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
    public float scrollOutY = -4.2f;
    public PuzzleController puzzleController;
    public int totalPiecesLeftInScroll;

    public void DivideTexture(Texture2D source,int pieces, float scale)
    {        
        noOfPieces = pieces;
        puzzleImageSource = source;
        maxHintsAllowed = noOfPieces;

        allPuzzlePieces = new PuzzlePiece[noOfPieces];
        allPuzzlePiecesPositionsInScroll = new PuzzlePositionInScroll[noOfPieces];

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

        //computing actual grid positions of puzzle pieces outside scroll
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

        //making puzzle pieces
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

        //assigning actual grid positions to puzzle pieces
        int positionNo = 0;
        foreach (PuzzlePiece puzzlePiece in allPuzzlePieces)
        {
            PuzzlePosition puzzlePosition = allPositions[positionNo] as PuzzlePosition;
            puzzlePiece.myPositionObject = puzzlePosition;
            positionNo++;
        }

        //shuffling pieces
        allPuzzlePieces.Shuffle();

        //calculating in scroll view positions
        float startXPositionActual = positionReferenceActual.gameObject.transform.position.x + (pieceWidth / (2 * pixelToUnitRatio));
        float startYPositionActual = positionReferenceActual.gameObject.transform.position.y;
        float startXActual = startXPositionActual;
        float startYActual = startYPositionActual;
        float XDiff = 0.75f;
        float fixedPieceWidthInScroll = 200.0f;
        float scaleToBe = fixedPieceWidthInScroll / pieceWidth;

        for (int k = 0; k < allPuzzlePieces.Length; k++)
        {
            Vector3 position = new Vector3(startXActual, startYActual, 0);
            PuzzlePiece puzzlePiece = allPuzzlePieces[k] as PuzzlePiece;
            puzzlePiece.scrollScale = scaleToBe;
            puzzlePiece.inScroll = true;
            puzzlePiece.SetLocalScale(scaleToBe);
            puzzlePiece.gameObject.transform.position = position;
            puzzlePiece.myPositionObjectInScroll = allPuzzlePiecesPositionsInScroll[k] = new PuzzlePositionInScroll(position,k,puzzlePiece);
            startXActual += ((pieceWidth * scaleToBe / pixelToUnitRatio) + XDiff);
            RectTransform rect = scrollContent.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(rect.anchorMax.x + 0.26f, rect.anchorMax.y);
        }

        spritesRoot.gameObject.transform.SetParent(scrollContent.gameObject.transform);
        totalPiecesLeftInScroll = noOfPieces;
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
            if (touchedObj != null && touchedObj.name != "PositionReferenceActual")
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
        if (touchedObj != null && touchedObj.name != "PositionReferenceActual")
        {
            scrollContent.GetComponentInParent<ScrollRect>().horizontal = false;

            PuzzlePiece pp = touchedObj.GetComponent<PuzzlePiece>();
            if (pp != null & !pp.placeOnActualGrid)
            {
                originalPositionOfPickedObject = touchedObj.transform.position;
                pickedObject = touchedObj;
                currentPuzzlePiece = pickedObject.GetComponent<PuzzlePiece>();
                if (currentPuzzlePiece.myPositionObjectInScroll != null)
                {
                    currentPuzzlePiece.myPositionObjectInScroll.position = originalPositionOfPickedObject;
                }
                currTopZ--;
            }
        }
    }

    void HandleOnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
    {
        if (!touchEnabled) return;

        if (pickedObject != null)
        {
            Vector3 position = SI_Helper.GetInstance.GetWorldPositionForCamera(fingerPos, _camera);
            pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);
        }
    }

    void HandleOnFingerMove(int fingerIndex, Vector2 fingerPos)
    {
        if (!touchEnabled) return;

        if (pickedObject != null)
        {
            Vector3 position = SI_Helper.GetInstance.GetWorldPositionForCamera(fingerPos, _camera);
            pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);

            //In & Out of scroll based on position
            if (pickedObject.transform.position.y > scrollOutY)//out
            {
                MovePuzzlePieceOutOfScroll(pickedObject);           
            }
            else if(pickedObject.transform.position.y <= scrollOutY)//in
            {
                PuzzlePiece pp = pickedObject.GetComponent<PuzzlePiece>();
                if(!pp.inScroll)
                {
                    pp.SetLocalScale(pp.scrollScale);
                    pp.inScroll = true;
                }
            }
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

            if (currentPuzzlePiece.inScroll)
            {
                //inserting in scroll on base of if it has left scroll already
                if (currentPuzzlePiece.outOfScrollOnce)
                {
                    //increment this when add the piece in scroll again
                    //totalPiecesLeftInScroll++;
                    //set this when add piece in scroll again
                    //currentPuzzlePiece.outOfScrollOnce = false;
                }//inserting in scroll on base of if it has not left scroll
                else
                {
                    pickedObject.gameObject.transform.position = currentPuzzlePiece.myPositionObjectInScroll.position;
                    pickedObject.gameObject.transform.SetParent(this.gameObject.transform);
                }
            }
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
            while (pp.placeOnActualGrid);

            if(pp.inScroll)
            {
                if (pp.myPositionObjectInScroll != null)
                {
                    pp.myPositionObjectInScroll.position = pp.gameObject.transform.position;
                }
                MovePuzzlePieceOutOfScroll(pp.gameObject);
            }

            pp.MoveForHint();
            hintsGiven++;
        }
        else
        {
            Debug.Log("hints limit reached");
        }
    }

    public void MovePuzzlePieceOutOfScroll(GameObject _pickedObject)
    {
        PuzzlePiece pp = _pickedObject.GetComponent<PuzzlePiece>();
        if (pp.inScroll)
        {
            pp.SetLocalScale(1.0f);
            pp.inScroll = false;
            if (!pp.outOfScrollOnce)
            {
                pp.outOfScrollOnce = true;
                _pickedObject.gameObject.transform.SetParent(puzzleController.gameObject.transform);

                for (int i = pp.myPositionObjectInScroll.myIndexInArray; i < allPuzzlePiecesPositionsInScroll.Length; i++)
                {
                    if (i == (totalPiecesLeftInScroll - 1))
                    {
                        //last valid piece in scroll
                        PuzzlePositionInScroll puzzlePositionInScroll1 = (PuzzlePositionInScroll)allPuzzlePiecesPositionsInScroll[i];
                        puzzlePositionInScroll1.myPuzzlePiece = null;
                        break;
                    }
                    else
                    {
                        PuzzlePositionInScroll puzzlePositionInScroll1 = (PuzzlePositionInScroll)allPuzzlePiecesPositionsInScroll[i];
                        PuzzlePositionInScroll puzzlePositionInScroll2 = (PuzzlePositionInScroll)allPuzzlePiecesPositionsInScroll[i + 1];
                        puzzlePositionInScroll2.position = puzzlePositionInScroll2.myPuzzlePiece.gameObject.transform.position;
                        puzzlePositionInScroll1.myPuzzlePiece = puzzlePositionInScroll2.myPuzzlePiece;
                        puzzlePositionInScroll1.myPuzzlePiece.myPositionObjectInScroll = puzzlePositionInScroll1;
                        puzzlePositionInScroll1.myPuzzlePiece.gameObject.transform.DOMove(puzzlePositionInScroll1.position, 0.5f, false);
                    }
                }
                totalPiecesLeftInScroll--;
                pp.myPositionObjectInScroll = null;
            }
        }
    }
}
     