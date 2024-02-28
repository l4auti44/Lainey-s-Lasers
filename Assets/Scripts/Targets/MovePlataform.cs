using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlataform : Target
{
    [SerializeField] private GameObject plataform;
    private MoveAtoB plataformScript;

    private void Start()
    {
        plataformScript = plataform.GetComponent<MoveAtoB>();
        plataformScript.enabled = false;
    }

    override
    public void DoAction()
    {
        plataformScript.enabled = true;
    }

    override
    public void Undo()
    {
        plataformScript.enabled = false;
    }
}
