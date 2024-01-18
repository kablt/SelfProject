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
        //스크립트가 달려있는 오브젝트의 자식 오브젝트에 animator가 달려있으므로 GetComponentInChildren통해 자식오브젝트에 접근하고
        //<>안에 접근할 컴포넌트 이름을 넣어준다.
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
        //wDown(Walk)상태일때 속력값에 0.3곱해 이동을 느리게 한다. false시 기본속도 유지를 위해 *1f를 뒤에 써준다
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("IsRun", moveVec != Vector3.zero); //moveVec가 Vector3.zero이면 움직임이 없다는뜻, 즉 가만히 있는게 아니라면 IsRun은 True로 해준다
        anim.SetBool("IsWalk", wDown);//input amanager에 등록되있는 Walk의 단축키 left shift가 눌렸을 경우, IsWalk를 True로 만들어준다는 뜻

    }

    void Turn()
    {
        //우리위치에 우리가 나아가야할 방향을 더해준다.
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

            //시간차 만드는 함수 Invoke
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

