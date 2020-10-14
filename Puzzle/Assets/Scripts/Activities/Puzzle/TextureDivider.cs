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
                    float minDis = 100000.0f;
                    PuzzlePiece nearestPuzzlePiece = null;
                    foreach (PuzzlePositionInScroll pp in allPuzzlePiecesPositionsInScroll)
                    {
                        if (pp.myPuzzlePiece != null)
                        {
                            if (pp.myPuzzlePiece.inScroll && !pp.myPuzzlePiece.outOfScrollOnce)
                            {
                                float distance = Vector2.Distance(new Vector2(currentPuzzlePiece.gameObject.transform.position.x, currentPuzzlePiece.gameObject.transform.position.y),
                                    new Vector2(pp.myPuzzlePiece.gameObject.transform.position.x, pp.myPuzzlePiece.gameObject.transform.position.y));
                                if (distance < minDis && (currentPuzzlePiece.transform.position.x < pp.myPuzzlePiece.transform.position.x))
                                {
                                    minDis = distance;
                                    nearestPuzzlePiece = pp.myPuzzlePiece;
                                }
                            }
                        }
                    }

                    if (nearestPuzzlePiece != null)
                    {
                        PuzzlePiece lastNotNullPuzzlePiece = null;
                        int indexOfFirstNullPuzzlePiece = 0;
                        foreach (PuzzlePositionInScroll pzp in allPuzzlePiecesPositionsInScroll)
                        {
                            if (pzp.myPuzzlePiece != null)
                            {
                                lastNotNullPuzzlePiece = pzp.myPuzzlePiece;
                            }
                            else
                            {
                                break;
                            }
                            indexOfFirstNullPuzzlePiece++;
                        }

                        int finalIndex = nearestPuzzlePiece.myPositionObjectInScroll.myIndexInArray;
                        for (int i = indexOfFirstNullPuzzlePiece; i > finalIndex; i--)
                        {
                            PuzzlePositionInScroll pzpInScrollfront = (PuzzlePositionInScroll)allPuzzlePiecesPositionsInScroll[i];
                            PuzzlePositionInScroll pzpInScrollback = (PuzzlePositionInScroll)allPuzzlePiecesPositionsInScroll[i-1];
                            pzpInScrollfront.myPuzzlePiece = pzpInScrollback.myPuzzlePiece;
                            pzpInScrollfront.myPuzzlePiece.myPositionObjectInScroll = pzpInScrollfront;
                            float pixelToUnitRatio = 80.0f;
                            float XDiff = 0.75f;
                            float fixedPieceWidthInScroll = 200.0f;
                            float scaleToBe = fixedPieceWidthInScroll / pieceWidth;
                            float startYPositionActual = positionReferenceActual.gameObject.transform.position.y;
                            Vector3 pos = new Vector3(pzpInScrollfront.myPuzzlePiece.transform.position.x + ((pieceWidth * scaleToBe / pixelToUnitRatio) + XDiff),
                                startYPositionActual, 0);
                            pzpInScrollfront.myPuzzlePiece.gameObject.transform.DOMove(pos, 0.35f, false);
                            pzpInScrollfront.position = pos;

                            if(i == (finalIndex + 1))//lastobject
                            {
                                pzpInScrollback.myPuzzlePiece = currentPuzzlePiece;
                                pzpInScrollback.myPuzzlePiece.myPositionObjectInScroll = pzpInScrollback;
                                pickedObject.gameObject.transform.SetParent(this.gameObject.transform);
                                totalPiecesLeftInScroll++;
                                currentPuzzlePiece.outOfScrollOnce = false;
                                RectTransform rect = scrollContent.GetComponent<RectTransform>();
                                rect.anchorMax = new Vector2(rect.anchorMax.x + 0.26f, rect.anchorMax.y);               
                                Vector3 position = new Vector3(pos.x - ((pieceWidth * scaleToBe / pixelToUnitRatio) + XDiff),
                                    startYPositionActual, 0);
                                currentPuzzlePiece.gameObject.transform.DOMove(position, 0.35f, false);
                                pzpInScrollback.myPuzzlePiece.myPositionObjectInScroll.position = position;
                            }
                        }
                    }
                    else
                    {
                        PuzzlePiece lastNotNullPuzzlePiece = null;
                        int indexOfFirstNullPuzzlePiece = 0;
                        foreach (PuzzlePositionInScroll pzp in allPuzzlePiecesPositionsInScroll)
                        {
                            if (pzp.myPuzzlePiece != null)
                            {
                                lastNotNullPuzzlePiece = pzp.myPuzzlePiece;
                            }
                            else
                            {
                                break;
                            }
                            indexOfFirstNullPuzzlePiece++;
                        }

                        PuzzlePositionInScroll pzpInScroll = (PuzzlePositionInScroll)allPuzzlePiecesPositionsInScroll[indexOfFirstNullPuzzlePiece];
                        pzpInScroll.myPuzzlePiece = currentPuzzlePiece;
                        pzpInScroll.myPuzzlePiece.myPositionObjectInScroll = pzpInScroll;
                        pickedObject.gameObject.transform.SetParent(this.gameObject.transform);
                        totalPiecesLeftInScroll++;
                        currentPuzzlePiece.outOfScrollOnce = false;
                        RectTransform rect = scrollContent.GetComponent<RectTransform>();
                        rect.anchorMax = new Vector2(rect.anchorMax.x + 0.26f, rect.anchorMax.y);

                        float pixelToUnitRatio = 80.0f;
                        float XDiff = 0.75f;
                        float fixedPieceWidthInScroll = 200.0f;
                        float scaleToBe = fixedPieceWidthInScroll / pieceWidth;
                        float startXPositionActual = positionReferenceActual.gameObject.transform.position.x + (pieceWidth / (2 * pixelToUnitRatio));
                        float startYPositionActual = positionReferenceActual.gameObject.transform.position.y;
                        Vector3 pos = Vector3.zero;
                        if (lastNotNullPuzzlePiece != null)
                        {
                            pos = new Vector3(lastNotNullPuzzlePiece.transform.position.x + ((pieceWidth * scaleToBe / pixelToUnitRatio) + XDiff),
                            startYPositionActual, 0);
                        }
                        else
                        {
                            pos = new Vector3(startXPositionActual,startYPositionActual, 0);
                        }                        
                        currentPuzzlePiece.gameObject.transform.DOMove(pos, 0.35f, false);
                        pzpInScroll.myPuzzlePiece.myPositionObjectInScroll.position = pos;
                    }

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

    public bool hintTouchAllowed = true;
    public void GiveHint()
    {
        if (hintTouchAllowed)
        {
            hintTouchAllowed = false;
            if (hintsGiven < maxHintsAllowed)
            {
                PuzzlePiece pp = null;
                do
                {
                    int randomIndex = Random.Range(0, allPuzzlePieces.Length);
                    pp = allPuzzlePieces[randomIndex] as PuzzlePiece;
                }
                while (pp.placeOnActualGrid);

                if (pp.inScroll)
                {//for reordering if hint is chosen from scroll
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
            Invoke("AllowHintTouch", 0.4f);
        }
    }

    public void AllowHintTouch()
    {
        hintTouchAllowed = true;
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
                        puzzlePositionInScroll1.myPuzzlePiece.gameObject.transform.DOMove(puzzlePositionInScroll1.position, 0.35f, false);
                    }
                }
                totalPiecesLeftInScroll--;
                pp.myPositionObjectInScroll = null;
                RectTransform rect = scrollContent.GetComponent<RectTransform>();
                rect.anchorMax = new Vector2(rect.anchorMax.x - 0.26f, rect.anchorMax.y);
            }
        }
    }
}
     