using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 currentPosition = player.transform.position;
            player.transform.position = new Vector3(currentPosition.x, currentPosition.y, GameManager.Instance.playerZPosition);
        }
    }
}
