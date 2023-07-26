using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IProduct
{
    public int hitsToKill;
    private int _score;
    private int _numberOfHits;
    private GameObject _gameControl;

    private Factory _factory;

    [SerializeField] private string _productName = "Block";
    public string productName { get => _productName; set => _productName = value; }

    public void Initialize()
    {
        _gameControl = GameObject.FindGameObjectsWithTag("GameControl")[0];
        _numberOfHits = 0;
        _score = hitsToKill * 10;
        float chance = hitsToKill * 10; //���ݷ���Ѫ������������߼��ʣ��ֱ�Ϊ10%��20%��40%
        if (Random.Range(0, 100) < chance) //�˴�ΪӦ�ü���chance��100���������������С��chance���ʾ�����ɵ���
        {
            Factory[] factories = _gameControl.GetComponentsInChildren<FactoryPowerup>();
            _factory = factories[Random.Range(0, factories.Length)];
        }
    }

    private void GetHit(int damage)
    {
        _numberOfHits += damage;
        if (_numberOfHits >= hitsToKill)
        {
            _gameControl.SendMessage("AddPoints", _score);
            if (_factory != null) 
            {
                _factory.GetProduct(transform.position);
            }
            Destroy(this.gameObject);
            
        }
    }
}
