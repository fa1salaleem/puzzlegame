using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonFile : MonoBehaviour
{
	public string jsonData;
    // Start is called before the first frame update
    void Start()
    {
//		Hashtable sampleData = (Hashtable)easy.JSON.JsonDecode(jsonData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void LoadJson()
	{
		string nameOfJson = "testmateen.json";
		string path = Application.streamingAssetsPath + "/" + nameOfJson;
		using (StreamReader r = new StreamReader(path))
		{
			jsonData = r.ReadToEnd();
		}
		Hashtable sampleData = (Hashtable)easy.JSON.JsonDecode(jsonData);
		Hashtable allImagesData = (Hashtable)sampleData["allImagesData"];
		Hashtable library = (Hashtable)allImagesData["library"];
		Hashtable animals = (Hashtable)allImagesData["animals"];

		ArrayList libraryImages = library["images"] as ArrayList;
		ArrayList animalsImages = animals["images"] as ArrayList;

		foreach(Hashtable hashLibraryImage in libraryImages)
		{
			string imagePath = (string)hashLibraryImage["imagePath"];
			Debug.Log(imagePath);
		}

		foreach (Hashtable hashanimalImage in animalsImages)
		{
			string imagePath = (string)hashanimalImage["imagePath"];
			Debug.Log(imagePath);
		}
	}

}
