using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {
	private Vector3 _currentPos;
	public Vector3 TargetPos;
	private Vector3 _expectTarget;
	
	
	private GameObject[] _walls;
	private GameObject[] _boxes;
	private GameObject[] _goals;

	public GameObject Player;
	private Vector3 _offset;
	
	private KeyCode _up;
	private KeyCode _down;
	private KeyCode _left;
	private KeyCode _right;
	private KeyCode _back;

	private int _playerStatus;

	private SpriteRenderer _spriteRenderer;
	public Sprite DarkBox;
	public Sprite LightBox;

	public bool ReachGoal;
	
	

	void Start()
	{
		_walls = GameObject.FindGameObjectsWithTag("Wall");
		_boxes = GameObject.FindGameObjectsWithTag("Box");
		_goals = GameObject.FindGameObjectsWithTag("Goal");
		_up = Player.GetComponent<PlayerController>().Up;
		_down = Player.GetComponent<PlayerController>().Down;
		_left = Player.GetComponent<PlayerController>().Left;
		_right = Player.GetComponent<PlayerController>().Right;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		TargetPos = transform.position;
		_expectTarget = transform.position;
	}

	void Update()
	{	
		_currentPos = transform.position;
		_offset = Player.transform.position - transform.position;
		if (_offset.x < -0.5f && _offset.x >= -1.0f && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x + 1,
				_currentPos.y,
				_currentPos.z);
//				Mathf.Round(_currentPos.x + 1),
//				Mathf.Round(_currentPos.y),
//				Mathf.Round(_currentPos.z));
//			Debug.Log("left");
//			Debug.Log(Blocked());
//			Debug.Log(_expectTarget);
			_playerStatus = 3;
		} 
		else if (_offset.x > 0.5f && _offset.x <= 1.0f  && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x - 1,
				_currentPos.y,
				_currentPos.z);
//			Debug.Log("right");
//			Debug.Log(Blocked());
//			Debug.Log(_expectTarget);
			_playerStatus = 4;
		}
		else if (_offset.y < -0.5f && _offset.y >= -1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x,
				_currentPos.y + 1,
				_currentPos.z);
//			Debug.Log("down");
//			Debug.Log(Blocked());
//			Debug.Log(_expectTarget);
			_playerStatus = 2;
		} 
		else if (_offset.y > 0.5f && _offset.y <= 1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x,
				_currentPos.y - 1,
				_currentPos.z);
//			Debug.Log("up");
//			Debug.Log(Blocked());
//			Debug.Log(_expectTarget);
//			Debug.Log(_playerStatus);
			_playerStatus = 1;
		}
		else
		{
			_expectTarget = transform.position;
			_playerStatus = 0;
		}

		if (!Blocked())
		{
		transform.position = Vector3.MoveTowards(_currentPos, TargetPos, Time.deltaTime * 8);
		}
//		else
//		{
//			transform.position = new Vector3(
//				Mathf.Round(_currentPos.x),
//				Mathf.Round(_currentPos.y),
//				Mathf.Round(_currentPos.z));
//			TargetPos = transform.position;
//		}

//		if (!Input.anyKeyDown)
//		{
//			transform.position = new Vector3(
//				Mathf.Round(_currentPos.x),
//				Mathf.Round(_currentPos.y),
//				Mathf.Round(_currentPos.z));
//		}

		foreach (var goal in _goals)
		{
			if (transform.position == goal.transform.position)
			{
				_spriteRenderer.sprite = DarkBox;
				ReachGoal = true;
				return;
			}
			ReachGoal = false;
			_spriteRenderer.sprite = LightBox;			
		}
		
	}



//	private void OnCollisionExit2D(Collision2D other)
//	{
//		if (other.gameObject.CompareTag("Player"))
//		{
////			_targetPos = transform.position;
//			transform.position = new Vector3(
//				Mathf.Round(_currentPos.x),
//				Mathf.Round(_currentPos.y),
//				Mathf.Round(_currentPos.z));
//		}
//
//		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Box"))
//		{
//			EnterObstacle = false;
//			GetComponent<Rigidbody2D>().isKinematic = false;
//		}
//	}
//	private void OnCollisionEnter2D(Collision2D other)
//	{
//		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Box"))
//		{
//			EnterObstacle = true;
//			Debug.Log("EnterObstacle");
//			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
//			transform.position = new Vector3(
//				Mathf.Round(_currentPos.x),
//				Mathf.Round(_currentPos.y),
//				Mathf.Round(_currentPos.z));
//			GetComponent<Rigidbody2D>().isKinematic = true;
//		}
//	}
	
	
	public bool Blocked()
	{
		foreach (var wall in _walls)
		{
			if (wall.transform.position == _expectTarget)
			{
				switch (_playerStatus)
				{
					case 0:
						return false;
					case 1:
						if (Input.GetKey(_down))
						{
							return true;

						}
						else
						{
							return false;
						}
					case 2:
						if (Input.GetKey(_up))
						{
							return true;
						}
						else
						{
							return false;
						}
					case 3:
						if (Input.GetKey(_right))
						{
							return true;
						}
						else
						{
							return false;
						}
					case 4:
						if (Input.GetKey(_left))
						{
							return true;
						}
						else
						{
							return false;
						}
				}
			}
		}
//			Vector3 distance = wall.transform.position - TargetPos;
//			Vector3 direction = wall.transform.position - transform.position;
//			if (Mathf.Abs(distance.x) < 0.02f && Mathf.Abs(distance.y) < 0.02f)
//			{
//				if (direction.x > 0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_right))
//				{
//					return true;
//				}
//				if (direction.x < -0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_left))
//				{
//					return true;
//				}
//				if (direction.y > 0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_up))
//				{
//					return true;
//				}
//				if (direction.y < -0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_down))
//				{
//					return true;
//				}
//			}

			foreach (var box in _boxes)
			{
				if (box != this.gameObject && box.transform.position == _expectTarget)
				{
					switch (_playerStatus)
					{
						case 0:
							return false;
						case 1:
							if (Input.GetKey(_down))
							{
								return true;
							}
							else
							{
								return false;
							}
						case 2:
							if (Input.GetKey(_up))
							{
								return true;
							}
							else
							{
								return false;
							}
						case 3:
							if (Input.GetKey(_right))
							{
								return true;
							}
							else
							{
								return false;
							}
						case 4:
							if (Input.GetKey(_left))
							{
								return true;
							}
							else
							{
								return false;
							}
					}
				}

//			Vector3 distance = box.transform.position - TargetPos;
//			Vector3 direction = box.transform.position - transform.position;
//			if (Mathf.Abs(distance.x) < 0.02f && Mathf.Abs(distance.y) < 0.02f)
//			{
//				if (direction.x > 0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_right))
//				{
//					return true;
//				}
//				if (direction.x < -0.02f && Mathf.Abs(direction.y) < 0.02f && Input.GetKey(_left))
//				{
//					return true;
//				}
//				if (direction.y > 0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_up))
//				{
//					return true;
//				}
//				if (direction.y < -0.02f && Mathf.Abs(direction.x) < 0.02f && Input.GetKey(_down))
//				{
//					return true;
//				}
//			}
			}
			return false;
		}
}
