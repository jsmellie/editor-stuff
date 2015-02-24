using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(CurrencyFloatAttribute))]
public class CurrencyFloatDrawer : PropertyDrawer 
{
	const string DOLLAR_ICON = "$";
	const string EURO_ICON = "€";
	const string YEN_ICON = "¥";

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		CurrencyFloatAttribute curAttribute = attribute as CurrencyFloatAttribute;

		curAttribute.currencyType = CurrencyType.Yen;

		if(property.propertyType != SerializedPropertyType.Float)
		{
			return;
		}

		string icon = "";

		switch(curAttribute.currencyType)
		{
		case CurrencyType.Dollar:
			icon = DOLLAR_ICON;
			break;
		case CurrencyType.Euro:
			icon = EURO_ICON;
			break;
		case CurrencyType.Yen:
			icon = YEN_ICON;
			break;
		}

		property.floatValue = EditorGUI.FloatField(position, property.displayName + " (" + icon + ")", property.floatValue);

	}
}
