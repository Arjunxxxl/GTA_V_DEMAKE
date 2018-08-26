
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraEdditor : Editor {

	CameraController cameraRig;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		cameraRig = (CameraController)target;
		EditorGUILayout.LabelField("Camera Helper");

		if(GUILayout.Button("Save camera pos now"))
		{
			Camera cam = Camera.main;
			if(cam)
			{
				Transform camT = cam.transform;
				Vector3 campos = camT.localPosition;
				Vector3 camRight = campos;
				Vector3 camLeft = campos;
				camLeft.x = -campos.x;
				cameraRig.camerasettings.camoffSetRight = camRight;
				cameraRig.camerasettings.camoffSetLeft = camLeft;
			}
		}
	}
}
