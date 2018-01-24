using System.Collections;
using UnityEngine;
using PlayGroup;

public class FuelTankHealthBehaviour : HealthBehaviour
{
	private PushPull pushPull;

	private void Awake()
	{
		pushPull = GetComponent<PushPull>();
	}

	protected override void OnDeathActions()
	{
		if (pushPull != null) {
			if (pushPull.pulledBy == PlayerManager.LocalPlayer) {
				PlayerManager.LocalPlayerScript.playerNetworkActions.CmdManualPullReset(gameObject);
			}
		}

		float delay = 0f;
		switch (LastDamageType)
		{
			case DamageType.BRUTE:
				delay = 0.1f;
				break;
			case DamageType.BURN:
				delay = Random.Range(0.2f, 2f);
				break; //surprise
		}

		string killer = "God";
		if (LastDamagedBy != null)
		{
			killer = LastDamagedBy.name;
		}
		StartCoroutine(explodeWithDelay(delay, killer));

		//            Debug.Log("FuelTank ded!");
	}

	private IEnumerator explodeWithDelay(float delay, string damagedBy)
	{
		yield return new WaitForSeconds(delay);
		GetComponentInParent<ExplodeWhenShot>().ExplodeOnDamage(damagedBy);
		yield return null;
	}
}