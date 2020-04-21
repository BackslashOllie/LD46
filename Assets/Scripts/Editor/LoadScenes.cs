using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LoadScenes
{
	[MenuItem("Scenes/MainMenu")]
	public static void LoadMainMenuScene()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
	}

	[MenuItem("Scenes/HowToPlay")]
	public static void LoadHowToScene()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/HowToPlay.unity");
	}

	[MenuItem("Scenes/Level_0")]
	public static void LoadLevel0Scene()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level_0.unity");
	}

	[MenuItem("Scenes/Exit")]
	public static void LoadExitScene()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Exit.unity");
	}
	
	[MenuItem("Scenes/ExitLose")]
	public static void LoadExitLoseScene()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/ExitLose.unity");
	}

	[MenuItem("Custom/ParentLanterns")]
	public static void Lanterns()
	{
		Lantern[] lanterns = GameObject.FindObjectsOfType<Lantern>();
		Transform parent = GameObject.Find("Lanterns").transform;
		foreach (Lantern lantern in lanterns) lantern.transform.SetParent(parent);
	}
}