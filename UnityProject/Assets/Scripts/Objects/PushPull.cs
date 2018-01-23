using System.Collections;
using PlayGroup;
using Items;
using Tilemaps;
using Tilemaps.Behaviours.Objects;
using Tilemaps.Scripts;
using UnityEngine;
using UnityEngine.Networking;

public class PushPull : VisibleBehaviour
{
	private Matrix matrix => registerTile.Matrix;
	private CustomNetTransform customNetTransform;

	//Check via PlayerMove. Not checked internally so that TryPush
	//can be used for explosions or atmos events
	public bool isPlayerPushable = true;

	private void OnEnable(){
		customNetTransform = GetComponent<CustomNetTransform>();
		DetermineIsPushable();
	}

	void DetermineIsPushable(){
		//Determine below if isPushable should be enabled:
		PickUpTrigger pickUpTrigger = GetComponent<PickUpTrigger>();
		if (pickUpTrigger != null) {
			isPlayerPushable = false;
		}

		ItemAttributes itemAtrributes = GetComponent<ItemAttributes>();
		if (itemAtrributes != null) {
			isPlayerPushable = false;
		}
	}

	public override void Interact(GameObject originator, Vector3 position, string hand)
	{
		base.Interact(originator, position, hand);
	}
}