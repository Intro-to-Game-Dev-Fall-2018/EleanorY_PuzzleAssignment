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

	private bool _keyPressed;

	private Vector3 _pos;
	private Vector3 _tr;

	private bool _setOffset;
	private bool _pushBox;

	[SerializeField] private SpriteRenderer _sprRen;
//	[SerializeField] private Animator _animator;

	public Sprite Horizontal1;
	public Sprite Horizontal2;
	public Sprite Vertical1;
	public Sprite Vertical2;
	
	public int Move;
	private bool _animDir;


	void Start()
	{
		Move = 0;
		_walls = GameObject.FindGameObjectsWithTag("Wall");
		_boxes = GameObject.FindGameObjectsWithTag("Box");
		_pos = transform.position;
		_sprRen = GetComponent<SpriteRenderer>();
//		_animator = GetComponent<Animator>();
		_sprRen.sprite = Horizontal2;
	}


	void Update()
	{
		Debug.Log(transform.localScale);
		if (Input.GetKey(Up))
		{
			_animDir = true;
			Movement = new Vector3(0, 1, 0);
			_direction = 1;
			if (_pos == transform.position && !_keyPressed)
			{
				_pos += Vector3.up * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
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
				_pos += Vector3.down * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
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
				_pos += Vector3.right * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
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
				_pos += Vector3.left * GridSize;
				_keyPressed = true;
				if (!Blocked())
				{
					Move++;
				}
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
					transform.localScale = new Vector3(1, 1, 1);
//						_animator.SetBool("HorizontalMove", false);
//						_animator.SetBool("VerticalMove", true);
					break;
				case 2:
					transform.localScale = new Vector3(1, -1, 1);
//						_animator.SetBool("HorizontalMove", false);
//						_animator.SetBool("VerticalMove", true);
					break;
				case 3:
//						_animDir = false;
					transform.localScale = new Vector3(1, 1, 1);
//						_animator.SetBool("HorizontalMove", true);
//						_animator.SetBool("VerticalMove", false);
					break;
				case 4:
//						_animDir = false;
					transform.localScale = new Vector3(-1, 1, 1);
//						_animator.SetBool("HorizontalMove", true);
//						_animator.SetBool("VerticalMove", false);
					break;
				case 0:
//						_animator.SetBool("HorizontalMove", false);
//						_animator.SetBool("VerticalMove", false);
					break;
			}
		}
		if (_animDir)
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
				_sprRen.sprite = Horizontal1;
			}
			else
			{
				_sprRen.sprite = Horizontal2;;
			}
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


