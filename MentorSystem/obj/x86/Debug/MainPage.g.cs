﻿#pragma checksum "C:\Users\M2M ADMIN\documents\visual studio 2017\Projects\MentorSystem\MentorSystem\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F4181DAF458ADC32C1956224693E9DBB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MentorSystem
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // MainPage.xaml line 11
                {
                    this.BackgroundImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.BackgroundImage).Tapped += this.imagesPanelTapped;
                    ((global::Windows.UI.Xaml.Controls.Image)this.BackgroundImage).PointerMoved += this.LineDrawing;
                    ((global::Windows.UI.Xaml.Controls.Image)this.BackgroundImage).PointerExited += this.LineStopped;
                }
                break;
            case 2: // MainPage.xaml line 13
                {
                    this.imagesPanel = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 3: // MainPage.xaml line 15
                {
                    this.contentPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 4: // MainPage.xaml line 25
                {
                    this.drawingPanel = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 5: // MainPage.xaml line 26
                {
                    this.PivotPanel = (global::Windows.UI.Xaml.Controls.Pivot)(target);
                }
                break;
            case 6: // MainPage.xaml line 100
                {
                    this.TrashBinOpenImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 7: // MainPage.xaml line 101
                {
                    this.TrashBinClosedImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.TrashBinClosedImage).PointerEntered += this.PointerOverTrashBin;
                    ((global::Windows.UI.Xaml.Controls.Image)this.TrashBinClosedImage).PointerExited += this.PointerLeftTrashBin;
                }
                break;
            case 8: // MainPage.xaml line 102
                {
                    this.buttonLinesBorder = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 9: // MainPage.xaml line 105
                {
                    this.buttonPointsBorder = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 10: // MainPage.xaml line 112
                {
                    this.buttonExit = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.buttonExit).Click += this.ExitButtonClicked;
                }
                break;
            case 11: // MainPage.xaml line 109
                {
                    this.buttonEraseAll = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.buttonEraseAll).Click += this.EraseAllButtonClicked;
                }
                break;
            case 12: // MainPage.xaml line 106
                {
                    this.buttonPoints = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonPoints).Checked += this.PointsButtonChecked;
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonPoints).Unchecked += this.PointsButtonUnchecked;
                }
                break;
            case 13: // MainPage.xaml line 103
                {
                    this.buttonLines = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonLines).Checked += this.LinesButtonChecked;
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonLines).Unchecked += this.LinesButtonUnchecked;
                }
                break;
            case 14: // MainPage.xaml line 81
                {
                    this.TextsPanel = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 15: // MainPage.xaml line 95
                {
                    this.iconPanelImage20 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage20).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 16: // MainPage.xaml line 92
                {
                    this.iconPanelImage19 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage19).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 17: // MainPage.xaml line 89
                {
                    this.iconPanelImage18 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage18).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 18: // MainPage.xaml line 86
                {
                    this.iconPanelImage17 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage17).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 19: // MainPage.xaml line 83
                {
                    this.iconPanelImage16 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage16).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 20: // MainPage.xaml line 68
                {
                    this.HandsPanel = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 21: // MainPage.xaml line 76
                {
                    this.iconPanelImage15 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage15).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 22: // MainPage.xaml line 73
                {
                    this.iconPanelImage14 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage14).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 23: // MainPage.xaml line 70
                {
                    this.iconPanelImage13 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage13).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 24: // MainPage.xaml line 28
                {
                    this.ToolsPanel = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 25: // MainPage.xaml line 63
                {
                    this.iconPanelImage12 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage12).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 26: // MainPage.xaml line 60
                {
                    this.iconPanelImage11 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage11).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 27: // MainPage.xaml line 57
                {
                    this.iconPanelImage10 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage10).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 28: // MainPage.xaml line 54
                {
                    this.iconPanelImage9 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage9).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 29: // MainPage.xaml line 51
                {
                    this.iconPanelImage8 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage8).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 30: // MainPage.xaml line 48
                {
                    this.iconPanelImage7 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage7).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 31: // MainPage.xaml line 45
                {
                    this.iconPanelImage6 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage6).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 32: // MainPage.xaml line 42
                {
                    this.iconPanelImage5 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage5).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 33: // MainPage.xaml line 39
                {
                    this.iconPanelImage4 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage4).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 34: // MainPage.xaml line 36
                {
                    this.iconPanelImage3 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage3).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 35: // MainPage.xaml line 33
                {
                    this.iconPanelImage2 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage2).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 36: // MainPage.xaml line 30
                {
                    this.iconPanelImage1 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage1).Tapped += this.iconPanelImageTapped;
                }
                break;
            case 37: // MainPage.xaml line 22
                {
                    this.greetingOutput = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

