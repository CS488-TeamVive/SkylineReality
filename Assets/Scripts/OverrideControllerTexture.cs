using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Override the texture of the Vive controllers, with your own texture, after SteamVR has loaded and applied the original texture.
/// </summary>
public class OverrideControllerTexture : SteamVR_RenderModel
{
    public List<Texture2D> skinList;
    private MenuDisplayOption currentMenu = MenuDisplayOption.Default;

    private enum MenuDisplayOption { Default = 0, Calendar_Selected = 1, Sun_Selected = 2, Building_Selected = 3 };

    protected new void Update()
    {
        base.Update();
        //Subscribe to the event that is called by SteamVR_RenderModel, when the controller mesh + texture, has been loaded completely.
        SteamVR_Utils.Event.Listen("render_model_loaded", OnControllerLoaded);
    }

    void OnEnable()
    {
        LeftMenuController.OnMenuSelection += UpdateUI;
    }

    void OnDisable()
    {
        LeftMenuController.OnMenuSelection -= UpdateUI;
    }

    /// <summary>
    /// Override the texture of each of the parts, with your texture.
    /// </summary>
    /// <param name="newTexture">Override texture</param>
    /// <param name="modelTransform">Transform of the gameobject, which has the SteamVR_RenderModel component.</param>
    public void UpdateControllerTexture(Texture2D newTexture, Transform modelTransform)
    {
        modelTransform.Find("body").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("button").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("led").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("lgrip").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("rgrip").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("scroll_wheel").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("sys_button").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trackpad").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trackpad_scroll_cut").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trackpad_touch").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trigger").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
    }

    /// <summary>
    /// Call this method, when the "render_model_loaded".
    /// </summary>
    /// <param name="args">bool success, SteamVR_RenderModel model</param>
    void OnControllerLoaded(params object[] args)
    {
        if (args[0].Equals(this))
        {
            UpdateControllerTexture(skinList.ElementAt((int)MenuDisplayOption.Default), this.gameObject.transform);
        }
    }

    private void UpdateController()
    {
        UpdateControllerTexture(skinList.ElementAt((int)currentMenu), this.gameObject.transform);
    }

    private void UpdateUI(LeftMenuController.MenuOption menuSelection)
    {
        UpdateMenu(menuSelection);
        UpdateController();
    }

    private void UpdateMenu(LeftMenuController.MenuOption menuSelection)
    {
        try
        {
            MenuDisplayOption selection = (MenuDisplayOption)Enum.Parse(typeof(MenuDisplayOption), menuSelection.ToString());

            if(currentMenu == selection)
            {
                currentMenu = MenuDisplayOption.Default;
            }
            else
            {
                currentMenu = selection;
            }
        }
        catch (ArgumentException)
        {
            return;
        } 
    }
}