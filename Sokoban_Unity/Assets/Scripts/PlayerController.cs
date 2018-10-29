using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private int _direction;

	private bool _push;
	private GameObject _boxPushed;
	private bool _back;

	private bool _keyPressed;

	private Vector3 _pos;
	private Vector3 _tr;

	private bool _setOffset;


	private Vector3 _lastPlayerPos;
	private Vector3 _lastBoxPos;
	

	[SerializeField] private SpriteRenderer _sprRen;

	public Sprite Horizontal1;
	public Sprite Horizontal2;
	public Sprite Vertical1;
	public Sprite Vertical2;
	public Sprite Victory;
	public Sprite HorizontalPush1;
	public Sprite HorizontalPush2;
	public Sprite VerticalPush1;
	public Sprite VerticalPush2;

	public int Move;
	private bool _animDir;

	public GameManager GameManager;

	public bool GameStart;


	private void Start()
	{
		_back = true;
		Move = 0;
		_walls = GameObject.FindGameObjectsWithTag("Wall");
		_boxes = GameObject.FindGameObjectsWithTag("Box");
		_boxes = GameObject.FindGameObjectsWithTag("Box");
		_pos = transform.position;
		_sprRen = GetComponent<SpriteRenderer>();
		_sprRen.sprite = Horizontal2;
	}


	private void Update()
	{
		if (GameStart)
		{
			if (Input.GetKey(Up))
			{
				_direction = 1;
				PlayerMove();
			}

			if (Input.GetKey(Down))
			{
				_direction = 2;
				PlayerMove();
			}
			
			if (Input.GetKey(Right))
			{
				_direction = 3;
				PlayerMove();
			}
			
			if (Input.GetKey(Left))
			{
				_direction = 4;
				PlayerMove();
			}
			if (Input.GetKeyUp(Back))
			{
				_pos = _lastPlayerPos;
				if (!_back)
				{
					Move--;
					if (_push)
					{
						_boxPushed.GetComponent<BoxController>().TargetPos = _lastBoxPos;
					}
					_back = true;
				}

			}
			if (!Input.anyKeyDown && transform.position == _pos)
			{
				_keyPressed = false;
				Movement = new Vector3(0, 0, 0);
				_direction = 0;
			}
		}

		if (!Blocked())
		{
			transform.position = Vector3.MoveTowards(transform.position, _pos, Time.deltaTime * Speed);
		}
		else
		{
			_pos = new Vector3(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.y),
				Mathf.Round(transform.position.z));
		}

		if (Input.anyKey && GameStart)
		{
			switch (_direction)
			{
				case 1:
					_sprRen.flipX = false;
					_sprRen.flipY = false;

					break;
				case 2:
					_sprRen.flipX = false;
					_sprRen.flipY = true;
					break;
				case 3:
					_sprRen.flipX = false;
					_sprRen.flipY = false;
					break;
				case 4:
					_sprRen.flipX = true;
					_sprRen.flipY = false;
					break;
				case 0:
					break;
			}
		}

		if (!GameManager.AllSet())
		{
			if (_animDir)
			{
				if (!_push)
				{
					if (Move % 2 == 0)
					{
						_sprRen.sprite = Vertical1;
					}
					else
					{
						_sprRen.sprite = Vertical2;
					}
				}
				else
				{
					if (Move % 2 == 0)
					{
						_sprRen.sprite = VerticalPush1;
					}
					else
					{
						_sprRen.sprite = VerticalPush2;
					}
				}
			}
			else
			{
				if (!_push)
				{
					if (Move % 2 == 0)
					{
						_sprRen.sprite = Horizontal1;
					}
					else
					{
						_sprRen.sprite = Horizontal2;
					}
				}
				else
				{
					if (Move % 2 == 0)
					{
						_sprRen.sprite = HorizontalPush1;
					}
					else
					{
						_sprRen.sprite = HorizontalPush2;
					}
				}
			}
		}
		else
		{
			_sprRen.sprite = Victory;
			Speed = 0.0f;
		}


		if (_push && Vector3.Distance(_boxPushed.transform.position, transform.position) > 1.4f)
		{
			_push = false;
		}		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Box"))
		{
			other.GetComponent<BoxController>().TargetPos += Movement;
			_boxPushed = other.gameObject;
			_push = true;
			Debug.Log("Enter");
			_lastBoxPos = other.transform.position;
		}
	}

	private bool Blocked()
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


	private void PlayerMove()
	{
		switch (_direction)
		{
			case 1:
				Movement = Vector3.up;
				_animDir = true;
				break;
			case 2:
				Movement = Vector3.down;
				_animDir = true;
				break;
			case 3:
				Movement = Vector3.right;
				_animDir = false;
				break;
			case 4:
				Movement = Vector3.left;
				_animDir = false;
				break;
			default:
				Movement = Vector3.zero;
				break;
		}
		if (_pos == transform.position && !_keyPressed)
		{
			if (!Blocked())
			{
				_lastPlayerPos = new Vector3(
					Mathf.Round(transform.position.x),
					Mathf.Round(transform.position.y),
					Mathf.Round(transform.position.z));
			}
			if (_push && !_boxPushed.GetComponent<BoxController>().Blocked())
			{
				_lastBoxPos = new Vector3(
					Mathf.Round(_boxPushed.transform.position.x),
					Mathf.Round(_boxPushed.transform.position.y),
					Mathf.Round(_boxPushed.transform.position.z));
			}
			_pos += Movement * GridSize;
			_keyPressed = true;
			if (!Blocked())
			{
				Move++;
				_back = false;
			}
		}
	}
}



