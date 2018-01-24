using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
///     Tells client to change world object's transform state ((dis)appear/change pos/start floating)
/// </summary>
public class TransformStateMessage : ServerMessage
{
	public static short MessageType = (short) MessageTypes.TransformStateMessage;
	public bool ForceRefresh;
	public bool IsPushing;
	public TransformState State;
	public NetworkInstanceId TransformedObject;

	//For syncing pull
	public bool IsBeingPulled;
	public GameObject Puller;

	///To be run on client
	public override IEnumerator Process()
	{
//		Debug.Log("Processed " + ToString());
		if (TransformedObject == NetworkInstanceId.Invalid)
		{
			//Doesn't make any sense
			yield return null;
		}
		else
		{
			yield return WaitFor(TransformedObject);
			if (CustomNetworkManager.Instance._isServer || ForceRefresh)
			{
				//update NetworkObject transform state
				var transform = NetworkObject.GetComponent<CustomNetTransform>();
				transform.UpdateClientState(State, IsPushing);

				//Used for init sync of new players
				if(IsBeingPulled){
					transform.pushPull.SyncWithNewPlayers(Puller);
				}
			}
		}
	}

	public static TransformStateMessage Send(GameObject recipient, GameObject transformedObject, TransformState state, bool isBeingPulled, GameObject puller, bool forced = true)
	{
		var msg = new TransformStateMessage
		{
			TransformedObject = transformedObject != null ? transformedObject.GetComponent<NetworkIdentity>().netId : NetworkInstanceId.Invalid,
			State = state,
			IsBeingPulled = isBeingPulled,
			Puller = puller,
			ForceRefresh = forced
		};
		msg.SendTo(recipient);
		return msg;
	}

	/// <param name="transformedObject">object to hide</param>
	/// <param name="state"></param>
	/// <param name="forced">
	///     Used for client simulation, use false if already updated by prediction
	///     (to avoid updating it twice)
	/// </param>
	public static TransformStateMessage SendToAll(GameObject transformedObject, TransformState state, bool _isPushing = false, bool forced = true)
	{
		var msg = new TransformStateMessage {
			TransformedObject = transformedObject != null ? transformedObject.GetComponent<NetworkIdentity>().netId : NetworkInstanceId.Invalid,
			State = state,
			IsPushing = _isPushing,
			ForceRefresh = forced
		};
		msg.SendToAll();
		return msg;
	}

	public override string ToString()
	{
		return
			$"[TransformStateMessage Parameter={TransformedObject} Active={State.Active} WorldPos={State.position} localPos={State.localPos} " +
			$"Spd={State.Speed} Imp={State.Impulse} Type={MessageType} Forced={ForceRefresh}]";
	}
}