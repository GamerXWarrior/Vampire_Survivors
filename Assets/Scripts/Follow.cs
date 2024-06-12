using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerTarget;
    [SerializeField]
    private Vector3 offset;
    
    // Update is called once per frame
    void Update()
    {
        // Making follow the object to the player transform
        transform.position = new Vector3(playerTarget.transform.position.x , playerTarget.transform.position.y) + offset;
    }
}
