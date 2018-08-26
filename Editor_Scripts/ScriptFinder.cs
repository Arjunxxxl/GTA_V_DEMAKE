using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptFinder : EditorWindow {

		string mstr = "";

		[MenuItem("Window/FindGameObjects")]	
		public static void ShowWindow()
		{
			EditorWindow.GetWindow<ScriptFinder>("Find game objects");	//name the appear in the windows panel
		}

		private void OnGUI()
		{
			GUILayout.Label("Find gameobject", EditorStyles.boldLabel);
			mstr = EditorGUILayout.TextField("Name of GameObject", mstr);

			if(GUILayout.Button("Find Now"))
			{
				//Object []go = GameObject.FindObjectsOfType(typeof(MonoBehaviour));	//find the object in the scene
				Object []go =  Resources.FindObjectsOfTypeAll(typeof(Mesh));			//find the object in the whole project
				foreach(Object o in go)
				{
					Debug.Log(o.name);
				}
			}

		}

}
