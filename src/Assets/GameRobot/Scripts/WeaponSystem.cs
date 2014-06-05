using UnityEngine;
using System.Collections;

public class WeaponSystem : MonoBehaviour
{
	public GameObject weapon;
	public GameObject laser;
	public GameObject laserDot;
	public GameObject muzzleFlash;
	public GameObject bullet;
	public GameObject unequipPos;
	public float equipTime = 1f;
	public float reloadTime = 0.2f;
	public AudioClip equipSound;
	public AudioClip shootSound;

	private bool _equipment = false;
	private bool _shooting = false;
	private Vector3 _equipPos;
	private Vector3 _startPos;
	private Vector3 _endPos;
	private float _time = 0;
	private float _reloadTime = 0;

	void Awake()
	{
		_equipPos = weapon.transform.localPosition;
		_endPos = unequipPos.transform.localPosition;
		_time = equipTime;
		_equipment = false;
		laser.SetActive(false);
		muzzleFlash.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_time >= equipTime)
			weapon.transform.localPosition = _endPos;
		else
		{
			weapon.transform.localPosition = Vector3.Lerp(_startPos, _endPos, _time / equipTime);
			_time += Time.deltaTime;
		}

		if(_shooting == true)
		{
			if(_reloadTime >= reloadTime)
			{
				GameObject go = (GameObject)GameObject.Instantiate(bullet);
				go.transform.parent = this.transform;
				go.transform.position = muzzleFlash.transform.position;
				go.transform.rotation = muzzleFlash.transform.rotation;
				Bullet instBullet = go.GetComponent<Bullet>();
				instBullet.Fire(Vector3.Distance(laser.transform.position, laserDot.transform.position));
				_reloadTime = 0;
			}
			else
				_reloadTime += Time.deltaTime;
		}
	}

	public bool equipment
	{
		get
		{
			return _equipment;
		}
		set
		{
			if(_equipment == value)
				return;

			_equipment = value;
			if(_equipment == true)
			{
				_startPos = unequipPos.transform.localPosition;
				_endPos = _equipPos;
			}
			else
			{
				_startPos = _equipPos;
				_endPos = unequipPos.transform.localPosition;
				shooting = false;
			}
			laser.SetActive(_equipment);
			_time = 0;
			audio.PlayOneShot(equipSound);
		}
	}

	public bool shooting
	{
		get
		{
			return _shooting;
		}
		set
		{
			if(_shooting == value)
				return;

			_shooting = value;
			if(_shooting == true)
			{
				audio.clip = shootSound;
				audio.loop = true;
				audio.Play();
				_reloadTime = reloadTime;
			}
			else
			{
				audio.Stop();
			}
			muzzleFlash.SetActive(_shooting);
		}
	}
}
