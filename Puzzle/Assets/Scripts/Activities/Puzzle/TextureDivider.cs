using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TextureDivider : MonoBehaviour
{
    public GameObject spritesRoot;
    public ArrayList allSprites = new ArrayList();

    public Vector3 originalPositionOfPickedObject;
    public GameObject pickedObject;
    public PuzzlePiece currentPuzzlePiece;

    public Texture2D puzzleImageSource;
    public int currTopZ = 0;
    public int noOfPieces;
    float pieceWidth, pieceHeight;    

    public void DivideTexture(Texture2D source,int pieces, float scale)
    {
        noOfPieces = pieces;
        puzzleImageSource = source;

        int rows = 0;
        int coloumns = 0;

        float textureWidth = source.width;
        float textureHeight = source.height;

        if (textureWidth > textureHeight) //horizontal image
        {
            if(noOfPieces == 2)
            {
                rows = 1;
                coloumns = 2;
            }
            else if(noOfPieces == 3)
            {
                rows = 1;
                coloumns = 3;
            }
            else if (noOfPieces == 4)
            {
                rows = 2;
                coloumns = 2;
            }
            else if (noOfPieces == 6)
            {
                rows = 2;
                coloumns = 3;
            }
            else if (noOfPieces == 9)
            {
                rows = 3;
                coloumns = 3;
            }
            else if (noOfPieces == 12)
            {
                rows = 3;
                coloumns = 4;
            }
            else if (noOfPieces == 15)
            {
                rows = 3;
                coloumns = 5;
            }
        }
        else
        {
            if (noOfPieces == 2)
            {
                rows = 2;
                coloumns = 1;
            }
            else if (noOfPieces == 3)
            {
                rows = 3;
                coloumns = 1;
            }
            else if (noOfPieces == 4)
            {
                rows = 2;
                coloumns = 2;
            }
            else if (noOfPieces == 6)
            {
                rows = 3;
                coloumns = 2;
            }
            else if (noOfPieces == 9)
            {
                rows = 3;
                coloumns = 3;
            }
            else if (noOfPieces == 12)
            {
                rows = 4;
                coloumns = 3;
            }
            else if (noOfPieces == 15)
            {
                rows = 5;
                coloumns = 3;
            }
        }

        pieceWidth = textureWidth / coloumns;
        pieceHeight = textureHeight / rows;

        SI_Helper.GetInstance.CalculateScreenWidthHeight();

        //starting X & Y space from left & above for first piece
        float startXPositionOffset = 1.25f;
        float startYPositionOffset = 2.1f;
        float pixelToUnitRatio = 200.0f;

        float spaceToCoverX = (SI_Helper.GetInstance.screenWidthInWorldPoints - startXPositionOffset);
        float textureSpaceX = textureWidth / pixelToUnitRatio;
        float leftOverX = Mathf.Abs(spaceToCoverX - textureSpaceX);
        float xDiff = leftOverX / (coloumns - 1);

        float spaceToCoverY = (SI_Helper.GetInstance.screenHeightInWorldPoints - startYPositionOffset);
        float textureSpaceY = textureHeight / pixelToUnitRatio;
        float leftOverY = Mathf.Abs(spaceToCoverY - textureSpaceY);
        float yDiff = leftOverY / (rows - 1);

        float startXPosition = (-SI_Helper.GetInstance.screenWidthInWorldPoints / 2) + startXPositionOffset;
        float startYPosition = (SI_Helper.GetInstance.screenHeightInWorldPoints / 2) - startYPositionOffset;

        float startX = startXPosition;
        float startY = startYPosition;

        ArrayList allPositions = new ArrayList();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {
                startX = startX + (pieceWidth / (2 * pixelToUnitRatio));
                Vector3 position = new Vector3(startX, startY, 0);
                allPositions.Add(position);
                startX += xDiff;
            }
            startX = startXPosition;
            startY = startY - (pieceHeight / pixelToUnitRatio) - yDiff;
        }
        allPositions.Shuffle();

        int positionNo = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {
                Sprite newSprite = Sprite.Create(source, new Rect(j * pieceWidth, i * pieceHeight, pieceWidth, pieceHeight), new Vector2(0.5f, 0.5f), pixelToUnitRatio);
                GameObject n = new GameObject();
                SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
                sr.sprite = newSprite;
                sr.sortingOrder = 10;
                n.transform.parent = spritesRoot.transform;
                allSprites.Add(n);
                n.transform.position = (Vector3)allPositions[positionNo];
                if (true)
                {
                    float[] rotations = { 0, 90, 180, 270 };
                    n.transform.localEulerAngles = new Vector3(0, 0, rotations[Random.Range(0, 4)]);
                }
                positionNo++;
                n.name = ""+i+"_"+j;
                n.AddComponent<BoxCollider2D>().isTrigger = true;
                PuzzlePiece puzzlePiece = n.AddComponent<PuzzlePiece>();
                puzzlePiece.textureDivider = this;
                n.AddComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
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
        if (true)
        {
            GameObject touchedObj = SI_Helper.GetInstance.PickObject(fingerPos);
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
        GameObject touchedObj = SI_Helper.GetInstance.PickObject(fingerPos);
        if (touchedObj != null)
        {
            originalPositionOfPickedObject = touchedObj.transform.position;
            pickedObject = touchedObj;
            currentPuzzlePiece = pickedObject.GetComponent<PuzzlePiece>();
            currTopZ--;
        }
    }

    //for test temorary solution
    float maxPositonY = 1.8f;

    void HandleOnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
    {
        if (!touchEnabled) return;
        if (pickedObject != null)
        {
            Vector3 position = SI_Helper.GetInstance.GetWorldPosition(fingerPos);              
            if((position.y) > maxPositonY)
            {
                pickedObject.transform.position = new Vector3(position.x, maxPositonY, currTopZ);
            }
            else
            {
                pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);
            }
        }
    }

    void HandleOnFingerMove(int fingerIndex, Vector2 fingerPos)
    {
        if (!touchEnabled) return;
        if (pickedObject != null)
        {
            Vector3 position = SI_Helper.GetInstance.GetWorldPosition(fingerPos);
            if ((position.y) > maxPositonY)
            {
                pickedObject.transform.position = new Vector3(position.x, maxPositonY, currTopZ);
            }
            else
            {
                pickedObject.transform.position = new Vector3(position.x, position.y, currTopZ);
            }
        }
    }

    void HandleOnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
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
        foreach(GameObject spriteLocal in allSprites)
        {            
            Vector2 snapPositionVector = new Vector2(snapPosition.x, snapPosition.y);
            Vector2 spritePosition = new Vector2(spriteLocal.transform.position.x, spriteLocal.transform.position.y);

            string xSnapPos = snapPosition.x.ToString("0.0");
            string ySnapPos = snapPosition.y.ToString("0.0");

            string xSpritePos = spriteLocal.transform.position.x.ToString("0.0");
            string ySpritePos = spriteLocal.transform.position.y.ToString("0.0");

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
}
     