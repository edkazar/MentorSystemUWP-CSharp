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

        ///////////////// Variables for the socket communication
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

        ////////////////////////////////////////////////////////////////////// START OF SOCKET CODE
        /// <summary>
        /// Callback executed upon a new client's connection request.
        /// In here, the socket is bind and read to obtain the 
        /// info that is being transmitted. Although the connection
        /// is made, the data is not being transmitted correctly to
        /// the system.
        /// Things I've noticed:
        /// 1- Right now, it just puts a blank image as a background.
        /// Most likely because the new image has not all the information.
        /// 2- I tried to put the read method in a loop to populate the 
        /// buffer. However, the condition (bytes read > 0) is laways true,
        /// which means the stream continuously send info instead on delimiting
        /// it to "single images" per request.
        /// 3- I'm not doing any image resizing yet (640x480 to 1920x1080).
        /// </summary>
        private async void OnConnected(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            if (connectedSocket != null)
            {
                connectedSocket.Dispose();
                connectedSocket = null;
            }
            // Obtain the socket that is sending the info
            connectedSocket = args.Socket;
            Debug.WriteLine("Client has connected"); // It does connect

            // Get the socket's stream
            Stream streamIn = connectedSocket.InputStream.AsStreamForRead();

            // Size of buffer
            int maxSize = 3 * 640 * 480; //Screen res + 3 channels

            // Buffer to store the stream data into
            byte[] buffer = new byte[maxSize];

            //int offset = 0;
            //int bytesRead;
            //do
            //{

            // Asynchronously read the incoming stream and store it in our buffer. 
            // Having it in a loop didn't help either
            await streamIn.ReadAsync(buffer, 0, maxSize);
                //offset += bytesRead;
            //} while (bytesRead > 0);
            Debug.WriteLine("Left loop");

            // Create a stream that can be used as input for an image
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();

            // Asynchronously copy the read stream into this new stream
            await randomAccessStream.WriteAsync(buffer.AsBuffer());

            // Go to the stream's beginning
            randomAccessStream.Seek(0);

            // Give the control the the UI thread
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                // Create a bitmap
                var bitmap = new BitmapImage();

                // Assign the read stream as the bitmap's content
                bitmap.SetSource(randomAccessStream);

                // Use the created bitmap as the background image's source
                BackgroundImage.Source = bitmap;
            });

            ///
            /// Several other attemps.
            ///
            /*var inputStream = await connectedSocket.InputStream.ReadAsync();


            DataReader reader = new DataReader(connectedSocket.InputStream);
            reader.InputStreamOptions = InputStreamOptions.Partial;
            await reader.LoadAsync(250);
            var dataString = reader.ReadString(reader.UnconsumedBufferLength);
            greetingOutput.Text = "Stream: " + dataString;*/


            //Stream streamIn = connectedSocket.InputStream.AsStreamForRead();
            //IBuffer result = new Windows.Storage.Streams.Buffer(921600);
            //await connectedSocket.InputStream.ReadAsync(result, result.Capacity, InputStreamOptions.Partial);

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

        ////////////////////////////////////////////////////////////////////// END OF SOCKET CODE


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
