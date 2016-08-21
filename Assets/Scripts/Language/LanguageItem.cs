using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class LanguageItem : MonoBehaviour {
	public UILabel name1;
	public UILabel englishName;
	public LanguageWin parent;

	string lang;

	public void InitialData(string lang)
	{	
		string str0 = LanguageWin.CodeToName(LocalizationSettings.GetLanguageEnum(lang));
		name1.text=str0;
		englishName.text = Language.CodeToEnglishName(LocalizationSettings.GetLanguageEnum(lang));
		this.lang=lang;

		if (LocalizationSettings.GetLanguageEnum(lang)==Language.CurrentLanguage()){
			print (transform.position);
 			parent.glowSprite.transform.position=new Vector3(transform.position.x,transform.position.y-0.115f,0);
		}
	}

	public void SwitchLanguage()
	{
		Language.SwitchLanguage(lang);
		parent.glowSprite.transform.position=new Vector3(transform.position.x,transform.position.y-0.115f,0);
	}
}
