using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float WalkSpeed;
    [SerializeField] float RunSpeed;
    Vector3 MoveDirection;
    Vector3 Velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] float GroundCheckDistance;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] float Gravity;
    [SerializeField] float JumpHeight;
    private CharacterController controller;
    Animator anim;
    PlayerMovement stopMove;
    [SerializeField]bool isJumping;


    private void Start() 
    {
        controller=GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        stopMove=GetComponent<PlayerMovement>();
    }

    private void Update() 
    {
        Move();
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Move()
    {
        isGrounded=Physics.CheckSphere(transform.position,GroundCheckDistance,GroundMask);
        if(isGrounded && Velocity.y<0)
        {
            Velocity.y=-2;
        }
        float MoveZ = Input.GetAxis("Vertical");
        float MoveX = Input.GetAxis("Horizontal");
        
        MoveDirection= new Vector3(0,0,MoveZ);
        if(isGrounded)
        {
            if(MoveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (MoveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if(MoveDirection==Vector3.zero)
            {
                Idle();
            }
            MoveDirection*=MoveSpeed;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                Invoke("EnableMovement",2.5f);
                
            }
        }
     
        
        controller.Move(MoveDirection* Time.deltaTime);
        Velocity.y+=Gravity*Time.deltaTime;
        controller.Move(Velocity*Time.deltaTime);
    }

    void Idle()
    {
        anim.SetFloat("Speed",0,0.15f,Time.deltaTime);
    }
    void Walk()
    {
        MoveSpeed=WalkSpeed;
        anim.SetFloat("Speed",0.5f,0.215f,Time.deltaTime);
    }
    void Run()
    {
        MoveSpeed=RunSpeed;
        anim.SetFloat("Speed",1,0.15f,Time.deltaTime);
    }
    void Jump()
    {
       isJumping=true;
       if(isJumping==true)
       {
           stopMove.enabled=false;
       }
       anim.SetTrigger("Jump");
    }
    void Attack()
    {
        anim.SetTrigger("Attack");
    }
    void EnableMovement()
    {
        stopMove.enabled=true;
    }
}
