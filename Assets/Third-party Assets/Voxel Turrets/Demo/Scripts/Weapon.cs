namespace MoenenGames.Turrets {

	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class Weapon : MonoBehaviour {




		#region --- VAR ---


		// Shot Cut


		public Transform Model {
			get {
				return model ? model : transform;
			}
		}

		public float PrevAttackTime {
			get {
				return prevAttackTime;
			}
		}

		public bool ReadyToShoot {
			get {
				int len = ConflictWeapons.Length;
				if (PrevAttackTime + AttackFrequency * (len + 1) > Time.time) {
					return false;
				}
				for (int i = 0; i < len; i++) {
					if (ConflictWeapons[i].PrevAttackTime + AttackFrequency > Time.time) {
						return false;
					}
				}
				return true;
			}
		}


		// Serialize

		[Header("Game")]
		[SerializeField]
		private float AttackFrequency = 0.4f;
		[SerializeField]
		private float BulletSpeed = 60f;
		[SerializeField]
		private float BulletSize = 0.3f;
		[SerializeField]
		private float RandomTimeGap = 0.05f;

		[Header("System")]
		[SerializeField]
		private AnimationCurve LerpCurveF = AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);
		[SerializeField]
		private AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f) });
		[SerializeField]
		private Vector3 CurveRandom = Vector3.zero;
		[SerializeField]
		private KeyCode AttackKey = KeyCode.Mouse0;

		[Header("Component")]
		[SerializeField]
		private ParticleSystem[] Particles = new ParticleSystem[0];
		[SerializeField]
		private Weapon[] ConflictWeapons;
		[SerializeField]
		private Transform Bullet;
		[SerializeField]
		private LerpLight TheLight;
		[SerializeField]
		private Transform model;
		[SerializeField]
		private Transform bulletSpawnPivot;

		// Data
		private float prevAttackTime = float.MinValue;
		private Vector3 InitLocalPos = Vector3.zero;
		private Vector3 CurrentCurveRandom = Vector3.zero;


		#endregion



		#region --- EDT ---

#if UNITY_EDITOR

		void Reset () {

			// Init conflict weapon
			List<Weapon> rws = new List<Weapon>(transform.parent.GetComponentsInChildren<Weapon>(true));
			if (rws.Contains(this)) {
				rws.Remove(this);
			}
			ConflictWeapons = rws.ToArray();

			// Init _m
			Transform _m = transform.FindChild("_m");
			if (_m) {
				model = _m;
			}

			// Init _s
			Transform _s = transform.FindChild("_s");
			if (!_s) {
				bulletSpawnPivot = new GameObject("_s").transform;
				bulletSpawnPivot.SetParent(transform);
				bulletSpawnPivot.localPosition = Vector3.zero;
				bulletSpawnPivot.localRotation = Quaternion.identity;
				bulletSpawnPivot.localScale = Vector3.one;
			} else {
				bulletSpawnPivot = _s;
			}

			// Init Light
			LerpLight _l = transform.parent.GetComponentInChildren<LerpLight>(true);
			if (_l) {
				TheLight = _l;
			}


			// Init Particles
			ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>(true);
			Particles = pss;

		}

#endif

		#endregion



		#region --- MSG ---



		void Awake () {
			InitLocalPos = Model.localPosition;
			StopAllParticles();
		}



		void Update () {
			KeyUpdate();
			ModelUpdate();
		}




		#endregion




		#region --- API ---



		public void Attack () {
			ShootBullet();
			TriggerLoghtOn();
		}



		#endregion




		#region --- LGC ---



		void KeyUpdate () {
			if (Input.GetKey(AttackKey)) {
				if (ReadyToShoot) {
					Attack();
					PlayAllParticles();
					CurrentCurveRandom = new Vector3(
						Random.Range(-CurveRandom.x, CurveRandom.x),
						Random.Range(-CurveRandom.y, CurveRandom.y),
						Random.Range(-CurveRandom.z, CurveRandom.z)
					);
					prevAttackTime = Time.time + Random.Range(0f, RandomTimeGap);
				}
			}
		}



		void ModelUpdate () {
			float t = Time.time - PrevAttackTime;
			Model.localPosition = InitLocalPos + Vector3.forward * LerpCurveF.Evaluate(t) + CurrentCurveRandom;
			Model.localScale = Vector3.one * ScaleCurve.Evaluate(t);
		}




		private void PlayAllParticles () {
			for (int i = 0; i < Particles.Length; i++) {
				Particles[i].Play();
			}
		}



		private void StopAllParticles () {
			for (int i = 0; i < Particles.Length; i++) {
				Particles[i].Stop();
			}
		}



		private void ShootBullet () {

			Transform tf = Instantiate(Bullet.gameObject).transform;
			tf.gameObject.SetActive(false);
			tf.position = bulletSpawnPivot.position;
			tf.rotation = bulletSpawnPivot.rotation;
			tf.gameObject.SetActive(true);
			tf.localScale = Vector3.one * BulletSize;

			Bullet b = tf.GetComponent<Bullet>();
			b.Rig.velocity = Vector3.ClampMagnitude(bulletSpawnPivot.forward, 1f) * BulletSpeed;
			b.Alive = true;
		}



		private void TriggerLoghtOn () {
			if (TheLight) {
				TheLight.TriggerOn();
			}
		}


		#endregion


	}
}