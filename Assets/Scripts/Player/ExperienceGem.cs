using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [SerializeField] private int experienceAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            if(LevelManger.Instance != null)
            {
                LevelManger.Instance.AddExp(experienceAmount);
            }
            Destroy(gameObject);
        }
    }
}
