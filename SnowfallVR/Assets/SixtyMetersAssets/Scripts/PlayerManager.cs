using SixtyMetersAssets.Characters.Player;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerBehaviour[] _players;

    private float _nextCheck;

    // Start is called before the first frame update
    void Start()
    {
        _players = FindObjectsOfType<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _nextCheck)
        {
            _players = FindObjectsOfType<PlayerBehaviour>();
            NextCheckInSeconds(3);
        }
    }

    public PlayerBehaviour[] GetAllPlayers()
    {
        return _players;
    }

    public PlayerBehaviour GetClosestPlayer(Transform input)
    {
        float shortestDistance = 1000;
        PlayerBehaviour playerWithShortestDistance = null;
        foreach (var player in _players)
        {
            var distanceToPlayer = Vector3.Distance(player.transform.position, input.position);
            if (playerWithShortestDistance == null || shortestDistance > distanceToPlayer)
            {
                shortestDistance = distanceToPlayer;
                playerWithShortestDistance = player;
            }
        }
        
        return playerWithShortestDistance;
    }

    private void NextCheckInSeconds(float seconds)
    {
        _nextCheck = Time.time + seconds;
    }
}