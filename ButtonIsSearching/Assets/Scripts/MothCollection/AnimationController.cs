using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MapDrawCollection;
using StateManagement;
using StateManagement.PlayState;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Does the MothMovement and depending on that controlling the animations.
/// </summary>
/// <author> Julian Schreiter, Jannick Mitsch </author>
/// <date>08.01.2022</date>
public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private Rigidbody2D rb;
    private Vector2 direction2d;
    private Vector3 target3d;
    private float speed = 3f;
    private float maxDistance = 4f;
    private WayDirection walkDirection;
    private PlayStateManager _playStateManager;

    void Start()
    {
        _playStateManager = StateManager.GetInstance()._playStateManager;
        walkDirection = WayDirection.NONE;
        rb = GetComponent<Rigidbody2D>();
        target3d = transform.position;
        _animator.speed = _animator.speed * 2;
    }

    void Update()
    {
        switch (walkDirection)
        {
            case WayDirection.RIGHT_TOP:
                moveRightUpwards();
                break;
            case WayDirection.RIGHT:
                moveRightSideways();
                break;
            case WayDirection.RIGHT_BOTTOM:
                moveRightDownwards();
                break;
            case WayDirection.LEFT_BOTTOM:
                moveLeftDownwards();
                break;
            case WayDirection.LEFT:
                moveLeftSideways();
                break;
            case WayDirection.LEFT_TOP:
                moveLeftUpwards();
                break;
            default:
                DoIdleAnimation();
                break;
        }
    }

    void FixedUpdate()
    {
        IPlayState playState = _playStateManager.PlayState;
        
        if (playState == _playStateManager.Pause)
        {
            walkDirection = WayDirection.NONE;
            rb.MovePosition(target3d);
            return;
        }
        
        if (playState == _playStateManager.Speed)
        {
            if (Vector3.Distance(transform.position, target3d) >= 0.1f)
            {
                direction2d = new Vector2(target3d.x - transform.position.x, target3d.y - transform.position.y);
                direction2d.Normalize();
                direction2d = direction2d * speed * (float)(Math.Pow(_playStateManager.Speed.CurrentSpeed, -1));
                rb.MovePosition(rb.position + direction2d * Time.fixedDeltaTime);
            }
            else
            {
                walkDirection = WayDirection.NONE;
            }
        }
    }

    public void SetMothWalkTargetPos(Vector3Int currentPos, Vector3Int nextPos)
    {
        target3d = BuildingCreator.GetInstance().GetTilemap(Window.MAP_WINDOW, TilemapType.PREVIEW).CellToWorld(nextPos);
        walkDirection = new TilemapDrawer().CalculateWayDirection(currentPos, nextPos);
    }


    //methods to control the animations
    private void DoIdleAnimation()
    {
        _animator.SetInteger("direction", 6);
    }

    private void moveRightSideways()
    {
        _animator.SetInteger("direction", 0);
        _animator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void moveRightDownwards()
    {
        _animator.SetInteger("direction", 1);
        _animator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void moveLeftDownwards()
    {
        _animator.SetInteger("direction", 2);
        _animator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }

    private void moveLeftSideways()
    {
        _animator.SetInteger("direction", 3);
        _animator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }

    private void moveLeftUpwards()
    {
        _animator.SetInteger("direction", 4);
        _animator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }

    private void moveRightUpwards()
    {
        _animator.SetInteger("direction", 5);
        _animator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }
}