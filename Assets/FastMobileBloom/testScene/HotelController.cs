using UnityEngine;

public class HotelController : MonoBehaviour
{
	public UnityStandardAssets.ImageEffects.FastMobileBloom myBloomScript;
	public void SwitchBloom()
	{
		myBloomScript.enabled = !myBloomScript.enabled;
	}
}