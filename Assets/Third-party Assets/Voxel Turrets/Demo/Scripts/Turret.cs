namespace MoenenGames.Turrets {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class Turret : MonoBehaviour {



		[SerializeField]
		[Range(0f, 180f)]
		private float Limit = 180f;



		private float InitLocalRotY = 0f;
		private float AimRotationY = 0f;




		protected virtual void Start () {
			InitLocalRotY = transform.localRotation.eulerAngles.y % 360f;
			AimRotationY = transform.localRotation.eulerAngles.y;
		}



		protected virtual void Update () {

			// Rot
			transform.rotation = Quaternion.Euler(0f, AimRotationY, 0f);

			// Clamp
			float localY = Mathf.Repeat(transform.localRotation.eulerAngles.y + 180f, 360f) - 180f;

			if (Mathf.Abs(Mathf.Abs(localY % 360f) - Mathf.Abs(InitLocalRotY)) > Limit) {
				transform.localRotation = Quaternion.Euler(0f, InitLocalRotY + (localY > 0f ? Limit : -Limit), 0f);
			}
		}





		public void Rotate (float y) {
			AimRotationY = y;
		}


	}
}
