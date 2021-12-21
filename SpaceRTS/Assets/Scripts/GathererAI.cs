using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererAI : MonoBehaviour
{
    [SerializeField] private Transform MineralNodeTransform;
    [SerializeField] private Transform storageTransform;
    // Start is called before the first frame update

    private IUnit unit;

    private void Awake()
    {
        unit = gameObject.GetComponent<IUnit>();

        unit.MoveTo(MineralNodeTransform.position, 10f, () =>
        {
            unit.PlayAnimationMine(storageTransform.position, null);
        });
    }
}
