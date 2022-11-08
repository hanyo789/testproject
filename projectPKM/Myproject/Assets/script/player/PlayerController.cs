using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed;
   public LayerMask solidObjectsLayer;
   public LayerMask interactableLayer;
   public LayerMask grassLayer;
   public event Action OnEncountered;

   private bool isMoving;
   private Vector2 input;
   private CharacterAnimator animator;
   private void Awake()
   {
       animator = GetComponent<CharacterAnimator>();
   }

   
    

   public void HandleUpdate()
   {
       if(!isMoving)
       {
           input.x = Input.GetAxisRaw("Horizontal");
           input.y = Input.GetAxisRaw("Vertical");

            // remove diagonal movement
           if (input.x != 0) input.y = 0;

           if (input != Vector2.zero)
           {
               animator.MoveX = input.x;
               animator.MoveY = input.y;

               var targetPos = transform.position;
               targetPos.x += input.x;
               targetPos.y += input.y;

               if (isWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
                    
           }
       }
    
        animator.IsMoving = isMoving;
        if (Input.GetKeyDown(KeyCode.Z))
            Interact();
   }

   void Interact()
   {
    var facingDir = new Vector3(animator.MoveX, animator.MoveY);
    var interactPos = transform.position + facingDir;

    //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

    var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
    if (collider != null)
    {
        collider.GetComponent<Interactable>()?.Interact();
    }
   }

   IEnumerator Move(Vector3 targetPos)
   {
       isMoving = true;
       
       while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
       {
           transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
           yield return null;
       }
       transform.position = targetPos;
       
       isMoving = false;

       CheckForEncounters();
   }
   private bool isWalkable(Vector3 targetPos)
   {
   if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null )
   {
       return false;

   }
        return true;
   }

   

    private void CheckForEncounters()
   {
       if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
       {
           if (UnityEngine.Random.Range(1, 101) <= 10)
           {
                animator.IsMoving = false;
                OnEncountered();
            
                
           }
           

       }

   }
}