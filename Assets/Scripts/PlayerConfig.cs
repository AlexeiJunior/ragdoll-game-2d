using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public GameObject head;
    public GameObject neck;
    public GameObject torso;
    public GameObject hip;
    public GameObject armLeft;
    public GameObject handLeft;
    public GameObject armRight;
    public GameObject handRight;
    public GameObject legLeft;
    public GameObject legRight;
    public GameObject groundCheckLeftLeg;
    public GameObject groundCheckRightLeg;

    public bool isGrounded = false;
    public bool playerGrabbing = false;
    public bool useLeftHand = false;
    private float legLeftStrenght = -1f;
    private float legRightStrenght = 1f;

    void FixedUpdate()
    {
        verifyGrounded();
        keepStandUp();
        destroyPlayer();
        jumpPlayer();
        moveLegs();
        moveTorso();
        moveHands();
        verifyUseLeftHand();
    }

    void keepStandUp()
    {
        torso.GetComponent<Rigidbody2D>().angularVelocity = 0;

        if (!isGrounded)
        {
            legLeft.GetComponent<Rigidbody2D>().angularVelocity = 0;
            legRight.GetComponent<Rigidbody2D>().angularVelocity = 0;
            return;
        }

        if (torso.GetComponent<Transform>().eulerAngles.z > 45 && torso.GetComponent<Transform>().eulerAngles.z < 315)
        {
            if (torso.GetComponent<Transform>().eulerAngles.z > 45)
            {
                torso.GetComponent<Rigidbody2D>().AddTorque(1400f);
            }
            if (torso.GetComponent<Transform>().eulerAngles.z < 315)
            {
                torso.GetComponent<Rigidbody2D>().AddTorque(-1400f);
            }
        }

        torso.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (-Physics.gravity.y * getPeso(false)));
        head.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (-Physics.gravity.y * head.GetComponent<Rigidbody2D>().mass));
    }

    void verifyGrounded()
    {
        bool isGroundedLeftLeg = Physics2D.OverlapCircle(groundCheckLeftLeg.transform.position, 0.15f, LayerMask.GetMask("Ground"));
        bool isGroundedRightLeg = Physics2D.OverlapCircle(groundCheckRightLeg.transform.position, 0.15f, LayerMask.GetMask("Ground"));
        isGrounded = isGroundedLeftLeg || isGroundedRightLeg;
    }

    void moveTorso()
    {
        if (!isGrounded) return;

        float moveForce = 30;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (torso.GetComponent<Rigidbody2D>().velocity.x < 0.5f)
            {
                torso.GetComponent<Rigidbody2D>().velocity = new Vector2(10, torso.GetComponent<Rigidbody2D>().velocity.y);
                hip.GetComponent<Rigidbody2D>().velocity = new Vector2(10, torso.GetComponent<Rigidbody2D>().velocity.y);
            }
            torso.GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
            hip.GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
            legLeft.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 10f);
            legRight.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 10f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (torso.GetComponent<Rigidbody2D>().velocity.x > -0.5f)
            {
                torso.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, torso.GetComponent<Rigidbody2D>().velocity.y);
                hip.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, torso.GetComponent<Rigidbody2D>().velocity.y);
            }
            torso.GetComponent<Rigidbody2D>().AddForce(Vector2.left * moveForce);
            hip.GetComponent<Rigidbody2D>().AddForce(Vector2.left * moveForce);
            legLeft.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 10f);
            legRight.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 10f);
        }
        else
        {
            torso.GetComponent<Rigidbody2D>().velocity = new Vector2(0, torso.GetComponent<Rigidbody2D>().velocity.y);
            hip.GetComponent<Rigidbody2D>().velocity = new Vector2(0, torso.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    void moveLegs()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            float angleHip = hip.transform.eulerAngles.z;
            float angle = (legLeft.transform.eulerAngles.z + angleHip) % 360;
            float moveLegStrenght = 600f;
            float maxLegAngle = (210f + angleHip) % 360;
            float minLegAngle = (40f + angleHip) % 360;

            Debug.Log(angle + " " + angleHip + " " + maxLegAngle + " " + minLegAngle);
            legLeft.GetComponent<Rigidbody2D>().AddTorque(legLeftStrenght * moveLegStrenght);
            legRight.GetComponent<Rigidbody2D>().AddTorque(legRightStrenght * moveLegStrenght);

            if (angle >= maxLegAngle)
            {
                legLeftStrenght = 1;
                legRightStrenght = -1;
            }
            if (angle <= minLegAngle)
            {
                legLeftStrenght = -1;
                legRightStrenght = 1;
            }
        }
    }

    void jumpPlayer()
    {
        if (!isGrounded || !Input.GetKey(KeyCode.Space)) return;

        float jumpForce = 25;
        
        torso.GetComponent<Rigidbody2D>().velocity = new Vector2(torso.GetComponent<Rigidbody2D>().velocity.x, 0);
        torso.GetComponent<Rigidbody2D>().AddForce(transform.up * (-Physics.gravity.y * jumpForce), ForceMode2D.Impulse);
    }

    void destroyPlayer()
    {
        if(!Input.GetKey(KeyCode.T)) return;

        Destroy(head.GetComponent<HingeJoint2D>());
        Destroy(torso.GetComponent<HingeJoint2D>());
        Destroy(armLeft.GetComponent<HingeJoint2D>());
        Destroy(handLeft.GetComponent<HingeJoint2D>());
        Destroy(armRight.GetComponent<HingeJoint2D>());
        Destroy(handRight.GetComponent<HingeJoint2D>());
        Destroy(legLeft.GetComponent<HingeJoint2D>());
        Destroy(legRight.GetComponent<HingeJoint2D>());
        Destroy(this);
    }
    
    void moveHands()
    {
        GameObject tempGameObject = useLeftHand ? armLeft : armRight;
        
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDir = mousePos - tempGameObject.GetComponent<Transform>().position;

        mousePos.z = 0.0f;
        mouseDir = mouseDir.normalized;
        float moveHandToPointStrenght = 200f;
        tempGameObject.GetComponent<Rigidbody2D>().AddForce(mouseDir * moveHandToPointStrenght);
        neck.GetComponent<Rigidbody2D>().AddForce(mouseDir * moveHandToPointStrenght * -1);
        tempGameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
    }

    void verifyUseLeftHand()
    {
        if (playerGrabbing) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) useLeftHand = true;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) useLeftHand = false;
    }

    float getPeso(bool useHead)
    {
        float peso = 0;
        peso += torso.GetComponent<Rigidbody2D>().mass;
        peso += neck.GetComponent<Rigidbody2D>().mass;
        peso += hip.GetComponent<Rigidbody2D>().mass;
        peso += armLeft.GetComponent<Rigidbody2D>().mass;
        peso += handLeft.GetComponent<Rigidbody2D>().mass;
        peso += armRight.GetComponent<Rigidbody2D>().mass;
        peso += handRight.GetComponent<Rigidbody2D>().mass;
        peso += legLeft.GetComponent<Rigidbody2D>().mass;
        peso += legRight.GetComponent<Rigidbody2D>().mass;

        if (useHead)
        {
            peso += head.GetComponent<Rigidbody2D>().mass;
        }

        return peso;
    }
}
