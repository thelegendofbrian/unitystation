using PlayGroup;
using UnityEngine;
using UnityEngine.Networking;

public partial class PlayerNetworkActions : NetworkBehaviour
{
	[Command]
	public void CmdTryPush(GameObject objToPush, Vector3 posToPushTo)
	{
		CustomNetTransform custNetTransform = objToPush.GetComponent<CustomNetTransform>();
		if (custNetTransform != null) {
			if(custNetTransform.pushPull.isBeingPulled){
				custNetTransform.pushPull.RpcPullState(null);
			}
			custNetTransform.PushTo(posToPushTo, playerSprites.currentDirection, true,
			                        playerMove.speed, true);
		}
	}

	[Command]
	public void CmdPullState(GameObject objToPull){
		PushPull pushPull = objToPull.GetComponent<PushPull>();
		if(pushPull != null){
			pushPull.RpcPullState(gameObject);
		}
	}

	[Command]
	public void CmdManualPullReset(GameObject objToPull)
	{
		PushPull pushPull = objToPull.GetComponent<PushPull>();
		if (pushPull != null) {
			pushPull.RpcPullState(null);
		}
	}
}