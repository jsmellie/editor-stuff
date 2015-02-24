using UnityEngine;
using System;

public enum CurrencyType
{
	Dollar = 0,
	Euro,
	Yen,
	UNKNOWN
}

[AttributeUsage( AttributeTargets.Field ) ] 
public class CurrencyFloatAttribute : PropertyAttribute 
{
	public CurrencyType currencyType = CurrencyType.UNKNOWN;

	public CurrencyFloatAttribute(CurrencyType currencyType)
	{
		this.currencyType = currencyType;
	}
}
