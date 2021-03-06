﻿#pragma checksum "C:\Users\edkaz\Documents\Visual Studio 2017\Projects\MentorSystemWebRTC\MentorSystemWebRTC\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3E575E143AD916FBFF615B374C52267D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MentorSystemWebRTC
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
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Grid element1 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 10 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element1).PointerExited += this.FingerLeft;
                    #line default
                }
                break;
            case 2:
                {
                    this.BackgroundImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 3:
                {
                    this.mediaPlayerElement = (global::Windows.UI.Xaml.Controls.MediaPlayerElement)(target);
                    #line 14 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaPlayerElement)this.mediaPlayerElement).Tapped += this.imagesPanelTapped;
                    #line 14 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaPlayerElement)this.mediaPlayerElement).PointerMoved += this.LineDrawing;
                    #line 14 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaPlayerElement)this.mediaPlayerElement).ManipulationDelta += this.ZoomBackgroundImage;
                    #line 14 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaPlayerElement)this.mediaPlayerElement).PointerPressed += this.LineStarting;
                    #line default
                }
                break;
            case 4:
                {
                    this.imagesPanel = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 5:
                {
                    this.contentPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 6:
                {
                    this.drawingPanel = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 7:
                {
                    this.PivotPanel = (global::Windows.UI.Xaml.Controls.Pivot)(target);
                }
                break;
            case 8:
                {
                    this.TrashBinOpenImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 9:
                {
                    this.TrashBinClosedImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 104 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.TrashBinClosedImage).PointerEntered += this.PointerOverTrashBin;
                    #line 104 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.TrashBinClosedImage).PointerExited += this.PointerLeftTrashBin;
                    #line default
                }
                break;
            case 10:
                {
                    this.buttonLinesBorder = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 11:
                {
                    this.buttonPointsBorder = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 12:
                {
                    this.ConnectionStatus = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.StatusButton = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 14:
                {
                    this.buttonSendAllBorder = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 15:
                {
                    this.buttonSendAll = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 122 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.buttonSendAll).Click += this.SendAllButtonClicked;
                    #line default
                }
                break;
            case 16:
                {
                    this.buttonExit = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 115 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.buttonExit).Click += this.ExitButtonClicked;
                    #line default
                }
                break;
            case 17:
                {
                    this.buttonEraseAll = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 112 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.buttonEraseAll).Click += this.EraseAllButtonClicked;
                    #line default
                }
                break;
            case 18:
                {
                    this.buttonPoints = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    #line 109 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonPoints).Checked += this.PointsButtonChecked;
                    #line 109 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonPoints).Unchecked += this.PointsButtonUnchecked;
                    #line default
                }
                break;
            case 19:
                {
                    this.buttonLines = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    #line 106 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonLines).Checked += this.LinesButtonChecked;
                    #line 106 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.buttonLines).Unchecked += this.LinesButtonUnchecked;
                    #line default
                }
                break;
            case 20:
                {
                    this.TextsPanel = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 21:
                {
                    this.iconPanelImage20 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 98 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage20).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 22:
                {
                    this.iconPanelImage19 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 95 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage19).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 23:
                {
                    this.iconPanelImage18 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 92 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage18).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 24:
                {
                    this.iconPanelImage17 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 89 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage17).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 25:
                {
                    this.iconPanelImage16 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 86 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage16).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 26:
                {
                    this.HandsPanel = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 27:
                {
                    this.iconPanelImage15 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 79 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage15).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 28:
                {
                    this.iconPanelImage14 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 76 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage14).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 29:
                {
                    this.iconPanelImage13 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 73 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage13).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 30:
                {
                    this.ToolsPanel = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 31:
                {
                    this.iconPanelImage12 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 66 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage12).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 32:
                {
                    this.iconPanelImage11 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 63 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage11).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 33:
                {
                    this.iconPanelImage10 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 60 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage10).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 34:
                {
                    this.iconPanelImage9 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 57 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage9).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 35:
                {
                    this.iconPanelImage8 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 54 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage8).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 36:
                {
                    this.iconPanelImage7 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 51 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage7).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 37:
                {
                    this.iconPanelImage6 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 48 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage6).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 38:
                {
                    this.iconPanelImage5 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 45 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage5).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 39:
                {
                    this.iconPanelImage4 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 42 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage4).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 40:
                {
                    this.iconPanelImage3 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 39 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage3).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 41:
                {
                    this.iconPanelImage2 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 36 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage2).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 42:
                {
                    this.iconPanelImage1 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 33 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.iconPanelImage1).Tapped += this.iconPanelImageTapped;
                    #line default
                }
                break;
            case 43:
                {
                    this.greetingOutput = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

