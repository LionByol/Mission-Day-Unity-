using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Simple Editor Script to togle between Android Platforms
/// </summary>
public class BuildPlatform : MonoBehaviour {
	
   	[MenuItem ("Soft/Set KeyStore")]
	static void SniperTank () {

        PlayerSettings.keyaliasPass = "mission123!@#";
        PlayerSettings.keystorePass = "mission123!@#";

		AssetDatabase.Refresh();

		if( EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android ) {
			EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTarget.Android );
		}
	}

	[MenuItem ("Soft/Build Platform/IOS")]
	static void IOS () {
		//Texture2D[] icon=new Texture2D[]{(Texture2D)Resources.Load("/icon/icon", typeof(Texture2D))};
		//PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iPhone,icon);

		if( EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS ) {
			EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTarget.iOS );
		}
		
//		BuildPipeline.BuildPlayer( BuildScenes(), "Build/PlumberCrackIOS", BuildTarget.iPhone, BuildOptions.AcceptExternalModificationsToPlayer );
	}
	
	
	static string[] BuildScenes() {
		
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in EditorBuildSettings.scenes) {
            if ( S.enabled ) {
                temp.Add(S.path);
            }
        }
        return temp.ToArray();

	}

}
