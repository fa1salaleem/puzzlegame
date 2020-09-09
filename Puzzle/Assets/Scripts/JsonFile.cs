using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonFile : MonoBehaviour
{
	public Hashtable baseData;
	public ArrayList allLibraryImages;

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

		allLibraryImages = library["images"] as ArrayList;
		ArrayList animalsImages = animals["images"] as ArrayList;

		foreach(Hashtable hashLibraryImage in allLibraryImages)
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
