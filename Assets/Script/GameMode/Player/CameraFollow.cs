using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject MainCharacter;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float height;
    [SerializeField] private float rearDistance;
    private Vector3 cameraOffset;
    private Vector3 currentVector;

    private void Start()
    {
        transform.position = new Vector3(MainCharacter.transform.position.x,
                                         MainCharacter.transform.position.y + height,
                                         MainCharacter.transform.position.z - rearDistance);
        transform.rotation = Quaternion.LookRotation(MainCharacter.transform.position - transform.position);
    }
    private void SetOffset(Vector3 offset)
    {
        if (offset.z < 0) cameraOffset = offset * 10; //Down
        else if (offset.z > 0) cameraOffset = offset * 3; //Up
        else cameraOffset = offset * 7; //Right/Left
    }
    private void CameraMovement()
    {
        currentVector = new Vector3(MainCharacter.transform.position.x + cameraOffset.x,
                                    MainCharacter.transform.position.y + height,
                                    (MainCharacter.transform.position.z - rearDistance) + cameraOffset.z);
        transform.position = Vector3.Lerp(transform.position, currentVector, returnSpeed * Time.deltaTime);
    }
    private void Update()
    {
        CameraMovement();
    }
}
