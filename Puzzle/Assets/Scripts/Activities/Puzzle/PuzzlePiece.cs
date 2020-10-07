using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzlePiece : MonoBehaviour
{
    public TextureDivider textureDivider;
    public GameObject pickedObjectHandle;
    public List<GameObject> allCollidedObjects = new List<GameObject>();
    public GameObject nearestColliderObject = null;
    public float distance = 100000.0f;
    public int myPositionIndex;
    public PuzzlePosition myPositionObject;
    public bool placed;

    public void MoveForHint()
    {
        gameObject.transform.DOMove(myPositionObject.position, 0.35f, false);
        myPositionObject.isOccupied = true;
        placed = true;
    }

    void OnTriggerEnter2D(Collider2D other)
	{
        if (textureDivider.pickedObject != null && textureDivider.pickedObject.name == gameObject.name)
        {
            nearestColliderObject = null;
            distance = 100000.0f;
            pickedObjectHandle = textureDivider.pickedObject;
            allCollidedObjects.Add(other.gameObject);
            Debug.Log("SELF = " + gameObject.name);
            Debug.Log("OTHER = " + other.gameObject.name);
        }
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if(allCollidedObjects.Contains(other.gameObject))
            allCollidedObjects.Remove(other.gameObject);
    }

    public bool CheckClose(float distance, float tooCloseParameter)
    {
        bool tooClose = false;
        float tooCloseDivider = 2f;
        float closeDistance = tooCloseParameter / tooCloseDivider;
        Debug.Log("Distance = " + distance);
        Debug.Log("Close Distance = " + closeDistance);
        if (distance < closeDistance)
        {           
            tooClose = true;
        }
        return tooClose;
    }

    public void TouchLifted()
    {
        nearestColliderObject = null;
        distance = 100000.0f;
        bool tooClose = false;

        if(allCollidedObjects.Count > 0)
        {
            int selfRot = (int)gameObject.transform.localEulerAngles.z;
            float width = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            float height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
            float tooCloseParameter = height;
            if(width > height)
            {
                tooCloseParameter = width;
            }

            if (selfRot % 180 == 0)
            {
                Debug.Log("SELF IS IN RIGHT ROTATION");

                //Finding correct collision objects
                for (int i = 0; i < allCollidedObjects.Count; i++)
                {
                    GameObject colliderObject = allCollidedObjects[i] as GameObject;
                    int collRot = (int)colliderObject.transform.localEulerAngles.z;
                    float x2 = (Mathf.Abs(transform.position.x) - Mathf.Abs(colliderObject.transform.position.x)) * (Mathf.Abs(transform.position.x) - Mathf.Abs(colliderObject.transform.position.x));
                    float y2 = (Mathf.Abs(transform.position.y) - Mathf.Abs(colliderObject.transform.position.y)) * (Mathf.Abs(transform.position.y) - Mathf.Abs(colliderObject.transform.position.y));
                    float distanceTemp = Mathf.Sqrt(x2 + y2);

                    float distance50 = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                        new Vector2(colliderObject.transform.position.x, colliderObject.transform.position.y));
                    if (!tooClose)
                    {
                        tooClose = CheckClose(distance50, tooCloseParameter);
                        Debug.Log("tooClose");
                    }

                    if (collRot % 180 == 0)
                    {
                        Debug.Log("CORRECT ORIENTATION COLLIDER = " + colliderObject.name);
                        Debug.Log("NAME = " + colliderObject.name + " & DISTANCE = " + distanceTemp);
                        if (distanceTemp < distance)
                        {
                            distance = distanceTemp;
                            nearestColliderObject = colliderObject;
                        }
                    }
                    else
                    {
                        //allCollidedObjects.RemoveAt(i);
                        //i--;
                    }
                }

                if (nearestColliderObject != null)
                {
                    Vector3 snapPosition = Vector3.zero;
                    SpriteRenderer colliderRenderer = nearestColliderObject.GetComponent<SpriteRenderer>();
                    SpriteRenderer gameObjectRender = gameObject.GetComponent<SpriteRenderer>();

                    if (gameObject.transform.position.x > nearestColliderObject.transform.position.x)
                    {
                        //Right Side

                        if (Mathf.Abs(gameObject.transform.position.y - nearestColliderObject.transform.position.y) < (colliderRenderer.bounds.size.y / 4))
                        {
                            //Right
                            float xPos = nearestColliderObject.transform.position.x + (colliderRenderer.bounds.size.x / 2) + (gameObjectRender.bounds.size.x / 2);
                            snapPosition = new Vector3(xPos, nearestColliderObject.transform.position.y, gameObject.transform.position.z);
                        }
                        else if (gameObject.transform.position.y > nearestColliderObject.transform.position.y)
                        {
                            //up
                            float yPos = nearestColliderObject.transform.position.y + (colliderRenderer.bounds.size.y / 2) + (gameObjectRender.bounds.size.y / 2);
                            snapPosition = new Vector3(nearestColliderObject.transform.position.x, yPos, gameObject.transform.position.z);
                        }
                        else
                        {
                            //down
                            float yPos = nearestColliderObject.transform.position.y - (colliderRenderer.bounds.size.y / 2) - (gameObjectRender.bounds.size.y / 2);
                            snapPosition = new Vector3(nearestColliderObject.transform.position.x, yPos, gameObject.transform.position.z);
                        }
                    }
                    else
                    {
                        //Left Side

                        if (Mathf.Abs(gameObject.transform.position.y - nearestColliderObject.transform.position.y) < (colliderRenderer.bounds.size.y / 4))
                        {
                            //Left
                            float xPos = nearestColliderObject.transform.position.x - (colliderRenderer.bounds.size.x / 2) - (gameObjectRender.bounds.size.x / 2);
                            snapPosition = new Vector3(xPos, nearestColliderObject.transform.position.y, gameObject.transform.position.z);
                        }
                        else if (gameObject.transform.position.y > nearestColliderObject.transform.position.y)
                        {
                            //up
                            float yPos = nearestColliderObject.transform.position.y + (colliderRenderer.bounds.size.y / 2) + (gameObjectRender.bounds.size.y / 2);
                            snapPosition = new Vector3(nearestColliderObject.transform.position.x, yPos, gameObject.transform.position.z);
                        }
                        else
                        {
                            //down
                            float yPos = nearestColliderObject.transform.position.y - (colliderRenderer.bounds.size.y / 2) - (gameObjectRender.bounds.size.y / 2);
                            snapPosition = new Vector3(nearestColliderObject.transform.position.x, yPos, gameObject.transform.position.z);
                        }
                    }

                    bool snapped = false;
                    if (textureDivider.CheckPosition(snapPosition))
                    {
                        Debug.Log("Already Occupied - Not Snapped");
                        Debug.Log("Too Close = " + tooClose);
                    }
                    else
                    {
                        snapped = true;
                        gameObject.transform.position = snapPosition;
                    }

                    if (!snapped && tooClose)
                    {
                        //gameObject.transform.position = textureDivider.originalPositionOfPickedObject;
                    }
                }
                else
                {
                    Debug.Log("Nearest Object Collider is NULL");
                    if (tooClose)
                    {
                        //gameObject.transform.position = textureDivider.originalPositionOfPickedObject;
                    }
                }
            }
            else
            {
                Debug.Log("NOT IN RIGHT ROTATION");
                //Finding correct collision objects
                for (int i = 0; i < allCollidedObjects.Count; i++)
                {
                    GameObject colliderObject = allCollidedObjects[i] as GameObject;
                    float distance50 = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
    new Vector2(colliderObject.transform.position.x, colliderObject.transform.position.y));
                    tooClose = CheckClose(distance50, tooCloseParameter);
                    if (tooClose)
                    {
                        gameObject.transform.position = textureDivider.originalPositionOfPickedObject;
                        break;
                    }
                }
            }
        }
        else
        {
            pickedObjectHandle = null;
            Debug.Log("NO ACTIVE COLLISION");
        }
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
