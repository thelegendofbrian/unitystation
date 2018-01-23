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
			custNetTransform.PushTo(posToPushTo, playerSprites.currentDirection, true,
									playerMove.speed, true);
		}
	}
}