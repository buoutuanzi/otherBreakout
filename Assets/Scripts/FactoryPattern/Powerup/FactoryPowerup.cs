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
        spawnQuaternion.eulerAngles = new Vector3(0, 0, 90);//���ڴ���prefabʱ������״Ĭ��Ϊ���ŵģ�����Ҫ��ת90��
        Powerup instance = Instantiate(_powerupPrefabs[index], position, spawnQuaternion);
        instance.Initialize(_powerupPrefabs[index].name);
        return instance;
    }

    public int GetPowerupListLength()
    {
        return _powerupPrefabs.Length;
    }
}
