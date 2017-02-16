namespace MoenenGames.Turrets {

	using UnityEngine;
	using System.Collections;

	public class PlayerTurret : Turret {



		[SerializeField]
		private float RotateSpeed = 720f;



		protected override void Update () {

			Vector3 mousePos = Vector3.zero;

			Plane plane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float distance;
			if (plane.Raycast(ray, out distance)) {
				mousePos = ray.origin + ray.direction * distance;
			}

			Quaternion aimRot = Quaternion.RotateTowards(
				transform.rotation,
				Quaternion.LookRotation(
					mousePos - transform.position,
					Vector3.up
				),
				Time.deltaTime * RotateSpeed
			);

			Rotate(aimRot.eulerAngles.y);

			base.Update();
		}




	}
}