using System;
using System.Collections.Generic;
using Colyseus;
using Colyseus.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private EnemyController _enemyController;
    
    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

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
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"speed", _playerCharacter.Speed}
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);
        
        _room.OnStateChange += OnChange;
        
        //подписываемся на сообщения от сервера по ключу
        //в <> указываем получаемый тип данных
        //второй аргумент - какой метод вызывается
        _room.OnMessage<string>("Shoot", ApplyShoot);
        _room.OnMessage<Dictionary<string, object>>("Crawl", ApplyCrawl);
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState)
        {
            state.players.ForEach((key, player) =>
            {
                if (key == _room.SessionId)
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
        var position = new Vector3(player.pX, player.pY, player.pZ);
        Instantiate(_playerCharacter, position, Quaternion.identity);
    }
    
    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        EnemyController newEnemyController = Instantiate(_enemyController, position, Quaternion.identity);
        newEnemyController.Initialize(player);
        
        _enemies.Add(key, newEnemyController);
    }

    private void RemoveEnemy(string key, Player value)
    {
        if(_enemies.ContainsKey(key) == false) return;
        
        EnemyController enemyController = _enemies[key];
        enemyController.Destroy();

        _enemies.Remove(key);
    }
    
    private void ApplyShoot(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);
        
        if (_enemies.ContainsKey(shootInfo.key) == false)
        {
            Debug.Log("Missing enemy try shoot");
            return;
        }
        
        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void ApplyCrawl(Dictionary<string, object> data)
    {
        if (_enemies.ContainsKey((string) data["key"]) == false)
        {
            Debug.Log("Missing enemy try shoot");
            return;
        }
        
        _enemies[(string) data["key"]].Crawl(Convert.ToSingle(data["sMY"]));
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }
    
    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }
    
    public string GetSessionID() { return _room.SessionId; } 
}
