using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class KC_CharacterInput : MonoBehaviour
{
    private CharacterMotor motor;

    public KeyCode jumpKey = KeyCode.Space;

    private Vector3 moveDirection;
    private Vector3 rotationDirection;
    private bool jump = false;

    private void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    private void Update()
    {
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");

        jump = Input.GetKey(jumpKey);

        motor.MoveCharacter(moveDirection, jump);

        rotationDirection.x = -Input.GetAxis("Mouse Y");
        rotationDirection.y = Input.GetAxis("Mouse X");

        motor.RotateCharacter(rotationDirection);
    }
}
