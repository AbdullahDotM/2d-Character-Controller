using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] ParticleSystem walking;



    private void LateUpdate()
    {
        walking.enableEmission = playerController.isGrounded;

    }
}
