using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public GameObject model;
	public GameObject explosion;
	public float speed = 10f;
	public float lifeTime = 0.5f;

	private float _time;
	private float _distance;

	void Awake()
	{
		this.enabled = false;
		model.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += transform.forward * speed * Time.deltaTime;
		_distance -= speed * Time.deltaTime;
		_time += Time.deltaTime;

		if(_time > lifeTime || _distance < 0)
		{
			if(_distance < 0)
			{
				GameObject go = (GameObject)GameObject.Instantiate(explosion);
				go.transform.parent = transform.parent;
				go.transform.position = transform.position;
			}
			GameObject.Destroy(gameObject);
		}
	}

	public void Fire(float distance)
	{
		_distance = distance;
		this.enabled = true;
		model.SetActive(true);
	}
}
