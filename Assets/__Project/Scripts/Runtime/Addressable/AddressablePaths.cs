using System;
using UnityEngine;

public static class AddressablePaths 
{
    #region # DataTable (constant_table)

    public enum DataTable
    {
        LocalizationText,
        BottomTab,
    }
    
    public static string ToAddress(this DataTable e) => e switch
    {
        DataTable.LocalizationText => "data_table/localization_text",
        DataTable.BottomTab => "data_table/bottom_tab",
        _ => ""
    };

    #endregion
    
    #region # Manager (manager)

    public enum Manager
    {
        Audio,
        Data,
        Input,
        UI
    }

    public static string ToAddress(this Manager e) => e switch
    {
        Manager.Audio => "manager/audio",
        Manager.Data  => "manager/data",
        Manager.Input => "manager/input",
        Manager.UI    => "manager/ui",
        _ => ""
    };

    #endregion

    #region # Scenes (scene)

    public enum Scene
    {
        Main
    }
    
    public static string ToAddress(this Scene e) => e switch
    {
        Scene.Main => "scene/main",
        _ => ""
    };

    #endregion
    
    #region # UIPage (ui_page)

    public enum UIPage
    {
        TicTacToe
    }
    
    public static string ToAddress(this UIPage e) => e switch
    {
        UIPage.TicTacToe => "ui_page/tic_tac_toe",
        _ => ""
    };

    #endregion
    
    #region # UIPage (ui_page)

    public enum UICell
    {
        TicTacToe
    }
    
    public static string ToAddress(this UICell e) => e switch
    {
        UICell.TicTacToe => "ui_cell/tic_tac_toe",
        _ => ""
    };

    #endregion
}
