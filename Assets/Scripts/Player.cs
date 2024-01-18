using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    float hAxis;
    float vAxis;
    public float speed = 15.0f;
    bool wDown;
    bool jDown;
    bool isJump;
    bool isDodge;
    Rigidbody rb;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Animator anim;

    void Awake()
    {   
        //��ũ��Ʈ�� �޷��ִ� ������Ʈ�� �ڽ� ������Ʈ�� animator�� �޷������Ƿ� GetComponentInChildren���� �ڽĿ�����Ʈ�� �����ϰ�
        //<>�ȿ� ������ ������Ʈ �̸��� �־��ش�.
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
        {
            moveVec = dodgeVec;
        }
        //wDown(Walk)�����϶� �ӷ°��� 0.3���� �̵��� ������ �Ѵ�. false�� �⺻�ӵ� ������ ���� *1f�� �ڿ� ���ش�
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("IsRun", moveVec != Vector3.zero); //moveVec�� Vector3.zero�̸� �������� ���ٴ¶�, �� ������ �ִ°� �ƴ϶�� IsRun�� True�� ���ش�
        anim.SetBool("IsWalk", wDown);//input amanager�� ��ϵ��ִ� Walk�� ����Ű left shift�� ������ ���, IsWalk�� True�� ������شٴ� ��

    }

    void Turn()
    {
        //�츮��ġ�� �츮�� ���ư����� ������ �����ش�.
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            //�ð��� ����� �Լ� Invoke
            Invoke("DodgeOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

}

