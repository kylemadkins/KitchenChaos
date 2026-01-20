using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Player player;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player.IsWalking()) _animator.SetBool("IsWalking", true);
        else _animator.SetBool("IsWalking", false);
    }
}
