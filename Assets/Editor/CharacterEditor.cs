using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using RPG.Stats;
using RPG.Base;
using RPG.Items;

namespace RPG.AI
{
    [CustomEditor(typeof(CharacterVisual))]
    [CanEditMultipleObjects]
    public class CharacterVisualEditor : Editor 
    {

        void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            
            DrawDefaultInspector();
            serializedObject.Update();
            CharacterVisual character = (CharacterVisual) target;
            GUILayout.Label("Name: " + character.gameObject.name);
            Character charData = character.gameObject.GetComponent<Character>();

            GUILayout.Label("Points: ");
            foreach (KeyValuePair<CharacterPointTypes, int> points in charData.characterPoints)
            {
                    GUILayout.Label(points.Key + " : " + points.Value);
            }

            GUILayout.Label("Stats: ");
            foreach (KeyValuePair<StatName, float> points in charData.characterBase)
            {
                GUILayout.Label(points.Key + " : " + points.Value);
            }

            GUILayout.Label("Modifiers: ");
            foreach (KeyValuePair<StatName, float> points in charData.characterModAdd)
            {
                GUILayout.Label(points.Key + " : " + points.Value);
            }

            GUILayout.Label("Inventory: ");
            //foreach (ItemConfig item in charData.inventory)
            //{
            //    GUILayout.Label(points.Key + " : " + points.Value);
            //}

            GUILayout.Label("Equipment: ");
            foreach (KeyValuePair<EquipmentSlots, ItemConfig> points in charData.equipment.equipped)
            {
                GUILayout.Label(points.Key + " : " + points.Value);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
