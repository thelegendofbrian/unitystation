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
	private bool isSyncedWithNewPlayer = false;
	public GameObject pulledBy;
	public PlayerSync pullSync;

	public void OnEnable(){
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

	public void SyncWithNewPlayers(GameObject puller){
		if (!isSyncedWithNewPlayer) {
			isSyncedWithNewPlayer = true;
			isBeingPulled = true;
			pulledBy = puller;
			pulledBy.transform.hasChanged = false;
			PlayerSync pullerSync = pulledBy.GetComponent<PlayerSync>();
			pullSync = pullerSync;
			pullSync.pullingObject = customNetTransform;
		}
	}

	public override void Interact(GameObject originator, Vector3 position, string hand)
	{
		if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)){
			if(isPlayer){
				Debug.Log("No support for pulling a player yet. Coming soon");
				//TODO because players do not have CNT, need to come up with a solution to pull players
				return;
			}

			PlayerManager.LocalPlayerScript.playerNetworkActions.CmdPullState(gameObject);

			//TODO Prediction for starting to pull.
		}
		base.Interact(originator, position, hand);
	}

	[ClientRpc]
	public void RpcPullState(GameObject _pulledBy){

		//Clear pulling on this client
		if(_pulledBy == null){
			SetPull(false);
			return;
		}

		if (isBeingPulled) {
			if (pulledBy != PlayerManager.LocalPlayer) {
				SetPull(false);
				SetPull(true, _pulledBy);
			} else {
				//Stop pulling
				SetPull(false);
			}
		} else {
			//try to pull the object
			SetPull(true, _pulledBy);
		}
	}

	//Seperate from the Rpc so we can apply prediction if it is wanted
	private void SetPull(bool isPulling, GameObject _pulledBy = null){
		if(isPulling){
			isBeingPulled = true;
			pulledBy = _pulledBy;
			pulledBy.transform.hasChanged = false;
			PlayerSync pullerSync = pulledBy.GetComponent<PlayerSync>();
			pullSync = pullerSync;
			pullSync.pullingObject = customNetTransform;
		} else {
			isBeingPulled = false;
			pulledBy = null;
			if (pullSync != null) {
				pullSync.pullingObject = null;
				pullSync = null;
			}
		}
	}
}