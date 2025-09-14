using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Character : MonoBehaviour
{
    public float moveSpeed;

    public bool IsMoving { get; set; }
    public float OffsetY { get; private set; } = 0.3f;
    CharacterAnimator animator;
    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    }

    public void SetPositionAndSnapToTile(Vector3 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    public IEnumerator Move(Vector3 moveVec,Action OnMoveOver = null, bool checkCollisions = true)
    {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1 , 1);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1 , 1);

        Vector3 targetPos = gameObject.transform.position;
        targetPos += moveVec;

        var ledge = CheckForLedge(targetPos);

        if(ledge!=null)
        {
            if (ledge.TryToJump(this, moveVec))
            {
                yield break;
            }
        }

        if (checkCollisions && !IsPathClear(targetPos))
            yield break;

        if (animator.IsSurfing && Physics2D.OverlapCircle(targetPos, 0.3f, GameLayers.i.WaterLayer) == null)
            animator.IsSurfing = false;

        IsMoving = true;

        while ((targetPos - gameObject.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.transform.position = targetPos;

        IsMoving = false;
        
        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        var collisionLayer = GameLayers.i.SolidObjectsLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer;

        if (!animator.IsSurfing)
            collisionLayer = collisionLayer | GameLayers.i.WaterLayer;

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, collisionLayer ))
            return false;
        
        return true;
    }

    bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidObjectsLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    Ledge CheckForLedge(Vector3 targetPos)
    {
        var collider = Physics2D.OverlapCircle(targetPos, 0.15f, GameLayers.i.LedgeLayer);
        return collider?.GetComponent<Ledge>();
        
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xdiff == 0 || ydiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xdiff, -1, 1);
            animator.MoveY = Mathf.Clamp(ydiff, -1, 1);
        }
        else
            Debug.LogError("Error in Look Towards : You can't ask the character to look diagonally");
        
    }

    public CharacterAnimator Animator => animator;
}
