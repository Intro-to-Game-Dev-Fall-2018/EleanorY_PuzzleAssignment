using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Timers;
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


	void Start()
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


	void Update()
	{
		Debug.Log(_boxPushed);
		if (Input.GetKey(Up))
		{
			_animDir = true;
			Movement = new Vector3(0, 1, 0);
			_direction = 1;
			if (_pos == transform.position && !_keyPressed)
			{
				_lastPlayerPos = new Vector3(
					Mathf.Round(transform.position.x),
					Mathf.Round(transform.position.y),
					Mathf.Round(transform.position.z));
				if (_push)
				{
					_lastBoxPos = new Vector3(
						Mathf.Round(_boxPushed.transform.position.x),
						Mathf.Round(_boxPushed.transform.position.y),
						Mathf.Round(_boxPushed.transform.position.z));
				}
				_pos += Vector3.up * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
					_back = false;
				}
			}
		}

		if (Input.GetKey(Down))
		{
			_animDir = true;
			Movement = new Vector3(0, -1, 0);
			_direction = 2;
			if (_pos == transform.position && !_keyPressed)
			{
				_lastPlayerPos = new Vector3(
					Mathf.Round(transform.position.x),
					Mathf.Round(transform.position.y),
					Mathf.Round(transform.position.z));
				if (_push)
				{
					_lastBoxPos = new Vector3(
						Mathf.Round(_boxPushed.transform.position.x),
						Mathf.Round(_boxPushed.transform.position.y),
						Mathf.Round(_boxPushed.transform.position.z));
				}
				_pos += Vector3.down * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
					_back = false;
				}
			}
		}

		if (Input.GetKey(Right))
		{
			_animDir = false;
			Movement = new Vector3(1, 0, 0);
			_direction = 3;
			if (_pos == transform.position && !_keyPressed)
			{
				_lastPlayerPos = new Vector3(
					Mathf.Round(transform.position.x),
					Mathf.Round(transform.position.y),
					Mathf.Round(transform.position.z));
				if (_push)
				{
					_lastBoxPos = new Vector3(
						Mathf.Round(_boxPushed.transform.position.x),
						Mathf.Round(_boxPushed.transform.position.y),
						Mathf.Round(_boxPushed.transform.position.z));
				}
				_pos += Vector3.right * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
					_back = false;
				}
			}
		}

		if (Input.GetKey(Left))
		{
			_animDir = false;
			Movement = new Vector3(-1, 0, 0);
			_direction = 4;
			if (_pos == transform.position && !_keyPressed)
			{
				_lastPlayerPos = new Vector3(
					Mathf.Round(transform.position.x),
					Mathf.Round(transform.position.y),
					Mathf.Round(transform.position.z));
				if (_push)
				{
					_lastBoxPos = new Vector3(
						Mathf.Round(_boxPushed.transform.position.x),
						Mathf.Round(_boxPushed.transform.position.y),
						Mathf.Round(_boxPushed.transform.position.z));
				}
				_pos += Vector3.left * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
					_back = false;
				}
			}
		}

		if (Input.GetKeyUp(Back))
		{
			_pos = _lastPlayerPos;
			if (_push)
			{
				_boxPushed.GetComponent<BoxController>().TargetPos = _lastBoxPos;
			}

			if (!_back)
			{
				Move--;
				_back = true;
			}

		}

		if (!Input.anyKeyDown && transform.position == _pos)
		{
				_keyPressed = false;
				Movement = new Vector3(0, 0, 0);
				_direction = 0;
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

			if (Input.anyKey)
			{
				switch (_direction)
				{
					case 1:
						_sprRen.flipX = false;
						_sprRen.flipY = false;
//						transform.localScale = new Vector3(1, 1, 1);

						break;
					case 2:
						_sprRen.flipX = false;
						_sprRen.flipY = true;
//						transform.localScale = new Vector3(1, -1, 1);
						break;
					case 3:
						_sprRen.flipX = false;
						_sprRen.flipY = false;
//						transform.localScale = new Vector3(1, 1, 1);
						break;
					case 4:
						_sprRen.flipX = true;
						_sprRen.flipY = false;
//						transform.localScale = new Vector3(-1, 1, 1);
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
			_lastBoxPos = other.transform.position;
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



