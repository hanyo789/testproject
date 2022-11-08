using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    public float moveSpeed;

    public bool IsMoving {get; private set;}
    CharacterAnimator animator;
    private void Awake() {
        animator = GetComponent<CharacterAnimator>();
    }
   

   public IEnumerator Move(Vector2 moveVec, Action OnMoveOver=null)
   {

        animator.MoveX = moveVec.x;
        animator.MoveY = moveVec.y;

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!isWalkable(targetPos))
            yield break;
 
       IsMoving  = true;
       
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
           transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
           yield return null;
        }
        transform.position = targetPos;
       
        IsMoving = false;

        OnMoveOver?.Invoke();
   }

   public void HandleUpdate(){
    animator.IsMoving = IsMoving;
   }
   private bool isWalkable(Vector3 targetPos)
   {
   if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null )
   {
       return false;

   }
        return true;
   }
   public CharacterAnimator Animator{
    get => animator;
   }

}
