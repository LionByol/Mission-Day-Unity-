using UnityEditor;
using UnityEngine;

public class DeletePrefs : MonoBehaviour {

    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem ("Soft/Delete Player Prefs")]
    static void DeletePlayerPrefs () {
		PlayerPrefs.DeleteAll();
    }
}