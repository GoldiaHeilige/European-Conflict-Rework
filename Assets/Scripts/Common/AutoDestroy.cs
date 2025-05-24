using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 60f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}

// Thêm cái này sau khi Instantiate ( đặc biệt là ở DropSpawner.cs )

//  AutoDestroy ad = go.GetComponent<AutoDestroy>();
//  if (ad == null)
//  {
//     ad = go.AddComponent<AutoDestroy>();
//     ad.lifetime = 60f;    //  hoặc đặt theo config
//  }
