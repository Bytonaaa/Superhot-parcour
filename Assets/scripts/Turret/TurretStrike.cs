using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStrike : MonoBehaviour
{

    private GameObject player;
    [SerializeField] private LayerMask CheckLayer;
    [SerializeField] private LayerMask unCheckLayer;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float reloadTime;
    [SerializeField] private Transform[] InstantiatesTransform;

    private float time = 0f;

    public float TurretRadiurs { get { return radius; } }
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	    var t = GetComponent<SphereCollider>();
	    if (t != null)
	    {
	        radius = t.radius * Mathf.Max(transform.localScale.x, Mathf.Max(transform.localScale.y, transform.localScale.z));
	    }
	}
	
	// Update is called once per frame
	void Update () {
		TurnTurret();
	    if (time > 0f)
	    {
	        time -= Time.deltaTime;
	    }
	}

    void FixedUpdate()
    {
        RaycastHit rayHit;
        if (time <= 0f && Physics.Raycast(new Ray(transform.position, transform.forward), out rayHit, radius, CheckLayer | unCheckLayer, QueryTriggerInteraction.Collide))
        {
            if ((LayerMask.GetMask(LayerMask.LayerToName(rayHit.transform.gameObject.layer)) & unCheckLayer) == 0)
            {
                strike();
            }
        }
    }

    private void TurnTurret()
    {
        if (player != null)
        {
            transform.LookAt(player.transform.position + Vector3.down * player.GetComponent<CharacterController>().height*.15f);
        }
    }


    private int lastInst = -1;

    private Vector3 getRandomBulletPosition()
    {
        if (InstantiatesTransform.Length <= 1)
        {
            return InstantiatesTransform.Length == 0
                ? transform.position
                : InstantiatesTransform[0].position;
        }
        int temp = Random.Range(0, InstantiatesTransform.Length);
        while (temp == lastInst)
        {
            temp = Random.Range(0, InstantiatesTransform.Length);
        }
        lastInst = temp;
        return InstantiatesTransform[lastInst].position;
    }

    private void strike()
    {
        if (bullet != null)
        {
            var velocity = player.GetComponent<CharacterController>().velocity;
            velocity.y = 0f;
            var pos = getRandomBulletPosition();
            Instantiate(bullet, pos , Quaternion.LookRotation(player.transform.position + velocity.normalized * 2f - pos));
        }
        time = reloadTime;
    }
}
