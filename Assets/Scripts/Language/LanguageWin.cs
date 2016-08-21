using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class LanguageWin : MonoBehaviour {
	public TweenPosition tween_position; 
	public GameObject 	item;
	public UIGrid lang_grid;
	public GameObject glowSprite;

	public void InitialData()
	{
		foreach (string lang in Language.GetLanguages())
		{
			GameObject obj = NGUITools.AddChild(lang_grid.gameObject,item);
			obj.name = lang;
			lang_grid.Reposition();	
			obj.GetComponent<LanguageItem>().InitialData(lang);
		}
		lang_grid.Reposition();	
	}

	public void DismissPanel()
	{
		Destroy(gameObject);
	}

	public static string CodeToName(LanguageCode code)
	{
		string res="";
		if (code == LanguageCode.AF  ) res="Afrikaans"; //
		else if (code == LanguageCode.AR  ) res="اللغة العربية"; //
		else if (code == LanguageCode.BA  ) res="Basque";
		else if (code == LanguageCode.BE  ) res="Belarusian";
		else if (code == LanguageCode.BG  ) res="Bulgarian";
		else if (code == LanguageCode.CA  ) res="Catalan";
		else if (code == LanguageCode.ZH  ) res="中文";//
		else if (code == LanguageCode.CS  ) res="Czech";
		else if (code == LanguageCode.DA  ) res="Dansk"; //
		else if (code == LanguageCode.NL  ) res="Dutch";
		else if (code == LanguageCode.EN  ) res="English";
		else if (code == LanguageCode.ET  ) res="Estonian";
		else if (code == LanguageCode.FA  ) res="Faroese";
		else if (code == LanguageCode.FI  ) res="Finnish";
		else if (code == LanguageCode.FR  ) res="Français"; //
		else if (code == LanguageCode.DE  ) res="Deutsch";  //
		else if (code == LanguageCode.EL  ) res="Ελληνικά"; //
		else if (code == LanguageCode.HE  ) res="Hebrew";
		else if (code == LanguageCode.HU  ) res="Hungarian";
		else if (code == LanguageCode.IS  ) res="Icelandic";
		else if (code == LanguageCode.ID  ) res="Bahasa Indonesia"; //
		else if (code == LanguageCode.IT  ) res="Italian";
		else if (code == LanguageCode.JA  ) res="日本語";
		else if (code == LanguageCode.KO  ) res="한국어";
		else if (code == LanguageCode.LA  ) res="Latvian";
		else if (code == LanguageCode.LT  ) res="Lithuanian";
		else if (code == LanguageCode.NO  ) res="Norwegian";
		else if (code == LanguageCode.PL  ) res="Polish";
		else if (code == LanguageCode.PT  ) res="Portuguese";
		else if (code == LanguageCode.RO  ) res="Romanian";
		else if (code == LanguageCode.RU  ) res="Русский";  //
		else if (code == LanguageCode.SH  ) res="SerboCroatian";
		else if (code == LanguageCode.SK  ) res="Slovak";
		else if (code == LanguageCode.SL  ) res="Slovenian";
		else if (code == LanguageCode.ES  ) res="Español";  //
		else if (code == LanguageCode.SW  ) res="Swedish";
		else if (code == LanguageCode.TH  ) res="Thai";  // impossible 
		else if (code == LanguageCode.TR  ) res="Türkçe";  //
		else if (code == LanguageCode.UK  ) res="Ukrainian";
		else if (code == LanguageCode.VI  ) res="Vietnamese";
		else if (code == LanguageCode.HU  ) res="Hungarian";
		else if (code == LanguageCode.N  ) res="Unknown";
		else res="English";
		return res;
	}
}
