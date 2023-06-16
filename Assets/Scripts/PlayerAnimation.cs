using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(Animator))] 
internal class PlayerAnimation : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    void Start ()
    {
        
    }
    void Update()
    {
        animator.GetComponent<Animator>().SetBool(IS_WALKING, playerController.IsMoving);
    }
}

