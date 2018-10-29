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
	
	

	private void Start()
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

	private void Update()
	{	
		_currentPos = transform.position;
		_offset = Player.transform.position - transform.position;
		if (_offset.x < -0.5f && _offset.x >= -1.0f && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x + 1,
				_currentPos.y,
				_currentPos.z);
			_playerStatus = 3;
		} 
		else if (_offset.x > 0.5f && _offset.x <= 1.0f  && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x - 1,
				_currentPos.y,
				_currentPos.z);
			_playerStatus = 4;
		}
		else if (_offset.y < -0.5f && _offset.y >= -1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x,
				_currentPos.y + 1,
				_currentPos.z);
			_playerStatus = 2;
		} 
		else if (_offset.y > 0.5f && _offset.y <= 1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x,
				_currentPos.y - 1,
				_currentPos.z);
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
		else
		{
			transform.position = new Vector3(
				Mathf.Round(_currentPos.x),
				Mathf.Round(_currentPos.y),
				Mathf.Round(_currentPos.z));
			TargetPos = transform.position;
		}

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
			}
			return false;
		}
}
