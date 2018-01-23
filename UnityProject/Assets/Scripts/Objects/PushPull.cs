using System.Collections;
using PlayGroup;
using Tilemaps;
using Tilemaps.Behaviours.Objects;
using Tilemaps.Scripts;
using UnityEngine;
using UnityEngine.Networking;

public class PushPull : VisibleBehaviour
{
	public bool isPushing = false;

	public override void Interact(GameObject originator, Vector3 position, string hand)
	{
		base.Interact(originator, position, hand);
	}

	//Only happens on LocalPlayer
	public void TryPush(Vector3Int newPos)
	{
		if (isPushing) {
			return;
		}
	}
}