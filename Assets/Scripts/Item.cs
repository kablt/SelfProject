using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    //enum�� ���� ������ Ÿ���� �����Ѱ�
    public enum Type {Ammo, Coin , Grenade , Heart , Weapon };
    public Type type;
    public int value;

    Rigidbody rb;
    SphereCollider sphereCollider;

    void Awake()
    {
       rb= GetComponent<Rigidbody>();
        //����������Ʈ�� 2���̻� �϶� ��������Ʈ�� �ҷ��ý� �������� �ִ°��� �ҷ��´�.
        sphereCollider= GetComponent<SphereCollider>();
    }
    private void Update()
    {
        //������ ȸ���� ����, Vector3.up�� �ش� �������� y���� �������� ������
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Floor")
        {
            rb.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
