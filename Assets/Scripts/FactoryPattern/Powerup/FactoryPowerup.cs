using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryPowerup : Factory
{
    abstract protected Powerup _powerupPrefab { get; set; }

    public override IProduct GetProduct(Vector3 position)
    {
        if (_powerupPrefab == null)
        {
            throw new System.NotImplementedException();
        }
        Quaternion spawnQuaternion = Quaternion.identity;
        spawnQuaternion.eulerAngles = new Vector3(0, 0, 90);//由于创造prefab时胶囊形状默认为竖着的，所以要旋转90度
        GameObject instance = Instantiate(_powerupPrefab.gameObject, position, spawnQuaternion);
        Powerup newProduct = instance.GetComponent<Powerup>();
        newProduct.Initialize();
        return newProduct;
    }
}
