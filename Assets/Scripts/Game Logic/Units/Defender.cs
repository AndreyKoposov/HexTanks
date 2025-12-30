using System.Collections;
using UnityEngine;

public class Defender : Unit
{
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject sphere;
    [SerializeField] private Animator animator;

    public void SetField(bool active)
    {
        animator.SetBool("FieldActive", active);
    }

    private IEnumerator AnimateField()
    {
        sphere.SetActive(true);
        yield return ExpandField(5);
    }

    protected IEnumerator ExpandField(float targetScale)
    {
        for (int i = 0; i < Frames; i++)
        {
            sphere.transform.localScale += targetScale / Frames * Vector3.one;
            yield return new WaitForSeconds(1f / Frames);
        }
    }
}
