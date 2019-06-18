﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Behavior common to all wall mounts. Note that a wallmount's facing is determined by the Directional.InitialDirection,
/// but the wallmount sprites are always re-oriented to be upright when in game.
///
/// Adds a WallmountSpriteBehavior to all child objects that have SpriteRenderers. Facing / visibility checking is handled in
/// there. See <see cref="WallmountSpriteBehavior"/>
/// </summary>
[RequireComponent(typeof(Directional))]
public class WallmountBehavior : MonoBehaviour
{
	//cached spriteRenderers of this gameobject
	private SpriteRenderer[] spriteRenderers;
	private Directional directional;

	private void Start()
	{
		directional = GetComponent<Directional>();
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer renderer in spriteRenderers)
		{
			renderer.gameObject.AddComponent<WallmountSpriteBehavior>();
		}
	}

	/// <summary>
	/// Checks if the wallmount is facing the specified position
	/// </summary>
	/// <param name="worldPosition">position to check</param>
	/// <returns>true iff it is facing the position</returns>
	public bool IsFacingPosition(Vector3 worldPosition)
	{
		Vector3 headingToPosition = worldPosition - transform.position;
		Vector3 facing = -directional.CurrentDirection.Vector;
		float difference = Vector3.Angle(facing, headingToPosition);
		//91 rather than 90 helps prevent flickering due to rounding
		return difference >= 91 || difference <= -91;
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.green;
		if (directional == null)
		{
			directional = GetComponent<Directional>();
		}

		if (Application.isEditor && !Application.isPlaying)
		{
			DebugGizmoUtils.DrawArrow(transform.position, directional.InitialOrientation.Vector);
		}
		else
		{
			DebugGizmoUtils.DrawArrow(transform.position, directional.CurrentDirection.Vector);
		}


	}

	/// <summary>
	/// Checks if the wallmount has been hidden based on facing calculation already performed. Use this
	/// to avoid having to re-calculate facing.
	/// </summary>
	/// <returns>true iff this wallmount has been already hidden due to not facing the local player</returns>
	public bool IsHiddenFromLocalPlayer()
	{
		foreach (SpriteRenderer renderer in spriteRenderers)
		{
			if (renderer.color.a > 0)
			{
				//there's at least one non-transparent renderer, so it's not hidden
				return false;
			}
		}

		//there were no renderers or all of them were transparent, it's hidden
		return true;
	}
//  NOTE: This was the code used to set wallmount rotations with ExecuteInEditor, this can be removed once this is merged if there
//  are no mapping conflicts
//	#if UNITY_EDITOR
//	private void Update()
//	{
//		//fix the wallmounts
//		var directional = GetComponent<Directional>();
//		if (directional == null)
//		{
//			directional = gameObject.AddComponent<Directional>();
//			Vector3 facing = -transform.up;
//			var initialOrientation = Orientation.From(facing);
//			directional.InitialDirection = initialOrientation.AsEnum();
//		}
//		//directional.InitialDirection = directional.InitialOrientation.Rotate(RotationOffset.Backwards).AsEnum();
//
//	}
//	#endif
}
