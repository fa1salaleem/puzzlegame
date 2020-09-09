using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonFile : MonoBehaviour
{
	Hashtable baseData;

    private void Start()
    {
		LoadJson();
    }

    public void LoadJson()
	{
		string nameOfJson = "testmateen.json";
		string path = Application.streamingAssetsPath + "/" + nameOfJson;
		string jsonData = "";
		using (StreamReader r = new StreamReader(path))
		{
			jsonData = r.ReadToEnd();
		}
		baseData = (Hashtable)easy.JSON.JsonDecode(jsonData);
		Hashtable allImagesData = (Hashtable)baseData["allImagesData"];
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
