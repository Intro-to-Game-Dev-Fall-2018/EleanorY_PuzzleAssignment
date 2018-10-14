using System.Collections;
using System.Collections.Generic;
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
	private GameObject[] _boxs;


	public float Speed;
	public float GridSize;
	public Vector3 Movement;

	private bool _keyPressed;

	private Vector3 _pos;
	private Vector3 _tr;


	// Use this for initialization
	void Start()
	{
		_walls = GameObject.FindGameObjectsWithTag("Wall");
		_boxs = GameObject.FindGameObjectsWithTag("Box");
		_pos = transform.position;

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(Up) && _pos == transform.position && !_keyPressed)
		{
			_pos += Vector3.up * GridSize;
			_keyPressed = true;
			Movement = new Vector3(0, 1, 0);
		}

		if (Input.GetKey(Down) && _pos == transform.position && !_keyPressed)
		{
			_pos += Vector3.down * GridSize;
			_keyPressed = true;
			Movement = new Vector3(0, -1, 0);
		}

		if (Input.GetKey(Right) && _pos == transform.position && !_keyPressed)
		{
			_pos += Vector3.right * GridSize;
			_keyPressed = true;
			Movement = new Vector3(1, 0, 0);
			
		}

		if (Input.GetKey(Left) && _pos == transform.position && !_keyPressed)
		{
			_pos += Vector3.left * GridSize;
			_keyPressed = true;
			Movement = new Vector3(-1, 0, 0);
		}

		if (!Input.anyKeyDown && _keyPressed)
		{
			_keyPressed = false;
		}

		
		if (!Blocked())
		{
			transform.position = Vector3.MoveTowards(transform.position, _pos, Time.deltaTime * Speed);
		}
		else
		{
			_pos = transform.position;
		}
		
		
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Box") && other.gameObject.GetComponent<BoxController>().EnterObstacle)
		{
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			_pos = new Vector3(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.y),
				Mathf.Round(transform.position.z));
		}
	}
	

	bool Blocked()
	{
		foreach (var wall in _walls)
		{
			if (wall.transform.position == _pos)
			{
				return true;
			}
		}

		foreach (var box in _boxs)
		{
			if (box.GetComponent<BoxController>().Blocked())
			{
				return true;
			}
		}
		
		return false;

	}
	
	
}


