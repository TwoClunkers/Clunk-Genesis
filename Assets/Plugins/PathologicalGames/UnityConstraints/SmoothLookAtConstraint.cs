/// <Licensing>
/// � 2011 (Copyright) Path-o-logical Games, LLC
/// If purchased from the Unity Asset Store, the following license is superseded 
/// by the Asset Store license.
/// Licensed under the Unity Asset Package Product License (the "License");
/// You may not use this file except in compliance with the License.
/// You may obtain a copy of the License at: http://licensing.path-o-logical.com
/// </Licensing>
using UnityEngine;
using System.Collections;

/// <summary>
///	Billboarding is when an object is made to face a camera, so that no matter
///	where it is on screen, it looks flat (not simply a "look-at" constraint, it
///	keeps the transform looking parallel to the target - usually a camera). This
///	is ideal for sprites, flat planes with textures that always face the camera.
/// </summary>
[AddComponentMenu("Path-o-logical/UnityConstraints/Constraint - Look At - Smooth")]
public class SmoothLookAtConstraint : LookAtConstraint
{
	/// <summary>
	/// The rotation interpolation solution to use.
	/// </summary>
    public UnityConstraints.INTERP_OPTIONS interpolation = 
                                        UnityConstraints.INTERP_OPTIONS.Spherical;
    
	/// <summary>
	/// How fast the constrant can rotate. The result depends on the interpolation chosen.
	/// </summary>
    public float speed = 1;
	
	/// <summary>
	/// What axes and space to output the result to.
	/// </summary>
    public UnityConstraints.OUTPUT_ROT_OPTIONS output = 
                                        UnityConstraints.OUTPUT_ROT_OPTIONS.WorldAll;

    // Reused every frame (probably overkill, but can't hurt...)
    protected Quaternion lookRot;
    protected Quaternion usrLookRot;
    protected Quaternion curRot;
    protected Vector3 angles;
    protected Vector3 lookVectCache;


    /// <summary>
    /// Runs each frame while the constraint is active
    /// </summary>
    protected override void OnConstraintUpdate()
    {
        // Note: Do not run base.OnConstraintUpdate. It is not implimented

        this.lookVectCache = Vector3.zero;
        this.lookVectCache = this.lookVect;  // Property, so cache result
        if (this.lookVectCache == Vector3.zero) return;

        this.lookRot = Quaternion.LookRotation(this.lookVectCache, this.upVect);
        this.usrLookRot = this.GetUserLookRotation(this.lookRot);


        this.OutputTowards(usrLookRot);
    }

    /// <summary>
    /// Runs when the noTarget mode is set to ReturnToDefault
    /// </summary>
    protected override void NoTargetDefault()
    {
        UnityConstraints.InterpolateLocalRotationTo
        (
            this.xform, 
            Quaternion.identity, 
            this.interpolation, 
            this.speed
        );
    }

    /// <summary>
    /// Runs when the constraint is active or when the noTarget mode is set to 
    /// ReturnToDefault
    /// </summary>
    protected void OutputTowards(Quaternion destRot)
    {
        UnityConstraints.InterpolateRotationTo
        (
            this.xform, 
            destRot, 
            this.interpolation, 
            this.speed
        );

        UnityConstraints.MaskOutputRotations(this.xform, this.output);
    }

}
