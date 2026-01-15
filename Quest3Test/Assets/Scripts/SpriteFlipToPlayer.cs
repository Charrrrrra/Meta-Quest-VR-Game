using System;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer3D : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    void Update()
    {
        if (player == null) return;
        
        // 计算指向玩家的方向（忽略Y轴高度差）
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // 保持水平旋转
        
        // 沿Y轴旋转
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}



// if (myself.being)
// {
//     name = null;
//     memory = null;
//     existence = null;
//     being.recoded();
// }

// public Human recoded()
// {
//     body.reconstructed();
//     memory.reorgnaized();
//     symbolized(name);
//     Human me(body, memory, name);
//     return me;
// }

// public class Human
// {
//     public string name;
//     private List<F> memory;
//     public GameObject body;
// }


// Re-Coded Being