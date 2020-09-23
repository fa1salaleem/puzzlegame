using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SI_Helper : MonoBehaviour {
    // current screen width height in world units
    public float screenWidthInWorldPoints;
    public float screenHeightInWorldPoints;

    //for singleton
    private static SI_Helper instance = null;
    private SI_Helper() { }

    public static SI_Helper GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new SI_Helper();
                instance.CalculateScreenWidthHeight();
            }
            return instance;
        }
    }

    public static GameObject LoadPrefab(string path)
    {
        return Instantiate(Resources.Load(path)) as GameObject;
    }

    public GameObject PickObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        else
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f)), Vector2.zero);
            if (hit2D.collider != null)
                return hit2D.collider.gameObject;
        }

        return null;
    }

    public GameObject PickObject(Vector2 screenPos, Camera _camera)
    {
        Ray ray = _camera.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        else
        {
            RaycastHit2D hit2D = Physics2D.Raycast(_camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f)), Vector2.zero);
            if (hit2D.collider != null)
                return hit2D.collider.gameObject;
        }

        return null;
    }

    public Vector3 GetWorldPosition(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float t = -ray.origin.z / ray.direction.z;
        return ray.GetPoint(t);
    }

    public Vector3 GetWorldPositionForCamera(Vector2 screenPos, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(screenPos);
        float t = -ray.origin.z / ray.direction.z;
        return ray.GetPoint(t);
    }

    public void CalculateScreenWidthHeight()
    {
        Vector3 extremeLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screenWidthInWorldPoints = Mathf.Abs(extremeLeft.x) * 2;
        screenHeightInWorldPoints = Mathf.Abs(extremeLeft.y) * 2;
    } 

    public float CalculateAspectRatio()
    {
        float ratio = 0.0f;
        if(Screen.width > Screen.height)
        {
            ratio = (float)Screen.width / Screen.height;
        }
        else
        {
            ratio = (float)Screen.height / Screen.width;
        }
        return ratio;
    }
}
