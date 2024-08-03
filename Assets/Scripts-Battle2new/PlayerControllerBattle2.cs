using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerControllerBattle2 : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;

    public bool canMove = true;

    public GameObject Inst;

    public GameManagerBattle2 gameManager;

    public Animator anim;
    public Animator anim1;
    public Animator anim2;
    public Animator anim3;

    private void Start()
    {
        anim1 = GameObject.Find("Persian").GetComponent<Animator>();
        anim2 = GameObject.Find("Persian2").GetComponent<Animator>();
        anim3 = GameObject.Find("Kempe").GetComponent<Animator>();
        Inst.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Inst.gameObject.SetActive(false);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            anim2.SetBool("IsWalking", true);
            anim1.SetBool("IsWalking", true);
            anim3.SetBool("IsWalking", true);
        }
        else
        {
            anim2.SetBool("IsWalking", false);
            anim1.SetBool("IsWalking", false);
            anim3.SetBool("IsWalking", false);
        }

        if (transform.position.z >15)
        {
            gameManager.GameOver();
        }
    }
}