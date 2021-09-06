
using UnityEngine;
using System.Collections;

//This is a basic interface with a single required
//method.
public interface IInteractable
{
    void InteractAction();
}

public interface IGrabble
{
    Transform Grab();
}

public interface IDroppable
{
    Transform Drop();
}






