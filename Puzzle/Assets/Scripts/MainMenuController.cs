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
	public GameObject CategoryName, SelectedRotation;
	public Transform Pos1,Pos2;

    // Start is called before the first frame update
//    void Start()
//    {

//    }

    // Update is called once per frame
//    void Update()
//    {

//    }

	public void CollectionShopFunction(){
		CollectionShop.SetActive (true);
		MyCollection.SetActive (false);
	}

	public void Rotation(int j){
		if (j == 1) {
			SelectedRotation.transform.position = Pos1.transform.position;
		}
		else if (j == 2) {
			SelectedRotation.transform.position = Pos2.transform.position;
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