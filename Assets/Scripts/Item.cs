using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //enum을 통해 저장할 타입을 열거한것
    public enum Type {Ammo, Coin , Grenade , Heart , Weapon };
    public Type type;
    public int value;

    private void Update()
    {
        //아이템 회전을 구현, Vector3.up은 해당 아이템의 y축을 기준으로 잡은것
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
}
