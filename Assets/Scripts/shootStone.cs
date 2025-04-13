using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class shootStone : MonoBehaviour
{
    [SerializeField] private GameObject stoneGameObject;
    [SerializeField] private float stoneSpeed;
    [SerializeField] private Transform spawnLocation;
    
    public void ShootStone()
    {
        //Debug.LogWarning(this.gameObject.transform.localScale.x);
        int direction = this.gameObject.transform.localScale.z > 0 ? 1 : -1;
        
        GameObject s = Instantiate(stoneGameObject, spawnLocation.position, Quaternion.identity);
        s.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * stoneSpeed, -2f);
        if (s != null) Destroy(s, 5f);
    }
}
