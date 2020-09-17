using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Image;

public class MainMenuController : MonoBehaviour
{
	public GameObject Library,InProgess,Categories,MyCollection,GamePlaySetting,SettingsPanel,DailyPuzzle,GamePlay,DeletePanel,CollectionShop;

	public Image GamePlayImage;
	public GameObject IndividualPuzzleScreen;
	public GameObject[] CatPuzzleScreen;
	public string[] Name;
	public GameObject CategoryName, SelectedRotation,SelectedDifficulty;
	public Transform Pos1,Pos2;

	public GameObject libraryContentScroll;
	public JsonFile myJsonFile;

	public float Geihftl;

	public static int PiecesNo;

	private void Start()
	{
		PlayerPrefs.DeleteAll ();
		PiecesNo = 16;
		CheckRotation ();
//		myJsonFile.LoadJson();
//
//		for (int i = 0; i < 12; i++)
//		{
//			int imageCounter = 0;
//			foreach (Hashtable hashLibraryImage in myJsonFile.allLibraryImages)
//			{
//				string imagePath = (string)hashLibraryImage["imagePath"];
//				string imageName = (string)hashLibraryImage["imageName"];
//				float value = 0.34f;
//				if (Screen.height < 2960)
//				{
//					float percentDecrease = (((2960f - Screen.height) / 2960f) * 100f);
//					float percentIncrease = (17.6f / 35.1f) * percentDecrease;
//					value = value + (value * percentIncrease / 100f);
//				}
//				PuzzleImageButton.LoadImage(imagePath, imageName, this, libraryContentScroll.transform);
//				//for scroll content
//				if (imageCounter % 2 == 0)
//				{
//					RectTransform rectTransform = libraryContentScroll.GetComponent<RectTransform>();
//					rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, rectTransform.anchorMin.y - value);
//				}
//				imageCounter++;
//			}
//		}
	}

	void Update(){
		print (PlayerPrefs.GetInt ("IsRotating", 0));
			
	}

    public void CollectionShopFunction(){
		CollectionShop.SetActive (true);
		MyCollection.SetActive (false);
	}

	public void DificultySelection(int i){
		if (i == 1) {
			SelectedDifficulty.transform.localPosition = new Vector3(-300.0f,-29.0f,0.0f);
			PiecesNo = 16;
		}
		else if (i == 2) {
			SelectedDifficulty.transform.localPosition = new Vector3(-150.0f,-29.0f,0.0f);
			PiecesNo = 25;
		}
		else if (i == 3) {
			SelectedDifficulty.transform.localPosition = new Vector3(0.0f,-29.0f,0.0f);
			PiecesNo = 36;
		}
		else if (i == 4) {
			SelectedDifficulty.transform.localPosition = new Vector3(150.0f,-29.0f,0.0f);
			PiecesNo = 64;
		}
		else if (i == 5) {
			SelectedDifficulty.transform.localPosition = new Vector3(300.0f,-29.0f,0.0f);
			PiecesNo = 100;
		}
	}

	void CheckRotation(){
		if (PlayerPrefs.GetInt("IsRotating",0) == 0) {
			SelectedRotation.transform.position = Pos2.transform.position;
		} else {
			SelectedRotation.transform.position = Pos1.transform.position;
		}
	}

	public void Rotation(int j){
		if (j == 1) {
			SelectedRotation.transform.position = Pos1.transform.position;
			PlayerPrefs.SetInt ("IsRotating", 1);
		}
		else if (j == 2) {
			SelectedRotation.transform.position = Pos2.transform.position;
			PlayerPrefs.SetInt ("IsRotating", 0);
		}
	}

	public void BackToCategory(){
		for (int i = 0; i < CatPuzzleScreen.Length; i++) {
			CatPuzzleScreen [i].SetActive (false);
		}
		IndividualPuzzleScreen.SetActive(false);
		Categories.SetActive(true);
	}
	
	public void CategoryClick(int i){
		CategoryName.GetComponent<Text>().text = Name[i];
		CatPuzzleScreen [i].SetActive (true);
		IndividualPuzzleScreen.SetActive(true);Categories.SetActive(false);
	}

	public void GamePlayButton(int i){
		if(i == 0)
			GamePlayImage.sprite = Resources.Load<Sprite>("Images/Deer");
		else if(i == 1)
			GamePlayImage.sprite = Resources.Load<Sprite>("Images/Eagle");
		else if(i == 2)
			GamePlayImage.sprite = Resources.Load<Sprite>("Images/House");
		else if(i == 3)
			GamePlayImage.sprite = Resources.Load<Sprite>("Images/Owl");
		else if(i == 4)
			GamePlayImage.sprite = Resources.Load<Sprite>("Images/Rabit");

		GamePlayFunction ();
	}

	public void DeletePanelFunction(){
		DeletePanel.SetActive (true);
	}

	public void CancelDeletePanelFunction(){
		DeletePanel.SetActive (false);
	}

	public void LibraryFunction(){
		Library.SetActive (true);
		InProgess.SetActive (false);
		Categories.SetActive (false);
		MyCollection.SetActive (false);
		GamePlaySetting.SetActive (false);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (false);
		GamePlay.SetActive (false);
	}

	public void DailyFunction(){
		Library.SetActive (false);
		InProgess.SetActive (false);
		Categories.SetActive (false);
		MyCollection.SetActive (false);
		GamePlaySetting.SetActive (false);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (true);
	}

	public void InProgessFunction(){
		Library.SetActive (false);
		InProgess.SetActive (true);
		Categories.SetActive (false);
		MyCollection.SetActive (false);
		GamePlaySetting.SetActive (false);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (false);
	}

	public void CategoryFunction(){
		Library.SetActive (false);
		InProgess.SetActive (false);
		Categories.SetActive (true);
		MyCollection.SetActive (false);
		GamePlaySetting.SetActive (false);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (false);
	}

	public void MyCollectionFunction(){
		Library.SetActive (false);
		InProgess.SetActive (false);
		Categories.SetActive (false);
		MyCollection.SetActive (true);
		GamePlaySetting.SetActive (false);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (false);
	}

	public void GamePlayFunction(){
		Library.SetActive (false);
		InProgess.SetActive (false);
		Categories.SetActive (false);
		MyCollection.SetActive (false);
		GamePlaySetting.SetActive (true);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (false);
	}

	public void StartGame(){
		GamePlay.SetActive (true);
		Library.SetActive (false);
		InProgess.SetActive (false);
		Categories.SetActive (false);
		MyCollection.SetActive (false);
		GamePlaySetting.SetActive (false);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (false);
	}

	public void Settings(){
		SettingsPanel.SetActive (true);
	}

	public void BackFunction(){
		if (SettingsPanel.activeSelf) {
			SettingsPanel.SetActive (false);
		}
		else if (GamePlaySetting.activeSelf) {
			LibraryFunction ();
		}
		else if (GamePlay.activeSelf) {
			LibraryFunction ();
		}
		else if(CollectionShop.activeSelf){
			CollectionShop.SetActive (false);
			MyCollection.SetActive (true);
		}
	}

}