using System.Collections;
using UnityEngine;

public class Defender : Unit
{
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject sphere;

    private bool fieldActive = false;

    public void SetField()
    {
        void preAction()
        {
        }
        void postAction()
        {
        }

        StartCoroutine(Wrapper(
            preAction,
            () => AnimateField(),
            postAction
        ));
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
