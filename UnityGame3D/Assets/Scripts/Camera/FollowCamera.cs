using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset = new Vector3(0, 1, 0);

    // Update is called once per frame
    private void Update()
    {
        // Movement
        transform.position = new Vector3(_player.transform.position.x + _offset.x, _player.position.y + _offset.y, transform.position.z);
    }
}
