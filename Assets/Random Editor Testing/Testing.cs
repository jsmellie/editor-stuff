using UnityEngine;
using System.Collections;

public class Testing : MonoBehaviour {

	[CurrencyFloatAttribute(CurrencyType.Euro)]
	public float testCurrency = 0.99f;
}
