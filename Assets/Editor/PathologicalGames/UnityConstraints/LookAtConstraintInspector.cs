/// <Licensing>
/// © 2011 (Copyright) Path-o-logical Games, LLC
/// If purchased from the Unity Asset Store, the following license is superseded 
/// by the Asset Store license.
/// Licensed under the Unity Asset Package Product License (the "License");
/// You may not use this file except in compliance with the License.
/// You may obtain a copy of the License at: http://licensing.path-o-logical.com
/// </Licensing>
using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(LookAtConstraint)), CanEditMultipleObjects]
public class LookAtConstraintInspector : LookAtBaseClassInspector
{
	protected SerializedProperty upTarget;
	protected SerializedProperty upVectPointAt;

    protected override void OnEnable()
	{
		base.OnEnable();

		this.upTarget = this.serializedObject.FindProperty("upTarget");
		this.upVectPointAt = this.serializedObject.FindProperty("upVectPointAt");
    }
	
    protected override void OnInspectorGUIUpdate()
    {
        base.OnInspectorGUIUpdate();
		
		GUIContent content;
		
		content = new GUIContent
		(
			"Up Target (Optional)", 
			"An optional target just for the upAxis. The upAxis may not point directly at this. " +
				"See the online docs for more info"
		);
		EditorGUILayout.PropertyField(this.upTarget, content);	
		
		if (this.upTarget.objectReferenceValue == null)
		{
			content = new GUIContent
			(
				"Up Axis Point Direction", 
				"The world direction the upAxis will point at. For 3D games it is commong to have " +
					"the object's Y axis point at world Y. For 2D games it is common to have the " +
					"objects Z axis point at the world Z axis"
			);
			EditorGUILayout.PropertyField(this.upVectPointAt, content);	
		}
		
	}
}
