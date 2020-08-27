using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Image;

public class MainMenuController : MonoBehaviour
{
	public GameObject Library,InProgess,Categories,MyCollection,GamePlaySetting,SettingsPanel,DailyPuzzle;

	public Image GamePlayImage; 
	public Image[] Sources;
//	public Image ImgSprite = null;

	[SerializeField] private UnityEngine.UI.Image image = null;

    // Start is called before the first frame update
    void Start()
    {
//		image.sprite = Resources.Load<Sprite>("Images/test");
//		ImgSprite.sprite = Resources.Load<Sprite>("Library/image.png");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void LibraryFunction(){
		Library.SetActive (true);
		InProgess.SetActive (false);
		Categories.SetActive (false);
		MyCollection.SetActive (false);
		GamePlaySetting.SetActive (false);
		SettingsPanel.SetActive (false);
		DailyPuzzle.SetActive (false);
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

	public void Settings(){
		SettingsPanel.SetActive (true);
	}

	public void BackFunction(){
		if (SettingsPanel.activeSelf) {
			SettingsPanel.SetActive (false);
		}
	}

}
