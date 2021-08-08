using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour, IPlayerPointerHandler
{
    private ShellController shellController;


    private void Start()
    {
        if (!GetComponentInParent<ShellController>())
        {
            throw new System.Exception("Parent is not a shell controller!");
        }

        shellController = GetComponentInParent<ShellController>();
    }

    private void Update()
    {
        
    }

    public void Click(PointerController pointer)
    {
        shellController.Pressed(this, pointer);
    }


}
