﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7335892CB9B0473A2F28315C94C8113B75300E99AF5ABA43E1A635056975246F"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Syncfusion;
using Syncfusion.Windows;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using paint;


namespace paint {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 55 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel toolParams;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel tools;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock startActiveTool;
        
        #line default
        #line hidden
        
        
        #line 222 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.Windows.Tools.Controls.ColorPickerPalette colorPickerPalette;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel workSpace;
        
        #line default
        #line hidden
        
        
        #line 236 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewbox zoom;
        
        #line default
        #line hidden
        
        
        #line 237 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvas;
        
        #line default
        #line hidden
        
        
        #line 240 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ScaleTransform st;
        
        #line default
        #line hidden
        
        
        #line 241 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.TranslateTransform translate;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/paint;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\MainWindow.xaml"
            ((paint.MainWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.HandSwitchHander);
            
            #line default
            #line hidden
            
            #line 9 "..\..\MainWindow.xaml"
            ((paint.MainWindow)(target)).KeyUp += new System.Windows.Input.KeyEventHandler(this.GlobalHotKeyHandler);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 21 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.openMALHandler);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 22 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.createMALHandler);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 28 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenCanvasSizeDialogHandler);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 31 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateLayerHandler);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 37 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.SelectAllHandler);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenBlurDialog);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 49 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenLayersDialogHandler);
            
            #line default
            #line hidden
            return;
            case 9:
            this.toolParams = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 10:
            this.tools = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 11:
            this.startActiveTool = ((System.Windows.Controls.TextBlock)(target));
            
            #line 68 "..\..\MainWindow.xaml"
            this.startActiveTool.MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 68 "..\..\MainWindow.xaml"
            this.startActiveTool.MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 68 "..\..\MainWindow.xaml"
            this.startActiveTool.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 75 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 75 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 75 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 82 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 82 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 82 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 89 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 89 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 89 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 96 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 96 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 96 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 103 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 103 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 103 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 110 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 110 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 110 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 117 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 117 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 117 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 124 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 124 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 124 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 131 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 131 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 131 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 138 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 138 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 138 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 145 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 145 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 145 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 23:
            
            #line 152 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 152 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 152 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 24:
            
            #line 159 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 159 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 159 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 25:
            
            #line 166 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 166 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 166 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 26:
            
            #line 173 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 173 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 173 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 27:
            
            #line 180 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 180 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 180 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 28:
            
            #line 187 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 187 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 187 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 29:
            
            #line 194 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 194 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 194 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 30:
            
            #line 201 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 201 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 201 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 31:
            
            #line 208 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 208 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 208 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 32:
            
            #line 215 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 215 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 215 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 33:
            this.colorPickerPalette = ((Syncfusion.Windows.Tools.Controls.ColorPickerPalette)(target));
            
            #line 223 "..\..\MainWindow.xaml"
            this.colorPickerPalette.SelectedBrushChanged += new System.EventHandler<Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs>(this.ColorOfPageHandler);
            
            #line default
            #line hidden
            return;
            case 34:
            
            #line 226 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.HoverToolHandler);
            
            #line default
            #line hidden
            
            #line 226 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.HoutToolHandler);
            
            #line default
            #line hidden
            
            #line 226 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SetActiveTool);
            
            #line default
            #line hidden
            return;
            case 35:
            this.workSpace = ((System.Windows.Controls.StackPanel)(target));
            
            #line 235 "..\..\MainWindow.xaml"
            this.workSpace.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.SetZoomHandler);
            
            #line default
            #line hidden
            return;
            case 36:
            this.zoom = ((System.Windows.Controls.Viewbox)(target));
            return;
            case 37:
            this.canvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 237 "..\..\MainWindow.xaml"
            this.canvas.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.BrushDownHandler);
            
            #line default
            #line hidden
            
            #line 237 "..\..\MainWindow.xaml"
            this.canvas.MouseMove += new System.Windows.Input.MouseEventHandler(this.BrushMoveHandler);
            
            #line default
            #line hidden
            
            #line 237 "..\..\MainWindow.xaml"
            this.canvas.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.BrushUpHandler);
            
            #line default
            #line hidden
            return;
            case 38:
            this.st = ((System.Windows.Media.ScaleTransform)(target));
            return;
            case 39:
            this.translate = ((System.Windows.Media.TranslateTransform)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

