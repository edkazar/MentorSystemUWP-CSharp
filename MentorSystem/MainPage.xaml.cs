using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using MentorSystem.MentorClasses;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MentorSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>  
    public sealed partial class MainPage : Page
    {
        static double PI = 3.14159265358979323846f;

        private ControlCenter myController;

        private CommunicationManager myCommunication;


        private SolidColorBrush redColor;
        private SolidColorBrush greenColor;
        private SolidColorBrush tealColor;
        private SolidColorBrush goldColor;

        private SolidColorBrush buttonCheckedColor;
        private SolidColorBrush buttonUncheckedColor;

        private Polyline LineAnnotation;

        /////////////////
        private StreamSocketListener tcpListener;
        private StreamSocket connectedSocket = null;
        private const string port = "8900";

        public MainPage()
        {
            this.InitializeComponent();
            myController = new ControlCenter();
            myCommunication = new CommunicationManager();

            redColor = new SolidColorBrush(Windows.UI.Colors.Red);
            greenColor = new SolidColorBrush(Windows.UI.Colors.Green);
            tealColor = new SolidColorBrush(Windows.UI.Colors.Teal);
            goldColor = new SolidColorBrush(Windows.UI.Colors.Gold);
            CreateButtonsColor();

            ResetLineAnnotation();

            ColoredRectangle.PointerEntered += EnteringRectangle;

            ColoredRectangle.Holding += HoldingRectangle;

            //////////////Threads//////////////
            //Task t = Task.Run( () => ThreadExample());
            ////////////////////////////////////
        }

        ///////////////////////////////////////////////////////////////////
        private async void OnConnected(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            if (connectedSocket != null)
            {
                connectedSocket.Dispose();
                connectedSocket = null;
            }
            connectedSocket = args.Socket;
            Debug.WriteLine("Client has connected");

            Stream streamIn = connectedSocket.InputStream.AsStreamForRead();
            int maxSize = 3 * 640 * 480;
            byte[] buffer = new byte[maxSize];
            //int offset = 0;
            //int bytesRead;
            //do
            //{
                await streamIn.ReadAsync(buffer, 0, maxSize);
                //offset += bytesRead;
            //} while (bytesRead > 0);
            Debug.WriteLine("Left loop");
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await randomAccessStream.WriteAsync(buffer.AsBuffer());
            randomAccessStream.Seek(0);
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                var bitmap = new BitmapImage();
                //bitmap.SetSource(memStream.AsRandomAccessStream());
                bitmap.SetSource(randomAccessStream);
                BackgroundImage.Source = bitmap;
            });
            /*var inputStream = await connectedSocket.InputStream.ReadAsync();


            DataReader reader = new DataReader(connectedSocket.InputStream);
            reader.InputStreamOptions = InputStreamOptions.Partial;
            await reader.LoadAsync(250);
            var dataString = reader.ReadString(reader.UnconsumedBufferLength);
            greetingOutput.Text = "Stream: " + dataString;*/


            //Stream streamIn = connectedSocket.InputStream.AsStreamForRead();
            //IBuffer result = new Windows.Storage.Streams.Buffer(921600);
            //await connectedSocket.InputStream.ReadAsync(result, result.Capacity, InputStreamOptions.Partial);

            ///
            /// Falta leer async el stream
            ///
            //var memStream = new MemoryStream();
            //await streamIn.CopyToAsync(memStream);
            //memStream.Position = 0;
            /*int maxSize = 3 * 640 * 480;
            byte[] buffer = new byte[maxSize];
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                //byte[] buffer = new byte[3 * 640 * 480];
                using (MemoryStream ms = new MemoryStream())
                {

                    int read;
                    while ((read = streamIn.Read(buffer, 0, maxSize)) > 0)
                    {
                        // Debug.WriteLine("Chunchito {0}", read);
                        ms.Write(buffer, 0, read);
                    }
                    Debug.WriteLine("Left loop");
                    //return ms.ToArray();
                    var myBitmap = new BitmapImage();
                    myBitmap.SetSource(ms.AsRandomAccessStream());
                    //bitmap.SetSource(result.AsStream().AsRandomAccessStream());
                    BackgroundImage.Source = myBitmap;
                }
                /*var bitmap = new BitmapImage();
                //bitmap.SetSource(memStream.AsRandomAccessStream());
                bitmap.SetSource(result.AsStream().AsRandomAccessStream());
                BackgroundImage.Source = bitmap;
            });*/
            /*
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                var bitmap = new BitmapImage();
                //bitmap.SetSource(memStream.AsRandomAccessStream());
                bitmap.SetSource(result.AsStream().AsRandomAccessStream());
                BackgroundImage.Source = bitmap;
            });*/



            /*IRandomAccessStream streamForImage = new InMemoryRandomAccessStream();
            using (var inputStream = connectedSocket.InputStream)
            {
                await RandomAccessStream.CopyAsync(inputStream, streamForImage);
                Debug.WriteLine("Copying stream");
            }
            streamForImage.Seek(0);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                var bitmap = new BitmapImage();
                bitmap.SetSource(streamForImage);
                BackgroundImage.Source = bitmap;
                Debug.WriteLine("Image generated");
            });*/
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("Waiting for client to connect...");
            tcpListener = new StreamSocketListener();
            tcpListener.ConnectionReceived += OnConnected;
            await tcpListener.BindEndpointAsync(null, port);
        }

        ///////////////////////////////////////////////////////////////////


        private void ThreadExample()
        {
            int ctr = 0;
            for (ctr = 0; ctr <= 1000000; ctr++)
            {
                Debug.WriteLine("Doing iteration {0}", ctr);
            }
            Debug.WriteLine("Finished {0} loop iterations",ctr);
        }

        private void imagesPanelTapped(object sender, TappedRoutedEventArgs e)
        {
            ColoredRectangle.Fill = tealColor;

            if (myController.GetIconAnnotationSelectedFlag())
            {
                CreateIconAnnotation(e);

                myController.SetIconAnnotationSelectedFlag(false);
                myController.SetSelectedIconPath("");
            }
            else if (buttonPoints.IsChecked.Value)
            {
                PreparePointAnnotation(e);
                drawingPanel.Children.Add(LineAnnotation);             

                ResetLineAnnotation();
            }

            /*if (!myCommunication.Except)
            {
                if (myCommunication.Connected)
                {
                    ColoredRectangle.Fill = buttonCheckedColor;
                }
                else if (!myCommunication.Connected)
                {
                    ColoredRectangle.Fill = buttonUncheckedColor;
                }
            }
            else
            {
                ColoredRectangle.Fill = greenColor;
            }*/
        }

        private void LineDrawing(object sender, PointerRoutedEventArgs e)
        {
            if (buttonLines.IsChecked.Value)
            {
                LineAnnotation.Points.Add(new Point(e.GetCurrentPoint(imagesPanel).Position.X, e.GetCurrentPoint(imagesPanel).Position.Y));
            }
        }

        private void LineStopped(object sender, PointerRoutedEventArgs e)
        {
            if (buttonLines.IsChecked.Value)
            {
                drawingPanel.Children.Add(LineAnnotation);

                ResetLineAnnotation();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            greetingOutput.Text = "Hello, " + nameInput.Text + "!";
            ColoredRectangle.Fill = redColor;
        }

        private void Rectangle_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ColoredRectangle.Fill = greenColor;
        }

        private void iconPanelImageTapped(object sender, TappedRoutedEventArgs e)
        {
            buttonLines.IsChecked = false;
            buttonPoints.IsChecked = false;
            LinesButtonUnchecked(sender, e);
            PointsButtonUnchecked(sender, e);

            Image selectedImage = sender as Image;
            BitmapImage selectedSource = selectedImage.Source as BitmapImage;
            Uri selectedUri = selectedSource.UriSource;

            myController.SetIconAnnotationSelectedFlag(true);
            myController.SetSelectedIconPath(selectedUri.AbsoluteUri);
        }

        private void PointerOverTrashBin(object sender, PointerRoutedEventArgs e)
        {
            TrashBinOpenImage.Opacity = 1;
            TrashBinClosedImage.Opacity = 0;
        }

        private void PointerLeftTrashBin(object sender, PointerRoutedEventArgs e)
        {
            TrashBinOpenImage.Opacity = 0;
            TrashBinClosedImage.Opacity = 1;
        }

        private void LinesButtonChecked(object sender, RoutedEventArgs e)
        {
            buttonLinesBorder.Background = buttonCheckedColor;
        }

        private void LinesButtonUnchecked(object sender, RoutedEventArgs e)
        {
            buttonLinesBorder.Background = buttonUncheckedColor;
        }

        private void PointsButtonChecked(object sender, RoutedEventArgs e)
        {
            buttonPointsBorder.Background = buttonCheckedColor;
        }

        private void PointsButtonUnchecked(object sender, RoutedEventArgs e)
        {
            buttonPointsBorder.Background = buttonUncheckedColor;
        }

        private void EraseAllButtonClicked(object sender, RoutedEventArgs e)
        {
            imagesPanel.Children.Clear();
            drawingPanel.Children.Clear();
        }

        private void ExitButtonClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void IconImage_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            /*dynamic contextualizedObject = Convert.ChangeType(sender, sender.GetType());
            contextualizedObject.Opacity = 0.4;*/
            if ("Windows.UI.Xaml.Controls.Image" == sender.GetType().ToString())
            {
                Image selectedElement = sender as Image;
                selectedElement.Opacity = 0.4;
            }
            else
            {
                Polyline selectedElement = sender as Polyline;
                selectedElement.Opacity = 0.4;
            }
        }

        private void IconImage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            CompositeTransform myTransform;
            /*dynamic contextualizedObject = Convert.ChangeType(sender, sender.GetType());
            myTransform = (CompositeTransform)contextualizedObject.RenderTransform;*/

            if ("Windows.UI.Xaml.Controls.Image" == sender.GetType().ToString())
            {
                Image selectedElement = sender as Image;
                myTransform = selectedElement.RenderTransform as CompositeTransform;
            }
            else
            {
                Polyline selectedElement = sender as Polyline;
                myTransform = selectedElement.RenderTransform as CompositeTransform;
            }

            myTransform.Rotation += e.Delta.Rotation;
            myTransform.ScaleX *= e.Delta.Scale;
            myTransform.ScaleY *= e.Delta.Scale;
            myTransform.TranslateX += e.Delta.Translation.X;
            myTransform.TranslateY += e.Delta.Translation.Y;
        }

        private void IconImage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            /*dynamic contextualizedObject = Convert.ChangeType(sender, sender.GetType());
            contextualizedObject.Opacity = 1;*/

            /*if (TrashBinOpenImage->Opacity == 1)
            {
                unsigned int idx;
                imagesPanel->Children->IndexOf(selectedElement, &idx);
                imagesPanel->Children->RemoveAt(idx);
            }*/

            if ("Windows.UI.Xaml.Controls.Image" == sender.GetType().ToString())
            {
                Image selectedElement = sender as Image;
                selectedElement.Opacity = 1;

                if (TrashBinOpenImage.Opacity == 1)
                {
                    int idx = imagesPanel.Children.IndexOf(selectedElement);
                    imagesPanel.Children.RemoveAt(idx);
                }
            }
            else
            {
                Polyline selectedElement = sender as Polyline;
                selectedElement.Opacity = 1;

                if (TrashBinOpenImage.Opacity == 1)
                {
                    int idx = drawingPanel.Children.IndexOf(selectedElement);
                    drawingPanel.Children.RemoveAt(idx);
                }
            }
        }

        private void EnteringRectangle(object sender, PointerRoutedEventArgs e)
        {
            ColoredRectangle.Fill = goldColor;
        }

        private void HoldingRectangle(object sender, HoldingRoutedEventArgs e)
        {
            ColoredRectangle.Fill = tealColor;
        }

        private void CreateIconAnnotation(TappedRoutedEventArgs e)
        {
            // Annotation path retrieval
            Uri iconUri = new Uri(myController.GetSelectedIconPath());
            BitmapImage iconBitmap = new BitmapImage(iconUri);
            Image iconImage = new Image();
            iconImage.Source = iconBitmap;

            // Annotation dimension and position
            Point tappedPosition = e.GetPosition(imagesPanel);
            iconImage.Width = 150; iconImage.Height = 150;
            iconImage.HorizontalAlignment = HorizontalAlignment.Left;
            iconImage.VerticalAlignment = VerticalAlignment.Top;
            imagesPanel.Children.Add(iconImage);
            iconImage.Margin = new Thickness(tappedPosition.X, tappedPosition.Y, 0, 0);

            // Annotation manipulators
            iconImage.ManipulationStarted += IconImage_ManipulationStarted;
            iconImage.ManipulationDelta += IconImage_ManipulationDelta;
            iconImage.ManipulationCompleted += IconImage_ManipulationCompleted;
            iconImage.ManipulationMode = ManipulationModes.Rotate | ManipulationModes.Scale | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            iconImage.RenderTransform = new CompositeTransform();
        }

        private void PreparePointAnnotation(TappedRoutedEventArgs e)
        {
            double initial_point_distance = 5.0;
            double angle;
            double val = PI / 180.0f;

            int counter;

            //create enough points to make a round shape
            for (counter = 0; counter <= 360; counter = counter + 18)
            {
                //gets the new angle value
                angle = counter * val;

                //Calculates trigonometric values of the point
                double cosComponent = Math.Cos(angle) * initial_point_distance;
                double senComponent = Math.Sin(angle) * initial_point_distance;

                //Calculates the new point
                double transfX = ((cosComponent) - (senComponent));
                double transfY = ((senComponent) + (cosComponent));

                //assigns the results
                LineAnnotation.Points.Add(new Point((transfX) + (e.GetPosition(imagesPanel).X), (transfY) + (e.GetPosition(imagesPanel).Y)));
            }
        }

        private void CreateButtonsColor()
        {
            Windows.UI.Color tempRGBAColor;
            tempRGBAColor.R = 29;
            tempRGBAColor.G = 122;
            tempRGBAColor.B = 216;
            tempRGBAColor.A = 255;
            buttonCheckedColor = new SolidColorBrush(tempRGBAColor);

            tempRGBAColor.R = 182;
            tempRGBAColor.G = 175;
            tempRGBAColor.B = 175;
            tempRGBAColor.A = 255;
            buttonUncheckedColor = new SolidColorBrush(tempRGBAColor);
        }

        private void ResetLineAnnotation()
        {
            LineAnnotation = new Polyline();
            LineAnnotation.Stroke = new SolidColorBrush(Windows.UI.Colors.Aquamarine);
            LineAnnotation.StrokeThickness = 7;
            LineAnnotation.HorizontalAlignment = HorizontalAlignment.Left;
            LineAnnotation.VerticalAlignment = VerticalAlignment.Top;
            LineAnnotation.ManipulationStarted += IconImage_ManipulationStarted;
            LineAnnotation.ManipulationDelta += IconImage_ManipulationDelta;
            LineAnnotation.ManipulationCompleted += IconImage_ManipulationCompleted;
            LineAnnotation.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            LineAnnotation.RenderTransform = new CompositeTransform();
        }
    }
}
