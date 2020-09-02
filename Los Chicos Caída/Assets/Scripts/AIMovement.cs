using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIMovement : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public Material[] materials;

    private Rigidbody rb;

    public Animator myAnimator;
    private bool isMoving;

    //Jumping 
    public float jumpForce = 200f;
    public float jumpingProbability = 75;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Material material = materials[Random.Range(0, materials.Length)]; //This will be the AI material, random material
        ChangeChildrenMaterial(transform, material); //Changes all children materials
        agent.SetDestination(target.position);
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 0.1)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        myAnimator.SetBool("isMoving", isMoving);
        StartCoroutine(FindNewPath()); //Finds a new path every few seconds;
    }

    //Function that checks all children and grandchildren and changes material
    private void ChangeChildrenMaterial(Transform parent, Material material)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.material = material;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "JumpTrigger")
        {
            if(Random.Range(1, 101) < jumpingProbability)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                Debug.Log("Jump");
            }
        }
    }

    private IEnumerator FindNewPath()
    {
        yield return new WaitForSeconds(4);
        agent.SetDestination(target.position);
    }
}