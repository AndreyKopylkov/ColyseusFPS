using Colyseus;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;
    
    private ColyseusRoom<State> _room;

    protected override void Awake()
    {
        base.Awake();
        
        Instance.InitializeClient();
        Connect();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_room != null)
        {
            _room.Leave();
        }
        else
        {
            Debug.Log("Выход был произведён до подключения к комнате");
        }
    }
    
    private async void Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>("state_handler");
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState)
        {
            state.players.ForEach((key, player) =>
            {
                if (key == _room.SerializerId)
                    CreatePlayer(player);
                else
                    CreateEnemy(key, player);
            });

            _room.State.players.OnAdd += CreateEnemy;
            _room.State.players.OnRemove += RemoveEnemy;
        }
        else
        {
            
        }
    }

    private void CreatePlayer(Player player)
    {
        var position = new Vector3(player.x, 0, player.y);
        Instantiate(_player, position, Quaternion.identity);
    }

    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.x, 0, player.y);
        Instantiate(_enemy, position, Quaternion.identity);
    }

    private void RemoveEnemy(string key, Player value)
    {
        
    }
}
