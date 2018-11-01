using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor {

    Player player;

    public override void OnInspectorGUI()
    {
        player = (Player)target;

        DrawPlayerStats();
        EditorGUILayout.Separator();
        DrawPlayerInventory();
        EditorGUILayout.Separator();
        DrawPlayerPhysics();
        EditorGUILayout.Separator();
        DrawPlayerEffects();
    }

    private void DrawPlayerStats()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);

        player.maxHealth = EditorGUILayout.FloatField("Max Health", player.maxHealth);
        if (!EditorApplication.isPlaying)
            player.health = player.maxHealth;
        EditorGUILayout.LabelField("Health", player.health.ToString());

        player.maxMana = EditorGUILayout.FloatField("Max Mana", player.maxMana);
        if (!EditorApplication.isPlaying)
            player.mana = player.maxMana;
        EditorGUILayout.LabelField("Mana", player.mana.ToString());

        player.manaRegen = EditorGUILayout.FloatField("Mana Regen", player.manaRegen);

        EditorGUILayout.EndVertical();
    }

    private void DrawPlayerPhysics()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Physics", EditorStyles.boldLabel);

        player.maxSpeed = EditorGUILayout.IntField("Max Speed", player.maxSpeed);
        player.acceleration = EditorGUILayout.FloatField("Acceleration", player.acceleration);
        player.friction = EditorGUILayout.FloatField("Friction", player.friction);
        EditorGUILayout.LabelField("Can move", player.canMove.ToString());

        EditorGUILayout.EndVertical();
    }

    private void DrawPlayerInventory()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Inventory", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Current Weapon", player.currentWeapon != null ? player.currentWeapon.name : "None");

        EditorGUILayout.EndVertical();
    }

    private void DrawPlayerEffects()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

        player.DamageEffect = (GameObject) EditorGUILayout.ObjectField("Damaged Effect", player.DamageEffect, typeof(GameObject), false);
        player.damageShakeAmount = EditorGUILayout.FloatField("Damaged Shake Amount", player.damageShakeAmount);

        EditorGUILayout.EndVertical();
    }
}
