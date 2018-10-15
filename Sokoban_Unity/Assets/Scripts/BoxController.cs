using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {
	private Vector3 _currentPos;
	private Vector3 _targetPos;
	
	
	private GameObject[] _walls;
	private GameObject[] _boxs;

	public GameObject Player;
	private Vector3 _offset;
	
	private KeyCode _up;
	private KeyCode _down;
	private KeyCode _left;
	private KeyCode _right;
	private KeyCode _back;
	public bool EnterObstacle;
	

	void Start()
	{
		_walls = GameObject.FindGameObjectsWithTag("Wall");
		_boxs = GameObject.FindGameObjectsWithTag("Box");
		_up = Player.GetComponent<PlayerController>().Up;
		_down = Player.GetComponent<PlayerController>().Down;
		_left = Player.GetComponent<PlayerController>().Left;
		_right = Player.GetComponent<PlayerController>().Right;
	}

	void Update()
	{
		_currentPos = transform.position;
		_offset = Player.transform.position - transform.position;
		if (_offset.x < -0.5f && _offset.x >= -1.0f && Mathf.Abs(_offset.y) < 0.8f)
		{
			_targetPos = transform.position + Vector3.right;
//			Debug.Log("left");
//			Debug.Log("TargetPos: " + _targetPos);
//			Debug.Log(Blocked());
		} 
		else if (_offset.x > 0.5f && _offset.x <= 1.0f  && Mathf.Abs(_offset.y) < 0.8f)
		{
			_targetPos = transform.position + Vector3.left;
//			Debug.Log("right");
//			Debug.Log("TargetPos: " + _targetPos);
//			Debug.Log(Blocked());
		}
		else if (_offset.y < -0.5f && _offset.y >= -1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_targetPos = transform.position + Vector3.up;
//			Debug.Log("down");
//			Debug.Log("TargetPos: " + _targetPos);
//			Debug.Log(Blocked());
		} 
		else if (_offset.y > 0.5f && _offset.y <= 1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_targetPos = transform.position + Vector3.down;
//			Debug.Log("up");
//			Debug.Log("TargetPos: " + _targetPos);
//			Debug.Log(Blocked());
		}
		else
		{
//			Debug.Log("NotTouched");
			_targetPos = transform.position;
//			Debug.Log(Blocked());
		}


		if (Blocked())
		{
//			Debug.Log("Blocked");
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			transform.position = new Vector3(
				Mathf.Round(_currentPos.x),
				Mathf.Round(_currentPos.y),
				Mathf.Round(_currentPos.z));
		}
	}
	

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
//			_targetPos = transform.position;
			transform.position = new Vector3(
				Mathf.Round(_currentPos.x),
				Mathf.Round(_currentPos.y),
				Mathf.Round(_currentPos.z));
		}

		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Box"))
		{
			EnterObstacle = false;
			GetComponent<Rigidbody2D>().isKinematic = false;
		}
	}
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Box"))
		{
			EnterObstacle = true;
			Debug.Log("EnterObstacle");
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			transform.position = new Vector3(
				Mathf.Round(_currentPos.x),
				Mathf.Round(_currentPos.y),
				Mathf.Round(_currentPos.z));
			GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}
	
	
	public bool Blocked()
	{
		foreach (var wall in _walls)
		{
			Vector3 distance = wall.transform.position - _targetPos;
			Vector3 direction = wall.transform.position - transform.position;
			if (Mathf.Abs(distance.x) < 0.02f && Mathf.Abs(distance.y) < 0.02f)
			{
				if (direction.x > 0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_right))
				{
					return true;
				}
				if (direction.x < -0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_left))
				{
					return true;
				}
				if (direction.y > 0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_up))
				{
					return true;
				}
				if (direction.y < -0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_down))
				{
					return true;
				}
			}
		}

		foreach (var box in _boxs)
		{
			Vector3 distance = box.transform.position - _targetPos;
			Vector3 direction = box.transform.position - transform.position;
			if (Mathf.Abs(distance.x) < 0.02f && Mathf.Abs(distance.y) < 0.02f)
			{
				if (direction.x > 0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_right))
				{
					return true;
				}
				if (direction.x < -0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_left))
				{
					return true;
				}
				if (direction.y > 0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_up))
				{
					return true;
				}
				if (direction.y < -0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_down))
				{
					return true;
				}
			}
		}
		return false;

	}
}
