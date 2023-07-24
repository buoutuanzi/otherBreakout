using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
    public string productName { get; set; }
    public void Initialize();
}
