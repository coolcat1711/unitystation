﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Handles spawning of the NPCs
/// </summary>
public class NPCFactory : NetworkBehaviour
{
	public static NPCFactory Instance;

	[SerializeField] private GameObject xenoPrefab;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(Instance);
		}
	}

	/// <summary>
	/// Spawns a xenomorph from the server
	/// </summary>
	/// <param name="parent"> Pass the object parent of the matrix to add the npc too</param>
	/// <returns></returns>
	public static GameObject SpawnXenomorph(Vector2 worldPos, Transform parent)
	{
		var npc = PoolManager.PoolNetworkInstantiate(Instance.xenoPrefab, worldPos, parent, Quaternion.identity);
		return npc;
	}
}
