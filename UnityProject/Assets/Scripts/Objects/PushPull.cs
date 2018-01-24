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

	public bool isPlayerPushable { get; private set; } = true;
	public bool isBeingPulled { get; private set; }
	public GameObject pulledBy;

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
		if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)){
			if(isBeingPulled){
				if(pulledBy != PlayerManager.LocalPlayer){
					//TODO Take it off the other player
				} else {
					//TODO Stop pulling
				}
			} else {
				//TODO try to pull the object
				Debug.Log("TRY TO PULL");
			}
		}
		base.Interact(originator, position, hand);
	}
}