using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{

	public KeyCode Up;
	public KeyCode Down;
	public KeyCode Left;
	public KeyCode Right;
	public KeyCode Back;

	private GameObject[] _walls;
	private GameObject[] _boxes;


	public float Speed;
	public float GridSize;
	public Vector3 Movement;

//	private float _fracMovement;
//	private float _timer;

	private bool _keyPressed;

	private Vector3 _pos;
	private Vector3 _tr;

	private bool _setOffset;

	
	void Start()
	{
		_walls = GameObject.FindGameObjectsWithTag("Wall");
		_boxes = GameObject.FindGameObjectsWithTag("Box");
		_pos = transform.position;

	}


	void Update()
	{
		if (Input.GetKey(Up))
		{
			Movement = new Vector3(0, 1, 0);
			if (_pos == transform.position && !_keyPressed)
			{
				_pos += Vector3.up * GridSize;
				_keyPressed = true;
			}
		}

		if (Input.GetKey(Down))
		{
			Movement = new Vector3(0, -1, 0);
			if (_pos == transform.position && !_keyPressed)
			{
				_pos += Vector3.down * GridSize;
				_keyPressed = true;
			}
		}

		if (Input.GetKey(Right))
		{
			if (_pos == transform.position && !_keyPressed)
			{
				_pos += Vector3.right * GridSize;
				_keyPressed = true;
				Movement = new Vector3(1, 0, 0);
			}
		}

		if (Input.GetKey(Left))
		{
			Movement = new Vector3(-1, 0, 0);
			if (_pos == transform.position && !_keyPressed)
			{
				_pos += Vector3.left * GridSize;
				_keyPressed = true;
			}
		}

		if (!Input.anyKeyDown && transform.position == _pos)
		{
			_keyPressed = false;
//			_pos = transform.position;
//			if (transform.position == _pos)
//			{
				Movement = new Vector3(0, 0, 0);
//			}
		}


		
		if (!Blocked())
		{
			transform.position = Vector3.MoveTowards(transform.position, _pos, Time.deltaTime * Speed );
		}
		else 
		{
			_pos = new Vector3(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.y),
				Mathf.Round(transform.position.z));
		}


	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Box"))
		{
			other.GetComponent<BoxController>().TargetPos += Movement;
		}
	}


//	private void OnCollisionEnter2D(Collision2D other)
//	{
//		if (other.gameObject.CompareTag("Box") && other.gameObject.GetComponent<BoxController>().EnterObstacle)
//		{
//			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
//			_pos = new Vector3(
//				Mathf.Round(transform.position.x),
//				Mathf.Round(transform.position.y),
//				Mathf.Round(transform.position.z));
//		}
//
//		if (other.gameObject.CompareTag("Wall"))
//		{
//			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
//			_pos = new Vector3(
//				Mathf.Round(transform.position.x),
//				Mathf.Round(transform.position.y),
//				Mathf.Round(transform.position.z));
//		}
//	}se

	
	bool Blocked()
	{
	
			foreach (var wall in _walls)
			{
				if (wall.transform.position == _pos)
				{
					return true;
				}
			}
			foreach (var box in _boxes)
			{
				if (box.GetComponent<BoxController>().Blocked())
				{
					return true;
				}
			}
	
		return false;
	}
}


