using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPowerup : MonoBehaviour
{
    [SerializeField] private Powerup[] _powerupPrefabs;
    public Powerup GetProduct(Vector3 position, int index)
    {
        if (index < 0 || index >= GetPowerupListLength())
        {
            throw new System.IndexOutOfRangeException();
        }
        Quaternion spawnQuaternion = Quaternion.identity;
        spawnQuaternion.eulerAngles = new Vector3(0, 0, 90);//由于创造prefab时胶囊形状默认为竖着的，所以要旋转90度
        Powerup instance = Instantiate(_powerupPrefabs[index], position, spawnQuaternion);
        instance.Initialize(_powerupPrefabs[index].name);
        return instance;
    }

    public int GetPowerupListLength()
    {
        return _powerupPrefabs.Length;
    }
}
