namespace MoenenGames.Turrets {

	using UnityEngine;
	using UnityEditor;
	using System.Collections;


	[RequireComponent(typeof(Rigidbody))]
	public class Bullet : MonoBehaviour {



		#region --- VAR ---


		// Shot Cut



		public Rigidbody Rig {
			get {
				if (!rig) {
					rig = GetComponent<Rigidbody>();
				}
				return rig;
			}
		}

		public Collider Col {
			get {
				if (!col) {
					col = GetComponent<Collider>();
				}
				return col;
			}
		}


		// Serialize
		[Header("Setting")]
		[SerializeField]
		private float LifeTime = 1f;
		[SerializeField]
		private bool StopOnHit = false;
		[SerializeField]
		private bool EntityOnHit = false;

		[Header("Component")]
		[SerializeField]
		private ParticleSystem Particle;
		[SerializeField]
		private Transform Model;


		// Data
		[HideInInspector]
		public bool Alive = false;
		private Rigidbody rig = null;
		private Collider col = null;

		// Setting
		private const float BULLET_MAX_REBOUND_SPEED = 20f;


		#endregion




		#region --- MSG ---



		void Start () {
			// Size
			SetSize(transform.localScale.x);
		}



		void OnEnable () {
			// Self Kill
			Invoke("DisableCollider", LifeTime);
			Invoke("DestoryBullet", LifeTime + 1f);
		}




		void OnCollisionEnter (Collision col) {
			Colliding(col.transform);
		}




		void OnTriggerEnter (Collider c) {
			if (EntityOnHit) {
				Col.isTrigger = false;
			}
			Colliding(c.transform);
		}




		void Colliding (Transform tf) {

			if (!Alive) {
				return;
			}



			// Logic
			Alive = false;

			// Stop
			if (StopOnHit) {
				Rig.velocity = Vector3.zero;
			} else {
				Rig.velocity = Vector3.ClampMagnitude(Rig.velocity, BULLET_MAX_REBOUND_SPEED);
			}

			// Particle
			if (Particle) {
				Particle.Play();
			}

			// System
			CancelInvoke();
			DestoryBullet();



		}



		private void DisableCollider () {
			Alive = false;
			if (Col) {
				Col.enabled = false;
			}
		}


		public void DestoryBullet () {
			Alive = false;
			TrailRenderer t = GetComponent<TrailRenderer>();
			if (t) {
				t.enabled = false;
			}
			if (Model) {
				Model.gameObject.SetActive(false);
			}

			Destroy(gameObject, Particle ? Particle.main.duration + Particle.main.startLifetimeMultiplier : 0.1f);
		}


		#endregion




		#region --- API ---



		public void SetSize (float size) {

			// Trail
			TrailRenderer t = transform.GetComponent<TrailRenderer>();
			if (t) {
				t.widthMultiplier = size;
			}

			// Rig
			Rig.mass *= size;

		}



		#endregion




	}
}