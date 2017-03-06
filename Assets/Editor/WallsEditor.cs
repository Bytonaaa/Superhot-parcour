using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NeonEffect))]
[CanEditMultipleObjects]
public class WallsEditor : Editor
{

	
	// Update is called once per frame
	public override void OnInspectorGUI ()
	{
        serializedObject.Update();
        DrawDefaultInspector();

	    if (GUILayout.Button("Apply"))
	    {
	        foreach (var VARIABLE in targets)
	        {
	            var neonEffect = VARIABLE as NeonEffect;
	            if (neonEffect != null)
	            {
	                neonEffect.Reset(true);
	            }
	        }
	    }
	    serializedObject.ApplyModifiedProperties();
	}
}
