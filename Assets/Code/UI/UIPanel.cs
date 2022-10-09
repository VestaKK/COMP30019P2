using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public abstract void Initialise();

    public virtual void Hide() => gameObject.SetActive(false);

    public virtual void Show() => gameObject.SetActive(true);
}
